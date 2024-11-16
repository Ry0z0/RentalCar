import { Car } from './../../search-result/list-search/list-search.component';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
export interface GetCarByCarOwner {
  id: string; // Guid in C# is equivalent to string in TypeScript
  name: string; // Required string
  licensePlate: string; // Required string
  brand: string; // Required string
  model: string; // Required string
  color: string; // Required string
  numberOfSeats: number; // Integer value
  productionYears: number; // Integer value for year of production
  transmissionType: string; // Required string
  fuelType: string; // Required string
  mileage: number; // Integer representing mileage
  fuelConsumption: number; // Double value representing fuel consumption
  basePrice: number; // Decimal value in C# is equivalent to number in TypeScript
  deposit: number; // Decimal value for deposit amount
  address: string; // Required string
  description?: string; // Optional string (nullable in C#)
  additionalFunctions?: string; // Optional string (nullable in C#)
  termsOfUse?: string; // Optional string (nullable in C#)
  images?: string; // Optional string (nullable in C#)
  active: boolean; // Boolean value representing if the car is active
  carOwnerId: string; // Guid for the CarOwner
  carOwner?: string; // Optional string (nullable in C#)
  noOfRides: number;
  ratings: number;
}
export interface ListCarResponse {
  cars: GetCarByCarOwner[];
}
export interface Booking {
  id: string;
  bookingNo: string | null;
  startDateTime: string;
  endDateTime: string;
  driversInformation: string | null;
  paymentMethod: string;
  status: string;
  customerId: string;
  carId: string;
  feedbackId: string | null;
}

export interface BookingResponse {
  bookings: Booking[];
}
export interface avgOfCar {
  averageRatings: number;
}
@Component({
  selector: 'app-thumbview',
  templateUrl: './thumbview.component.html',
  styleUrl: './thumbview.component.scss',
})
export class ThumbviewComponent {
  constructor(private router: Router, private http: HttpClient) {}
  showModalConfirmDeposit = false;
  showModalConfirmPayment = false;
  selectedBookingId: string | null = null; // Add this to your component
  openModalConfirmDeposit(carId: string) {
    console.log(this.carBookings);
    this.selectedBookingId = this.carBookings[carId] || null; // Lấy bookingId từ carBookings
    if (this.selectedBookingId) {
      this.showModalConfirmDeposit = true;
      console.log(this.selectedBookingId);
    } else {
      alert('No booking found for this car.');
    }
  }
  
  openModalConfirmPayment(carId: string) {
    this.selectedBookingId = this.carBookings[carId] || null; // Lấy bookingId từ carBookings
    if (this.selectedBookingId) {
      this.showModalConfirmPayment = true;
      console.log(this.selectedBookingId);
    } else {
      alert('No booking found for this car.');
    }
  }
  

  closeModalConfirmDeposit() {
    this.showModalConfirmDeposit = false;
    this.selectedBookingId = null; // Reset the selected booking ID
  }
  closeModalConfirmPayment() {
    this.showModalConfirmPayment = false;
    this.selectedBookingId = null; // Reset the selected booking ID
  }
  confirmDeposit() {
    // Handle logout logic here
    this.closeModalConfirmDeposit();
  }
  confirmPayment() {
    // Handle logout logic here
    this.closeModalConfirmPayment();
  }

  gotoCarDetail(carId: string): void {
    this.router.navigate([`/car-detail/${carId}`]);
  }

  gotoAddCar() {
    this.router.navigate(['/add-car']);
  }
  gotoHomePage() {
    this.router.navigate(['/home']);
  }
  avgRatings: { [carId: string]: number } = {};

  getAvgCar(carOwnerId: string, carId: string): void {
    this.http
      .get<{ averageRatings: number }>(
        `/api/Feedback/GetAverageRatings/${carOwnerId}`,
        {}
      )
      .subscribe((response) => {
        console.log(`Rating for car ${carId}: `, response.averageRatings);
        this.avgRatings[carId] = response.averageRatings;
      });
  }

  carss: any[] = [];
  bookings: any[] = [];
  carStatuses: { [carId: string]: string } = {};
  showPopup = false;
  popupMessage = '';
  carBookings: { [carId: string]: string | null } = {};
  ngOnInit(): void {
    this.loadCars();
  }

  loadCars(): void {
    const carOwnerId = localStorage.getItem('userId');

    if (!carOwnerId) {
        console.error('No car owner ID found in local storage');
        return;
    }

    this.http.get<ListCarResponse>(`/api/Car/SearchByOwner/${carOwnerId}`).subscribe(
        (response) => {
            this.carss = response.cars;
            this.carss.forEach((car) => {
                this.carStatuses[car.id] = 'Loading...';
            });

            this.loadCarStatuses(); // Gọi API để lấy trạng thái xe
        },
        (error) => {
            console.error('Error fetching cars by owner', error);
        }
    );
}


loadCarStatuses(): void {
  const carOwnerId = localStorage.getItem('userId');

  if (!carOwnerId) {
      console.error('No car owner ID found in local storage');
      return;
  }

  this.http.get<any>(`/api/Car/GetBookingStatus/${carOwnerId}`).subscribe(
      (response) => {
          const carStatuses = response.carStatuses;

          if (carStatuses && carStatuses.length > 0) {
            console.log("Car status: ", carStatuses);
              carStatuses.forEach((statusObj: any) => {
                  this.carStatuses[statusObj.carId] = statusObj.status;
                  this.carBookings[statusObj.carId] = statusObj.bookingId; // Lưu bookingId vào carBookings
              });
          } else {
              console.error('Car statuses array is empty or undefined');
          }
      },
      (error) => {
          console.error('Error fetching car statuses', error);
      }
  );
}




  closePopup(): void {
    this.showPopup = false;
    this.popupMessage = '';
  }

  public getFormartLocation(address: string): string {
    let addressParts = address.split('|');
    let location = '';

    // Đảm bảo mảng addressParts có ít nhất 3 phần tử
    if (addressParts.length >= 3) {
      location = ` ${addressParts[1].replace(
        /^(Huyện|Thị xã|Quận) /,
        ''
      )}, ${addressParts[0].replace(/^(Thành phố|Tỉnh|Phường) /, '')}`;
    }
    return location;
  }

  public getFormatPrice(price: string): string {
    return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  }
  btnConfirmPayment(bookingId: string | null): void {
    const payload = { status: 'Completed' };
    this.http.patch(`/api/Booking/${bookingId}/status`, payload).subscribe(
      (response) => {
        // Xử lý response khi thành công
        alert('Booking status updated successfully');
        this.loadCarStatuses();
      },
      (error) => {
        // Xử lý lỗi khi có vấn đề xảy ra

        console.error('Error updating booking status', error);

        alert('Failed to update booking status');
      }
    );
    this.closeModalConfirmPayment();
  }
  btnConfirmDeposit(bookingId: string | null): void {
    const payload = { status: 'Confirmed' };
    // Gửi yêu cầu PATCH đến API với payload
    console.log(bookingId);
    this.http.patch(`/api/Booking/${bookingId}/status`, payload).subscribe(
      (response) => {
        this.loadCarStatuses();
        alert('Booking status updated successfully');
      },
      (error) => {
        // Xử lý lỗi khi có vấn đề xảy ra
        console.error('Error updating booking status', error);
        alert('Failed to update booking status');
      }
    );
    this.closeModalConfirmDeposit();
  }
}
