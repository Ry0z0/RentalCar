import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
  Renderer2,
} from '@angular/core';
import { setPostSignalSetFn } from '@angular/core/primitives/signals';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
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

export interface Customer {
  id: string;
  name: string;
  dateOfBirth: Date | null;
  nationalIdNo: string | null;
  phoneNo: string | null;
  email: string | null;
  passwordHash: string;
  address: string | null;
  drivingLicense: string | null;
  wallet: string | null;
}

export interface CustomerResponse {
  customer: Customer;
}

export interface Car {
  id: string; // uniqueidentifier -> string
  name: string; // nvarchar(100) -> string
  licensePlate: string; // nvarchar(20) -> string
  brand: string; // nvarchar(50) -> string
  model: string; // nvarchar(50) -> string
  color: string; // nvarchar(20) -> string
  numberOfSeats: number; // int -> number
  productionYears: number; // int -> number
  transmissionType: string; // nvarchar(50) -> string
  fuelType: string; // nvarchar(50) -> string
  mileage: number; // int -> number
  fuelConsumption: number; // float -> number
  basePrice: number; // decimal(18, 2) -> number
  deposit: number; // decimal(18, 2) -> number
  address: string; // nvarchar(200) -> string
  description?: string; // nvarchar(1000) -> string (optional)
  additionalFunctions?: string; // nvarchar(1000) -> string (optional)
  termsOfUse?: string; // nvarchar(1000) -> string (optional)
  images?: string; // nvarchar(1000) -> string (optional)
  carOwnerId: string; // uniqueidentifier -> string
}
export interface CarResponse {
  car: Car;
}
export interface NumberOfBooking {
  numberOfBooking: number;
}
export interface avgOfCar {
  averageRatings: number;
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
  booking: Booking;
}
@Component({
  selector: 'app-book-car',
  templateUrl: './book-car.component.html',
  styleUrl: './book-car.component.scss',
})
export class BookCarComponent implements OnInit, AfterViewInit {
  @ViewChild('process1') process1!: ElementRef;
  @ViewChild('process2') process2!: ElementRef;
  @ViewChild('process3') process3!: ElementRef;

  @ViewChild('bookCar1') bookCar1!: ElementRef;
  @ViewChild('bookCar2') bookCar2!: ElementRef;
  @ViewChild('bookCar3') bookCar3!: ElementRef;

  @ViewChild('pcBooking') pcBooking!: ElementRef;
  @ViewChild('pcPreview') pcPreview!: ElementRef;

  @ViewChild('booking') booking!: ElementRef;
  @ViewChild('preview') preview!: ElementRef;

  currentStepIndex = 0;
  steps: ElementRef[] = [];
  contents: ElementRef[] = [];
  steps1: ElementRef[] = [];
  contents1: ElementRef[] = [];
  bookingData: any = {
    bookingId: '',
    bookingNo: 'string',
    startDateTime: '',
    endDateTime: '',
    fullName: '',
    driverInfo: 'string',
    dob: '',
    phoneNum: '',
    email: '',
    nationalID: '',
    drivingLicense: 'string',
    cityD: '',
    districtD: '',
    wardD: '',
    houseNumberD: '',
    numOfDay: '',
    paymentMethod: '',
    status: 'Confirmed',
  };
  customerData: any = {
    fullName: '',
    dob: '',
    phoneNum: '',
    email: '',
    nationalID: '',
    drivingLicense: '',
    city: '',
    district: '',
    ward: '',
    houseNumber: '',
    wallet: '',
    walletU: '',
  };
  driverData: any = {
    fullName: '',
    dob: '',
    phoneNum: '',
    email: '',
    nationalID: '',
    drivingLicense: '',
    city: '',
    district: '',
    ward: '',
    houseNumber: '',
  };

  carData: any = {
    color: '',
    licensePlate: '',
    location: '',
    address: '',
    fuelType: '',
    basePriceF: '',
    basePrice: '',
    deposit: '',
    depositF: '',
    description: '',
    nor: '',
    images: '',
  };
  user: Customer = {
    id: '',
    name: '',
    dateOfBirth: new Date(),
    nationalIdNo: '',
    phoneNo: '',
    email: '',
    passwordHash: '',
    address: '',
    drivingLicense: '',
    wallet: '',
  };
  cities: City[] = [];
  districts: District[] = [];
  wards: Ward[] = [];
  // cityID: string = '';
  // districtID: string = '';
  loading: boolean = true;
  isFileInputDisabled: boolean = true;

