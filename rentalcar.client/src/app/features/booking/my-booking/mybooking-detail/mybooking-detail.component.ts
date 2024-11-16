import { NgClass, NgFor } from '@angular/common';
import {
  Component,
  ElementRef,
  ViewChild,
  Renderer2,
  OnInit,
  AfterViewInit,
} from '@angular/core';
import { HttpClient } from '@angular/common/http'; 
import { Router } from '@angular/router';
export interface BookingDetail {
  bookingNo: string;
  carId : string;
  customerId: string;
  driversInformation: string;
  endDateTime : Date;
  feedbackId: string;
  id: string;
  paymentMethod: string;
  startDateTime : Date;
  status: string;
}
export interface CarDetail {
  additionalFunctions: string;
  address : string;
  basePrice: number;
  brand: string;
  carOwnerId : Date;
  color: string;
  deposit: number;
  description: string;
  fuelConsumption : number;
  fuelType: string;
  id: string;
  images: string;
  licensePlate: string;
  mileage: number;
  model: string;
  name: string;
  numberOfSeats: number;
  productionYears: number;
  termsOfUse: string;
  transmissionType: string;
}
export interface CustomerDetail {
  address : string;
  dateOfBirth: Date;
  drivingLicense: string;
  email : string;
  id: string;
  name: string;
  nationalIdNo: number;
  passwordHash: string;
  phoneNo: number;
  wallet: number;
}
export interface BookingDetailResponse {
  booking: BookingDetail;
}
export interface CarDetailResponse {
  car: CarDetail;
}
export interface CustomerDetailResponse {
  customer: CustomerDetail;
}
export interface City {
  id: string;
  name: string;
}
export interface CityResponse {
  addressCity: City[];
}
export interface DistrictResponse {
  addressDistrict: District[];
}
export interface WardResponse {
  addressWard: Ward[];
}

export interface District {
  id: string;
  name: string;
  cityId: string;
}
export interface Ward {
  id: string;
  name: string;
  districtId: string;
}
@Component({
  selector: 'app-mybooking-detail',
  templateUrl: './mybooking-detail.component.html',
  styleUrl: './mybooking-detail.component.scss',
})
export class MybookingDetailComponent implements OnInit, AfterViewInit {
  @ViewChild('process1') process1!: ElementRef;
  @ViewChild('process2') process2!: ElementRef;
  @ViewChild('process3') process3!: ElementRef;

  @ViewChild('BookingInfo') BasicInfo!: ElementRef;
  @ViewChild('CarInfo') Details!: ElementRef;
  @ViewChild('ConfirmPayment') Princing!: ElementRef;
  currentStepIndex = 0;
  steps: ElementRef[] = [];
  contents: ElementRef[] = [];
  bookingDetail: BookingDetail | undefined;
  bookingId = sessionStorage.getItem('bookingDetailId') || "";
  CarDetail: CarDetail | undefined;
  cities: City[] = [];
  districts: District[] = [];
  wards: Ward[] = [];
  selectedFunctions: string[] = [];
  termsOfUse: string[] = [];
  cityID: string = '';
  districtID: string = '';
  CustomerDetail: CustomerDetail = {
    address: '',
    dateOfBirth: new Date(),
    drivingLicense: '',
    email: '',
    id: '',
    name: '',
    nationalIdNo: 0,
    passwordHash: '',
    phoneNo: 0,
    wallet: 0
  };
  constructor(private renderer: Renderer2,private router: Router, private http: HttpClient) {}
  
  ngOnInit(): void {
    this.getBookingDetail();
    this.getCity();
  }
  loading: boolean = true;
  getBookingDetail(): void {
    this.http.get<BookingDetailResponse>(`/api/Booking/${this.bookingId}`).subscribe(
      (response: BookingDetailResponse) => {
        this.bookingDetail = response.booking;
        console.log(this.bookingDetail);

        if (!this.bookingDetail?.carId) {
          console.warn('Car ID is not available.');
          return;
        }

        this.getCarDetail(this.bookingDetail.carId);
        this.getCustomerDetail(this.bookingDetail.customerId);
      },
      (error) => {
        if (error.error.message !== undefined) {
          alert('Login failed: ' + error.error.message);
        } else {
          alert('Login failed: ' + error.error);
        }
      }
    );
}
isChecked(functionName: string): boolean {
  return this.selectedFunctions.includes(functionName.toLowerCase());
}
isTermChecked(term: string): boolean {
  return this.termsOfUse.includes(term.toLowerCase());
}
getCarDetail(id: string): void {
  this.http.get<CarDetailResponse>(`/api/Car/${id}`).subscribe(
    (response: CarDetailResponse) => {
      this.CarDetail = response.car;
      
      if (this.CarDetail?.additionalFunctions) {
        this.selectedFunctions = this.CarDetail.additionalFunctions.split('|');
      }

      if (this.CarDetail?.termsOfUse) {
        this.termsOfUse = this.CarDetail.termsOfUse.split('|');
      }

      console.log(this.selectedFunctions); // Kiểm tra các chức năng đã chọn
      console.log(this.termsOfUse); // Kiểm tra các điều khoản đã chọn
    },
    (error) => {
      if (error.error.message !== undefined) {
        alert('Login failed: ' + error.error.message);
      } else {
        alert('Login failed: ' + error.error);
      }
    }
  );
}
  getCustomerDetail(customerId : string ): void {
    this.http.get<CustomerDetailResponse>(`/api/Customer/${customerId}`, {
    }).subscribe(
      (response: CustomerDetailResponse) => {
        this.CustomerDetail = response.customer;
        console.log(response);
      },
      (error) => {// Handle login error response
        //console.error('Login failed', error);
        if(error.error.message !== undefined) {
        alert('Login failed: ' + error.error.message);
        } else {
          alert('Login failed: ' + error.error);
        }
      }
    );
  }
  getCity(): void {
    this.http.get<CityResponse>('/api/Address/GetAddressCitys', {}).subscribe(
      (response) => {
        this.cities = response.addressCity;
        this.loading = false;
      },
      (error) => {
        this.handleError('get city', error);
      }
    );
  }

