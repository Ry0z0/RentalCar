import {
  Component,
  ElementRef,
  ViewChild,
  Renderer2,
  OnInit,
  AfterViewInit,
  AfterViewChecked,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CarService } from '../../../../core/services/Car/car.service';
export interface Car {
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
  images: string;
  active: boolean;
  carOwnerId: string;
  noOfRides: number;
  ratings: number;
}

export interface SearchResponse {
  cars: Car[];
}

@Component({
  selector: 'app-list-search',
  templateUrl: './list-search.component.html',
  styleUrls: ['./list-search.component.scss'],
})
export class ListSearchComponent implements OnInit, AfterViewChecked {
  @ViewChild('process1', { static: false }) process1!: ElementRef;
  @ViewChild('process2', { static: false }) process2!: ElementRef;

  @ViewChild('listview', { static: false }) listview!: ElementRef;
  @ViewChild('thumbview', { static: false }) thumbview!: ElementRef;

  currentStepIndex = 0;
  steps: ElementRef[] = [];
  contents: ElementRef[] = [];
  cars: Car[] = [];
  paginatedCars: Car[] = [];

  isLoading: boolean = false;

  currentPage = 1;
  pageSize = 5;
  totalPages = 1;

  location: string = '';
  pickupDate: string = '';
  pickupTime: string = '';
  dropoffDate: string = '';
  dropoffTime: string = '';

  sortOption: string = 'Star: High to Low';

  constructor(
    private renderer: Renderer2,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private carService: CarService
  ) {}

  ngOnInit(): void {
    this.activeRoute.queryParams.subscribe((params) => {
      this.location = params['location'] || '';
      this.pickupDate = params['pickupDate'] || '';
      this.pickupTime = params['pickupTime'] || '';
      this.dropoffDate = params['dropoffDate'] || '';
      this.dropoffTime = params['dropoffTime'] || '';

      this.searchCars();
    });
  }

  ngAfterViewChecked(): void {
    if (this.process1 && this.process2 && this.listview && this.thumbview) {
      this.steps = [this.process1, this.process2];
      this.contents = [this.listview, this.thumbview];

      this.steps.forEach((stepElement, index) => {
        if (stepElement) {
          this.renderer.listen(stepElement.nativeElement, 'click', () => {
            if (!this.isLoading) {
              this.steps.forEach((step) =>
                this.renderer.removeClass(step.nativeElement, 'actived')
              );
              this.contents.forEach((content) =>
                this.renderer.removeClass(content.nativeElement, 'current')
              );

              this.renderer.addClass(stepElement.nativeElement, 'actived');
              this.renderer.addClass(
                this.contents[index].nativeElement,
                'current'
              );
            }
          });
        }
      });
    }
  }

  onSearch(): void {
    this.cars = [];
    this.paginatedCars = [];
    this.currentStepIndex = 0;
    this.searchCars();
  }

  searchCars(): void {
    this.isLoading = true;
    const startDate = `${this.pickupDate}T${this.pickupTime}`;
    const endDate = `${this.dropoffDate}T${this.dropoffTime}`;
    this.carService.searchCars(startDate, endDate, this.location).subscribe(
      (response) => {
        this.cars = response.cars;
        this.totalPages = Math.ceil(this.cars.length / this.pageSize);
        this.sortAndPaginateCars();
        this.isLoading = false;
      },
      (error) => {
        console.error('Error fetching cars', error);
        this.isLoading = false;
      }
    );
  }

  sortAndPaginateCars() {
    let sortedCars = [...this.cars];

    switch (this.sortOption) {
      case 'Star: High to Low':
        sortedCars.sort((a, b) => b.ratings - a.ratings);
        break;
      case 'Star: Low to High':
        sortedCars.sort((a, b) => a.ratings - b.ratings);
        break;
      case 'Price: Low to High':
        sortedCars.sort((a, b) => a.basePrice - b.basePrice);
        break;
      case 'Price: High to Low':
        sortedCars.sort((a, b) => b.basePrice - a.basePrice);
        break;
    }

    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedCars = sortedCars.slice(start, end);
  }

  onSortChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.sortOption = selectElement.value;
    this.sortAndPaginateCars();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.sortAndPaginateCars();
  }

  gotoBookCar(carId: string) {
    this.router.navigate(['book-car/', carId], {
      queryParams: {
        location: this.location,
        pickupDate: this.pickupDate,
        pickupTime: this.pickupTime,
        dropoffDate: this.dropoffDate,
        dropoffTime: this.dropoffTime,
      },
    });
  }
  gotoViewCarDetail(carId: string): void {
    this.router.navigate(['view-car-detail/', carId], {
      queryParams: {
        location: this.location,
        pickupDate: this.pickupDate,
        pickupTime: this.pickupTime,
        dropoffDate: this.dropoffDate,
        dropoffTime: this.dropoffTime,
      },
    });
  }

  createArray(length: number): any[] {
    return new Array(Math.floor(length));
  }
}