  carId: string | null = null;
  pickupDate: string | null = null;
  pickupTime: string | null = null;
  dropoffDate: string | null = null;
  dropoffTime: string | null = null;
  rentalDays: number = 0;
  constructor(
    private renderer: Renderer2,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private router: Router,
    private activeRoute: ActivatedRoute
  ) {}
  ngOnInit(): void {
    this.activeRoute.queryParamMap.subscribe((params) => {
      this.pickupDate = params.get('pickupDate');
      this.pickupTime = params.get('pickupTime');
      this.dropoffDate = params.get('dropoffDate');
      this.dropoffTime = params.get('dropoffTime');
    });

    // Combine pickup date and time into a datetime-local format
    if (this.pickupDate && this.pickupTime) {
      this.bookingData.startDateTime = `${this.pickupDate}T${this.pickupTime}`;
    }

    // Combine dropoff date and time into a datetime-local format
    if (this.dropoffDate && this.dropoffTime) {
      this.bookingData.endDateTime = `${this.dropoffDate}T${this.dropoffTime}`;
    }
    this.activeRoute.paramMap.subscribe((params) => {
      this.carId = params.get('id');
    });
    this.rentalDays = this.calculateRentalDays;
    this.getCity();
    this.getCustomerInfo();
    this.getCarInfo();
    this.getNumberBookingOfCar();
  }

  ngAfterViewInit(): void {
    this.steps = [this.process1, this.process2, this.process3];
    this.contents = [this.bookCar1, this.bookCar2, this.bookCar3];
  }

  onNext() {
    if (this.currentStepIndex < this.steps.length - 1) {
      this.currentStepIndex++;
      this.goToStep(this.currentStepIndex);
    }
  }

  onPrevious() {
    if (this.currentStepIndex > 0) {
      this.currentStepIndex--;
      this.goToStep(this.currentStepIndex);
    }
  }
  goToStep(index: number) {
    this.currentStepIndex = index; // Update the currentStepIndex to sync with the step clicked
    this.steps.forEach((step) =>
      this.renderer.removeClass(step.nativeElement, 'current')
    );
    this.contents.forEach((content) =>
      this.renderer.removeClass(content.nativeElement, 'active')
    );

    this.renderer.addClass(this.steps[index].nativeElement, 'current');
    this.renderer.addClass(this.contents[index].nativeElement, 'active');
  }

  selectCity(city: City): void {
    this.bookingData.city = city.name;
    this.bookingData.district = '';
    this.bookingData.ward = '';
    this.districts = [];
    this.wards = [];
    this.getDistricts(city.id);
  }
  selectDistrict(district: District): void {
    this.bookingData.district = district.name;
    this.bookingData.ward = '';
    this.wards = [];
    this.getWards(district.id);
  }
  selectWard(ward: Ward): void {
    this.bookingData.ward = ward.name;
  }
  get getLocation(): string {
    let district = this.carData.district.replace(
      /^(Huyện|Thị xã|Thành phố) /,
      ''
    );
    let city = this.carData.city.replace(/^(Thành phố|Tỉnh) /, '');

    // Ghép các chuỗi đã được loại bỏ tiền tố
    let location: string = `${district}, ${city}`;
    if (location === ', ') {
      location = 'No address yet';
    }
    return location;
  }

  get calculateRentalDays(): number {
    const startTime = new Date(this.bookingData.startDateTime);
    const endTime = new Date(this.bookingData.endDateTime);
    // alert(startTime)
    if (isNaN(startTime.getTime()) || isNaN(endTime.getTime())) {
      return 0;
    }
    const diffInMs = endTime.getTime() - startTime.getTime();
    const diffInHours = diffInMs / (1000 * 60 * 60);

    if (diffInHours < 12) {
      return 0.5;
    } else {
      return Math.ceil(diffInHours / 24) + 1;
    }
  }

  get calculateTotalCost(): number | string {
    const totalCostt = this.calculateRentalDays * this.carData.basePrice;

    if (this.calculateRentalDays === 0) {
      return this.carData.basePriceF;
    }
    let total = totalCostt + '';

    return total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
  }
  customerWallet: number = 0;

  private formatDateTime(dateString: string): string {
    // Convert the string to a Date object
    const dateObj = new Date(dateString);

    // Check if the date is valid
    if (isNaN(dateObj.getTime())) {
      return '';
    }

    const formattedDate = dateObj.toLocaleDateString('en-GB');
    const formattedTime = dateObj.toLocaleTimeString('en-US', {
      hour: '2-digit',
      minute: '2-digit',
      hour12: true,
    });

    return `${formattedDate} - ${formattedTime}`;
  }
  public getFormattedDateTime(dateString: string): string {
    return this.formatDateTime(dateString);
  }
  compareValues(value1: string | number, value2: string | number): boolean {
    return Number(value1) >= Number(value2);
  }