  // Hàm lấy dữ liệu quận/huyện dựa vào ID thành phố
  getDistrict(cityID: string): void {
    this.http
      .get<DistrictResponse>(
        `/api/Address/GetAllAddressDistrictOfCity?id=${cityID}`,
        {}
      )
      .subscribe(
        (response) => {
          this.districts = response.addressDistrict;
        },
        (error) => {
          this.handleError('get district', error);
        }
      );
  }

  // Hàm lấy dữ liệu phường/xã dựa vào ID quận/huyện
  getWard(districtID: string): void {
    this.http
      .get<WardResponse>(
        `/api/Address/GetAllAddressWardOfDistrict?id=${districtID}`,
        {}
      )
      .subscribe(
        (response) => {
          this.wards = response.addressWard;
        },
        (error) => {
          this.handleError('get ward', error);
        }
      );
  }
  onCityChange(event: Event): void {
    const selectedCityId = (event.target as HTMLSelectElement).value;
    this.cityID = selectedCityId;
    this.getDistrict(this.cityID);  // Lấy dữ liệu các quận/huyện tương ứng
  }
  // Hàm xử lý khi chọn quận/huyện
  onDistrictChange(event: Event): void {
    const selectedDistrictId = (event.target as HTMLSelectElement).value;
    this.districtID = selectedDistrictId;
    this.getWard(this.districtID);  // Lấy dữ liệu các phường/xã tương ứng
  }
  updateCustomerDetail(): void {
    if (this.CustomerDetail) {
      const updatedCustomerDetail = {
        ...this.CustomerDetail
      };
  
      this.http.put(`/api/Customer/${this.CustomerDetail.id}`, updatedCustomerDetail).subscribe(
        (response) => {
          alert('Customer details updated successfully!');
        },
        (error) => {
          this.handleError('update customer detail', error);
        }
      );
    } else {
      console.warn('No customer detail available to update.');
    }
  }
  handleError(context: string, error: any): void {
    if (error.error instanceof ErrorEvent) {
      // Client-side or network error
      console.error('An error occurred:', error.error.message);
    } else {
      // Backend error
      console.error(`Backend returned code ${error.status}, body was: ${error.error}`);
    }
    alert(`${context} failed: ${error.message || error.statusText}`);
  }
  getNumberOfDays(booking: BookingDetail): number {
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
  getTotalCost(booking: BookingDetail, car: CarDetail): number {
    const numberOfDays = this.getNumberOfDays(booking);
    return (car.basePrice * numberOfDays)
  }
  showModalReturnCar = false;
  openModalReturnCar() {
    this.showModalReturnCar = true;
  }
  ngAfterViewInit(): void {
    this.steps = [this.process1, this.process2, this.process3];
    this.contents = [this.BasicInfo, this.Details, this.Princing];

    this.steps.forEach((stepElement, index) => {
      this.renderer.listen(stepElement.nativeElement, 'click', () => {
        console.log(`Step ${index + 1} clicked`); // Debug log

        this.steps.forEach((step) =>
          this.renderer.removeClass(step.nativeElement, 'active')
        );
        this.contents.forEach((content) =>
          this.renderer.removeClass(content.nativeElement, 'current')
        );

        this.renderer.addClass(stepElement.nativeElement, 'active');
        this.renderer.addClass(this.contents[index].nativeElement, 'current');
      });
    });
  }


  // Give rating
  showModalGiveRating = false;
  openModalGiveRating() {
    this.showModalGiveRating = true;
  }
  closeModalGiveRating() {
    this.showModalGiveRating = false;
  }
  confirmGiveRating() {
    // Handle logout logic here
    this.closeModalGiveRating();
  }
  stars: number[] = [1, 2, 3, 4, 5];
  rating: number = 0;
  hoverRating: number = 0;

  highlightStars(rating: number): void {
    this.hoverRating = rating;
  }

  resetStars(): void {
    this.hoverRating = 0;
  }

  rate(rating: number): void {
    this.rating = rating;
  }

  gotoHomePage() {
    this.router.navigate(['/home']);
  }
  gotoMyBookings() {
    this.router.navigate(['/my-bookings']);
  }
}
