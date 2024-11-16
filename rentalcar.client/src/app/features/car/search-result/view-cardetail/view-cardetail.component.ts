import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CardetailService } from '../../../../core/services/Cardetail/cardetail.service';
import { HttpClient } from '@angular/common/http';

export interface Car {
  Id: string;
  name: string;
  licensePlate: string;
  brand: string;
  model: string;
  color?: string;
  numberOfSeats: number;
  productionYears: number;
  transmissionType?: string;
  fuelType?: string;
  mileage: number;
  fuelConsumption: number;
  basePrice: number;
  deposit: number;
  address: string;
  description?: string;
  additionalFunctions?: string;
  termsOfUse?: string;
  images?: string;
  carOwnerId: string;
}

export interface CarRespone {
  car: Car;
}

export interface Booking {
  id: string;
  bookingNo: string;
  startDateTime: Date;
  endDateTime: Date;
  driversInformation: string;
  paymentMethod: string;
  status: string;
  customerId: string;
  carId: string;
  feedbackId: string;
}

export interface BookingRespone {
  booking: Booking;
}

export interface Feedback {
  id: string;
  ratings?: number;
  content: string;
  dateTime: Date;
  bookingId: string;
}

export interface FeedbackRespone {
  feedback: Feedback;
}
export interface avgOfCar {
  averageRatings: number;
}

@Component({
  selector: 'app-view-cardetail',
  templateUrl: './view-cardetail.component.html',
  styleUrls: ['./view-cardetail.component.scss'],
})
export class ViewCardetailComponent implements OnInit {
  car?: Car;
  numberOfRatings: number = 0;
  averageRatings: number = 0;
  carId: string | null = '';

  constructor(
    private carService: CardetailService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private http: HttpClient
  ) {}
  location: string = '';
  pickupDate: string = '';
  pickupTime: string = '';
  dropoffDate: string = '';
  dropoffTime: string = '';
  ngOnInit(): void {
    this.activeRoute.queryParams.subscribe((params) => {
      this.location = params['location'] || '';
      this.pickupDate = params['pickupDate'] || '';
      this.pickupTime = params['pickupTime'] || '';
      this.dropoffDate = params['dropoffDate'] || '';
      this.dropoffTime = params['dropoffTime'] || '';
    });
    this.activeRoute.paramMap.subscribe((params) => {
      this.carId = params.get('id');
    });
    this.loadCarData();
  }

  loadCarData() {
    this.carService.getCarById(this.carId).subscribe((response: CarRespone) => {
      this.car = response.car;
      this.getAvgCar(response.car.carOwnerId);
      console.log(this.car);

      // Lấy số lượng đánh giá và đánh giá trung bình dựa trên CarOwnerId
      if (this.car && this.car.carOwnerId) {
        this.carService
          .getNumberOfRatings(this.car.carOwnerId)
          .subscribe((numberOfRatings) => {
            console.log('Number Ratings:', this.numberOfRatings);
            this.numberOfRatings = numberOfRatings;
          });

        this.carService.getAverageRatings(this.car.carOwnerId).subscribe(
          (averageRatings) => {
            console.log('Average Ratings:', this.averageRatings);
            this.averageRatings = averageRatings;
          },
          (error) => {
            console.error('Error fetching car data:', error);
          }
        );
      }
    });
  }
  formatNumber(value: number | undefined): string {
    return value ? value.toLocaleString('en-US') : '0';
  }

  getCarPrice(): string {
    return this.car
      ? `${this.car.basePrice.toLocaleString('en-US')}k/day`
      : 'N/A';
  }

  getCarLocation(): string {
    if (!this.car || !this.car.address) {
      return 'N/A';
    }

    // Tách chuỗi địa chỉ thành các phần
    const parts = this.car.address.split('|');

    if (parts.length === 3) {
      let [city, district, ward] = parts;

      // Loại bỏ các từ "Xã", "Phường", "Quận", "Huyện", "Thành phố"
      ward = ward.replace(/Xã\s*/i, '').replace(/Phường\s*/i, '');
      district = district.replace(/Huyện\s*/i, '').replace(/Quận\s*/i, '');
      city = city.replace(/Thành phố\s*/i, '');

      // Trả về địa chỉ đã định dạng
      return `${ward}, ${district}, ${city}`;
    } else {
      // Nếu cấu trúc khác hoặc không đúng như mong đợi, trả về như ban đầu
      return this.car.address;
    }
  }
  avg: number = 0;
  getAvgCar(carOwnerId: string): void {
    this.http
      .get<avgOfCar>(`/api/Feedback/GetAverageRatings/${carOwnerId}`, {})
      .subscribe((respone) => {
        this.avg = respone.averageRatings;
      });
  }
  getCarRatings(): string {
    const fullStars = Math.floor(this.averageRatings || 0);
    const halfStar =
      this.averageRatings && this.averageRatings % 1 >= 0.5 ? 1 : 0;
    const emptyStars = 5 - fullStars - halfStar;

    const fullStarIcons = '<i class="fa-solid fa-star"></i>'.repeat(fullStars);
    const halfStarIcon = halfStar
      ? '<i class="fa-regular fa-star-sharp-half-stroke"></i>'
      : '';
    const emptyStarIcons = '<i class="fa-thin fa-star-sharp"></i>'.repeat(
      emptyStars
    );

    return `${fullStarIcons}${halfStarIcon}${emptyStarIcons}`;
  }

  getCarFeedbackMessage(): string {
    return this.numberOfRatings
      ? `${this.numberOfRatings} ratings`
      : 'No ratings yet';
  }
  gotoHomePage() {
    this.router.navigate(['/home']);
  }
  gotoSearch() {
    this.router.navigate(['/search']);
  }
  gotoBookCar() {
    this.router.navigate(['book-car/', this.carId], {
      queryParams: {
        location: this.location,
        pickupDate: this.pickupDate,
        pickupTime: this.pickupTime,
        dropoffDate: this.dropoffDate,
        dropoffTime: this.dropoffTime,
      },
    });
  }
}