  gotoHomePage() {
    this.router.navigate(['/home']);
  }

  gotoSearch() {
    this.router.navigate(['/search']);
  }
  // ================================================== API ==================================================

  // GetCity
  getCity(): void {
    this.http.get<CityResponse>('/api/Address/GetAddressCitys', {}).subscribe(
      (response) => {
        this.cities = response.addressCity;
        this.loading = false;
      },
      (error) => {
        // Handle login error response
        if (error.error && error.error.message) {
          alert('Failed to retrieve cities: ' + error.error.message);
        } else {
          alert(
            'Failed to retrieve cities: ' + error.message || 'Unknown error'
          );
        }
        this.loading = false;
      }
    );
  }

  // GetDistrict
  getDistricts(cityID: string): void {
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
          // Handle login error response
          if (error.error && error.error.message) {
            alert('Failed to retrieve cities: ' + error.error.message);
          } else {
            alert(
              'Failed to retrieve cities: ' + error.message || 'Unknown error'
            );
          }
        }
      );
  }

  // Get Ward
  getWards(districtID: string): void {
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
          // Handle login error response
          if (error.error && error.error.message) {
            alert('Failed to retrieve cities: ' + error.error.message);
          } else {
            alert(
              'Failed to retrieve cities: ' + error.message || 'Unknown error'
            );
          }
        }
      );
  }

  // Get Customer Infomation

  getCustomerInfo(): void {
    const userId = localStorage.getItem('userId');
    console.log(userId);

    this.http
      .get<CustomerResponse>(`/api/Customer/${userId}`, {})
      .subscribe((response) => {
        console.log(response);
        this.customerData.fullName = response.customer.name;
        this.customerData.dob = response.customer.dateOfBirth;

        // Convert the date to a Date object
        const dateObj = new Date(this.customerData.dob);

        // Extract the date in `YYYY-MM-DD` format
        this.customerData.dob = dateObj.toISOString().split('T')[0];
        this.customerData.email = response.customer.email;
        this.customerData.phoneNum = response.customer.phoneNo;
        this.customerData.nationalID = response.customer.nationalIdNo;
        this.user = response.customer;
        if (
          response.customer.address &&
          typeof response.customer.address === 'string'
        ) {
          let addressParts = response.customer.address.split('|');

          // Gán các giá trị vào các trường tương ứng

          this.customerData.city = addressParts[0];
          this.customerData.district = addressParts[1];
          this.customerData.ward = addressParts[2];
          this.customerData.houseNumber = addressParts[3];
        }
        this.customerData.wallet = response.customer.wallet ?? 0;
        this.customerData.walletU = this.customerData.wallet;
        this.customerData.wallet = this.customerData.wallet
          ? this.customerData.wallet
              .toString()
              .replace(/\B(?=(\d{3})+(?!\d))/g, ',')
          : '0';
        this.bookingData.walletU = response.customer.wallet ?? 0;
      });
  }

  // Get Car Infomation

  getCarInfo(): void {
    this.http.get<CarResponse>(`/api/Car/${this.carId}`, {}).subscribe(
      (response) => {
        console.log(response.car);
        let WardD = '';
        let districtD = '';
        let cityD = '';
        this.carData.fullName = response.car.name;
        this.carData.images = response.car.images;
        this.getAvgCar(response.car.carOwnerId);

        if (response.car.address && typeof response.car.address === 'string') {
          let addressParts = response.car.address.split('|');

          // Đảm bảo mảng addressParts có ít nhất 3 phần tử
          if (addressParts.length >= 3) {
            WardD = addressParts[2].replace(/^(Xã|Phường) /, '');
            districtD = addressParts[1].replace(/^(Huyện|Thị xã|Quận) /, '');
            cityD = addressParts[0].replace(/^(Thành phố|Tỉnh|Phường) /, '');
          } else {
            console.error(
              'Address format is incorrect. Expected at least 3 parts.'
            );
          }
        }
        this.carData.location = `${WardD}, ${districtD}, ${cityD}`;
        this.carData.address = `${districtD}, ${cityD}`;
        this.carData.basePrice = response.car.basePrice;
        this.carData.deposit = response.car.deposit;
        this.carData.basePriceF = response.car.basePrice;

        this.carData.basePriceF = this.carData.basePriceF
          .toString()
          .replace(/\B(?=(\d{3})+(?!\d))/g, ',');

        this.carData.depositF = response.car.deposit;

        // Convert number to string and apply the replace method
        this.carData.depositF = this.carData.depositF
          .toString()
          .replace(/\B(?=(\d{3})+(?!\d))/g, ',');
      },
      (error) => {
        console.log('Error Response:', error);
      }
    );
  }

  // Get Number of Booking of Car
  getNumberBookingOfCar(): void {
    this.http
      .get<NumberOfBooking>(
        `/api/Booking/GetNumberOfBookingOfCar/${this.carId}`,
        {}
      )
      .subscribe(
        (response) => {
          console.log(response.numberOfBooking);
          this.carData.nor = response.numberOfBooking;

          if (this.carData.nor === '') {
            this.carData.nor = '0';
          }
        },
        (error) => {
          console.error('AddCar failed', error);
          const errorMessage =
            error?.error?.message || 'An unexpected error occurred';
          alert('AddCar failed: ' + errorMessage);

          console.log('Error Response:', error);
          // console.log('Car Data Sent:', bookingData);
        }
      );
  }

  updateCusWallet(cusId: string | null): void {
    if (this.bookingData.paymentMethod === 'MyWallet') {
      this.customerWallet =
        Number(this.customerData.walletU) - Number(this.carData.deposit);
    }

    // console.log('newPriceInWallet: ' + this.customerWallet);
    // let updatedCustomerWallet!: Customer;
    // updatedCustomerWallet = {
    //   ...this.user,
    //   wallet: this.customerWallet.toString(),
    // };

    // console.log('id: ' + updatedCustomerWallet.id);
    this.http
      .patch<Customer>(`/api/Customer/${cusId}/wallet`, {
        wallet: this.customerWallet.toString(),
      })
      .subscribe(
        (response) => {
          console.log(response);
        },
        (error) => {
          console.error('AddCar failed', error);
          const errorMessage =
            error?.error?.message || 'An unexpected error occurred';
          alert('AddCar failed: ' + errorMessage);

          console.log('Error Response:', error);
        }
      );
  }
  avg: number = 0;
  getAvgCar(carOwnerId: string): void {
    this.http
      .get<avgOfCar>(`/api/Feedback/GetAverageRatings/${carOwnerId}`, {})
      .subscribe((respone) => {
        this.avg = respone.averageRatings;
      });
  }

  addBooking(): void {
    const userId = localStorage.getItem('userId');
    console.log(userId);

    if (
      this.bookingData.paymentMethod === 'Cash' ||
      this.bookingData.paymentMethod === 'BankTf'
    ) {
      this.bookingData.status = 'Pending deposit';
    }
    const bookingData = {
      bookingNo: this.bookingData.bookingNo,
      startDateTime: this.bookingData.startDateTime,
      endDateTime: this.bookingData.endDateTime,
      driversInformation: this.bookingData.driverInfo,
      paymentMethod: this.bookingData.paymentMethod,
      status: this.bookingData.status,
      customerId: userId,
      carId: this.carId,
      feedbackId: null,
    };
    this.updateCusWallet(userId);
    console.log('Car Data Object:', bookingData);

    this.http.post<any>('/api/Booking', bookingData, {}).subscribe(
      (response: any) => {
        this.getBookingJustAdded();
        this.onNext();
        console.log('Add Booking successful', response);
        this.snackBar.open('Booking added successful!', 'Close', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'top',
        });
      },
      (error) => {
        console.error('AddCar failed', error);
        const errorMessage =
          error?.error?.message || 'An unexpected error occurred';
        alert('AddCar failed: ' + errorMessage);

        console.log('Error Response:', error);
        console.log('Car Data Sent:', bookingData);
      }
    );
  }

  startBooking: string = '';
  endBooking: string = '';
  bookingNumber: string | null = '';

  getBookingJustAdded(): void {
    const userId = localStorage.getItem('userId');
    this.http
      .get<BookingResponse>(
        `/api/Booking/GetLatestBookingOfCustomer/${userId}`,
        {}
      )
      .subscribe((response) => {
        this.startBooking = response.booking.startDateTime;
        this.endBooking = response.booking.endDateTime;
        this.bookingNumber = response.booking.bookingNo;
        this.bookingData.bookingId = response.booking.id;
      });
  }

  goHomepage() {
    this.router.navigate(['/home']);
  }
  goBookAnotherCar() {
    this.router.navigate(['/my-bookings']);
  }
  goViewBooking() {
    this.router.navigate(['/my-booking-detail/', this.bookingData.bookingId]);
  }
  gotoBookCar() {
    this.router.navigate(['book-car/']);
  }

 
}
