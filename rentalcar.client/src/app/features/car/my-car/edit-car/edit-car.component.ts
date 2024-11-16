import {
  Component,
  ElementRef,
  ViewChild,
  Renderer2,
  OnInit,
  AfterViewInit,
} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
export interface CarDetail {
  id: string;
  name: string;
  licensePlate: string;
  brand: string;
  model: string;
  color: string;
  numberOfSeats: number;
  productionYears: number;
  transmissionType: string;
  fuelType: string;
  mileage: number;
  fuelConsumption: number;
  basePrice: number;
  deposit: number;
  address: string;
  description?: string;
  additionalFunctions?: string;
  termsOfUse?: string;
  images?: string;
  active: boolean;
}
export interface CarResponse {
  car: CarDetail;
}
@Component({
  selector: 'app-edit-car',
  templateUrl: './edit-car.component.html',
  styleUrls: ['./edit-car.component.scss'],
})
export class EditCarComponent implements OnInit {
  @ViewChild('process1') process1!: ElementRef;
  @ViewChild('process2') process2!: ElementRef;
  @ViewChild('process3') process3!: ElementRef;

  @ViewChild('BasicInfo') BasicInfo!: ElementRef;
  @ViewChild('Details') Details!: ElementRef;
  @ViewChild('Princing') Princing!: ElementRef;

  car: CarDetail ={
    id: '',
    name: '',
    licensePlate: '',
    brand: '',
    model: '',
    color: '',
    numberOfSeats: 0,
    productionYears: 0,
    transmissionType: '',
    fuelType: '',
    mileage: 0,
    fuelConsumption: 0,
    basePrice: 0,
    deposit: 0,
    address: '',
    active: false,
  }; // CarDetail object
  carId: string | null = null; // Car ID to be used for API calls
  currentStepIndex = 0;
  steps: ElementRef[] = [];
  contents: ElementRef[] = [];
  loading = true;

  constructor(
    private renderer: Renderer2,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.carId = params.get('id');
      if (this.carId) {
        this.getCarDetail(this.carId);
      }
    });
  }


  getCarDetail(carId: string): void {
    this.loading = true;
    this.http.get<CarResponse>(`/api/Car/${carId}`)
      .subscribe(
        response => {
          this.car = response.car;
          console.log('Car details:', this.car);
          this.loading = false;
          this.initializeTabs();
        },
        error => {
          console.error('Error fetching car details:', error);
          alert('Failed to load car details. Please try again later.');
          this.loading = false;
        }
      );
  }

  updateCarDetail(): void {
    if (this.car && this.carId) {
      this.loading = true;
      this.http.put(`/api/Car/${this.carId}`, this.car)
        .subscribe(
          response => {
            alert('Car details updated successfully!');
            console.log('Car details updated:', response);
            this.router.navigate(['/my-list-car']);
          },
          error => {
            console.error('Error updating car details:', error);
            alert('Failed to update car details. Please try again later.');
            this.loading = false;
            this.initializeTabs();
          }
        );
    }
  }

  initializeTabs(): void {
    if (!this.loading) {  // Ensure tabs are initialized only after data is loaded
      this.steps = [this.process1, this.process2, this.process3];
      this.contents = [this.BasicInfo, this.Details, this.Princing];
      console.log('Steps:', this.steps);
      console.log('Contents:', this.contents);

      this.steps.forEach((stepElement, index) => {
        this.renderer.listen(stepElement.nativeElement, 'click', () => {
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
  }

  gotoHomePage() {
    this.router.navigate(['/home']);
  }

  gotoListCar() {
    this.router.navigate(['/my-list-car']);
  }
}
