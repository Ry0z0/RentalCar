import { NgClass, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ICar } from '../../../../core/services/Admin/admin.service';
import { firstValueFrom } from 'rxjs';

export interface Booking {
  bookingNo: string;
  bookingId: string;
  carId: string;
  carName: string;
  startDateTime: Date;
  endDateTime: Date;
  basePrice: number;
  deposit: number;
  status: string;
}
export interface BookingDetail extends Booking{
  carImg: string;
}
export interface BookingResponse {
  bookingcars: Booking[];
}
export interface CarResponse{
  car: ICar;
}
@Component({
  selector: 'app-list-mybooking',
  templateUrl: './list-mybooking.component.html',
  styleUrl: './list-mybooking.component.scss',
})
export class ListMybookingComponent {
  userId = localStorage.getItem('userId') || '';
  bookings: BookingDetail[] = [];
  loading = true; // Track loading state
  exceedingAmount: number = 0;
  remainingAmount: number = 0;
  walletBalance: number = 0; // Số tiền hiện có trong ví
  bookingDetail: any;
  stars: number[] = [1, 2, 3, 4, 5];
  rating: number = 0;
  hoverRating: number = 0;
  reviewContent: string = '';
  reviewDate: string = '';
    selectedBookingId: string | null = null; // Thêm biến này để lưu bookingId khi mở popup

  constructor(private router: Router, private http: HttpClient) {}
  ngOnInit(): void {
    if (this.userId) {
      this.getBookings(this.userId);
      this.getWalletBalance(); // Lấy số dư trong ví từ localStorage
    } else {
      this.loading = false;
      alert('User ID is not set. Please log in again.');
    }
  }
  getBookings(userId: string): void {
    this.http.get<BookingResponse>(`/api/Booking/GetListBookingCar/${this.userId}`, {}).subscribe(
      async (response: BookingResponse) => {
        this.bookings = await Promise.all(response.bookingcars.map(async (booking) => {
          const carDetail = await this.getCarDetails(booking.carId);
          console.log(this.bookings);
          return {
            ...booking,
            carImg: carDetail.car.images // Gán hình ảnh xe vào BookingDetail
          } as BookingDetail;
        }));
        this.loading = false; // Stop loading once data is fetched
        console.log(this.bookings);
      },
      (error) => {
        this.loading = false; // Stop loading even if there's an error
        if (error.error.message !== undefined) {
          alert('Login failed: ' + error.error.message);
        } else {
          alert('Login failed: ' + error.error);
        }
      }
    );
  }
  

  getCarDetails(carId: string): Promise<CarResponse> {
    return firstValueFrom(this.http.get<CarResponse>(`/api/Car/${carId}`));
  }
  
  
  updateBookingStatus(id: string, status: string): void {
    // Tạo payload với thuộc tính `status` để gửi đến server
    const payload = { status: status };

    // Gửi yêu cầu PATCH đến API với payload
    this.http.patch(`/api/Booking/${id}/status`, payload)
        .subscribe(
            response => {
                // Xử lý response khi thành công
                alert('Booking status updated successfully');
                this.getBookings(this.userId); // Tải lại danh sách booking sau khi cập nhật thành công
            },
            error => {
                // Xử lý lỗi khi có vấn đề xảy ra
                console.error('Error updating booking status', error);
                alert('Failed to update booking status');
            }
        );
    }
    
  getNumberOfDays(booking: Booking): number {
    const startDateTime = new Date(booking.startDateTime).getTime();
    const endDateTime = new Date(booking.endDateTime).getTime();
    const millisecondsPerHour = 1000 * 60 * 60;
    const totalHours = (endDateTime - startDateTime) / millisecondsPerHour;

    if (totalHours < 12) {
      return 0.5;
    } else {
      return Math.ceil(totalHours / 24);
    }
  }
  getTotalPrice(booking: Booking): number {
    const numberOfDays = this.getNumberOfDays(booking);
    return booking.basePrice * numberOfDays;
  }
  getWalletBalance(): void {
    this.http.get<{ wallet: string }>(`/api/Customer/GetCustomerWallet/${this.userId}`).subscribe(
      (response) => {
        console.log("Wallet Balance: ", response); // Kiểm tra giá trị trả về
        this.walletBalance = Number(response.wallet); // Chuyển đổi chuỗi sang số và gán vào walletBalance
      },
      (error) => {
        console.error('Failed to fetch wallet balance', error);
      }
    );
  }
  handlePayment(booking: Booking): void {
    const totalPrice = this.getTotalPrice(booking);
    const deposit = booking.deposit;

    if (totalPrice > deposit) {
      this.remainingAmount = totalPrice - deposit;
      this.exceedingAmount = 0;
    } else {
      this.exceedingAmount = deposit - totalPrice;
      this.remainingAmount = 0;
    }

    // Kiểm tra xem ví có đủ tiền để thanh toán không
    if (this.remainingAmount > this.walletBalance) {
      alert('Your wallet balance is insufficient. Please top up your wallet.');
    }
  }
  viewDetails(id: string): void {
    console.log(id);
    sessionStorage.setItem('bookingDetailId', id);
    this.router.navigate([`/my-booking-detail/${id}`]);
  }
  showModalReturnCar = false;
  openModalReturnCar(booking: Booking) {
    this.handlePayment(booking); // Tính toán số tiền thừa hoặc còn lại
    this.getBookingIdForFb = booking.bookingId; // Assign the correct booking ID here
    this.showModalReturnCar = true;
  }
  closeModalReturnCar() {
    this.showModalReturnCar = false;
  }
  confirmReturnCar() {
    // Handle logout logic here
    this.closeModalReturnCar();
  }
  handleYesClick(): void {
    if (this.remainingAmount > this.walletBalance) {
      // Nếu số dư ví không đủ, điều hướng đến trang nạp tiền
      this.router.navigate(['/my-wallet']);
    } else {
      // Nếu đủ tiền, trừ số dư trong ví và cập nhật lại số dư
      const newBalance = this.walletBalance - this.remainingAmount;
      
      // Cập nhật lại số dư trong ví bằng cách gửi yêu cầu lên API
      this.updateWalletBalance(newBalance).subscribe(
        (response) => {
          // Sau khi cập nhật thành công, mở modal đánh giá
          this.walletBalance = newBalance; // Cập nhật giá trị trên giao diện
          this.openModalGiveRating(); // Mở modal đánh giá
        },
        (error) => {
          console.error('Failed to update wallet balance', error);
          alert('Failed to update wallet balance. Please try again.');
        }
      );
    }
  }
  
  updateWalletBalance(newBalance: number) {
    // API cập nhật số dư ví với URL mới
    return this.http.patch(`/api/Customer/${this.userId}/wallet`, { wallet: newBalance.toString() });
  }
  
  // Give rating
  showModalGiveRating = false;
  openModalGiveRating() {
    this.showModalReturnCar = false;
    this.showModalGiveRating = true;
  }
  closeModalGiveRating() {
    this.showModalGiveRating = false;
  }
  getBookingIdForFb: string =''
  confirmGiveRating(bookingId: string): void {
    const newStatus = 'Completed'; // Trạng thái mới
    // Gọi API để cập nhật trạng thái
    this.updateBookingStatus(this.getBookingIdForFb, newStatus);
    
    // Đóng modal đánh giá sau khi gửi thành công
    this.closeModalGiveRating();
  }
  closeModalAndComplete(bookingId: string): void {
    // Đóng modal
    this.closeModalGiveRating();
    console.log("Test:",this.getBookingIdForFb);
    // Gọi hàm cập nhật trạng thái đơn hàng thành 'Complete'
    this.updateBookingStatus(this.getBookingIdForFb, 'Complete');
  }
  handleCashPayment(bookingId: string): void {
    // Cập nhật trạng thái sang 'Pending Payment'
    this.updateBookingStatus(bookingId, 'Pending Payment');
  
    // Đóng modal sau khi cập nhật trạng thái
    this.closeModalReturnCar();
  }
  
  highlightStars(rating: number): void {
    this.hoverRating = rating;
  }

  resetStars(): void {
    this.hoverRating = 0;
  }

  rate(rating: number): void {
    this.rating = rating;
  }
  captureDateTime(): void {
    const now = new Date();
    this.reviewDate = now.toISOString(); // Formats the date in ISO 8601 (YYYY-MM-DDTHH:MM:SS.sssZ)
  }


  submitReview(): void {
    this.captureDateTime();
    const feedback = {
      content: this.reviewContent,
      ratings: this.rating,
      review: this.reviewContent,
      datetime: this.reviewDate,
      bookingId: this.getBookingIdForFb,
    };
    this.http.post<any>('/api/Feedback', feedback, {}).subscribe(
      (response) => {
        this.closeModalGiveRating();
      },
      (error) => {
        console.error('AddCar failed', error);
        const errorMessage =
          error?.error?.message || 'An unexpected error occurred';
        alert('AddCar failed: ' + errorMessage);

        console.log('Error Response:', error);
        console.log('feedback', feedback);
      }
    );
  }
  gotoHomePage() {
    this.router.navigate(['/home']);
  }
  gotoListCar() {
    this.router.navigate(['/my-list-car']);
  }
}
