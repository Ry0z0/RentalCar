import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AdminService, ICar } from '../../../core/services/Admin/admin.service';  // Ensure the correct path
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';  // Import NgbModal for handling modals

@Component({
  selector: 'app-admin-car',
  templateUrl: './admin-car.component.html',
  styleUrls: ['./admin-car.component.scss']
})
export class AdminCarComponent implements OnInit {
  cars: ICar[] = [];  // Array to hold the list of cars
  paginatedCars: ICar[] = [];  // Array to hold the cars displayed on the current page
  carForm!: FormGroup;  // FormGroup to handle the car edit form

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  carType: string = 'all';  // Track the current type of car ('all', 'active', 'inactive')
  
  sortColumn: keyof ICar | '' = '';  // Column being sorted
  sortDirection: 'asc' | 'desc' | '' = '';  // Sorting direction ('asc' or 'desc')

  constructor(private adminService: AdminService, private fb: FormBuilder, private modalService: NgbModal) { }

  ngOnInit(): void {
    this.loadAllCars();  // Load the list of cars when the component initializes
    this.initCarForm();  // Initialize the car form
  }

  loadAllCars(): void {
    this.adminService.getAllCars().subscribe(
      (data: any) => {
        this.cars = data.cars as ICar[];  // Convert the received data to an array of ICar
        this.totalPages = Math.ceil(this.cars.length / this.pageSize);
        this.updatePaginatedCars();
        this.carType = 'all';
      },
      error => {
        console.error('Error fetching car data', error);
      }
    );
  } 
  
  loadCarsByType(type: string): void {
    // Filter cars based on their active status
    this.cars = this.cars.filter(car => car.active === (type === 'active'));
    this.totalPages = Math.ceil(this.cars.length / this.pageSize);
    this.updatePaginatedCars();
    this.carType = type;
  }

  initCarForm(): void {
    this.carForm = this.fb.group({
      id: [''],
      name: ['', Validators.required],
      licensePlate: ['', Validators.required],
      brand: ['', Validators.required],
      model: ['', Validators.required],
      color: ['', Validators.required],
      numberOfSeats: ['', [Validators.required, Validators.min(1)]],
      productionYears: ['', [Validators.required, Validators.min(1886)]],
      transmissionType: ['', Validators.required],
      fuelType: ['', Validators.required],
      mileage: ['', [Validators.required, Validators.min(0)]],
      fuelConsumption: ['', [Validators.required, Validators.min(0)]],
      basePrice: ['', [Validators.required, Validators.min(0)]],
      deposit: ['', [Validators.required, Validators.min(0)]],
      address: ['', Validators.required],
      description: [''],
      additionalFunctions: [''],
      termsOfUse: [''],
      images: [''],
      active: [false],
      noOfRides: ['', [Validators.required, Validators.min(0)]],
      ratings: ['', [Validators.required, Validators.min(0), Validators.max(5)]],
      carOwnerId: ['', Validators.required]
    });
  }

  editCar(car: ICar, content: TemplateRef<any>): void {
    this.carForm.patchValue(car);  // Load the car data into the form
    this.modalService.open(content);  // Open the modal using the TemplateRef
  }

  updateCar(): void {
    if (this.carForm.valid) {
      const updatedCar = this.carForm.value;
      this.adminService.updateCar(updatedCar).subscribe(
        response => {
          this.loadAllCars();  // Reload cars after a successful update
          this.modalService.dismissAll();  // Close the modal
        },
        error => {
          console.error('Error updating car', error);
        }
      );
    }
  }

  deleteCar(id: string): void {
    if (confirm('Are you sure you want to delete this car?')) {
      this.adminService.deleteCar(id).subscribe(
        response => {
          this.loadAllCars();  // Reload the list of cars after a successful deletion
        },
        error => {
          console.error('Error deleting car', error);
        }
      );
    }
  }

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    const query = input.value;
    
    if (query) {
      this.adminService.searchCars(query).subscribe(
        (data: any) => {
          this.cars = data.cars as ICar[];
          this.totalPages = Math.ceil(this.cars.length / this.pageSize);
          this.updatePaginatedCars();
        },
        error => {
          console.error('Error searching cars', error);
        }
      );
    } else {
      this.loadAllCars();
    }
  }

  updatePaginatedCars(): void {
    let sortedCars = [...this.cars];
    if (this.sortColumn) {
        sortedCars.sort((a, b) => {
            const aValue = a[this.sortColumn as keyof ICar];
            const bValue = b[this.sortColumn as keyof ICar];

            if (aValue === undefined || bValue === undefined) return 0; // Add undefined check

            if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
            if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
            return 0;
        });
    }
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedCars = sortedCars.slice(start, end);
  }

  sort(column: keyof ICar): void {
    if (this.sortColumn === column) {
        this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
        this.sortColumn = column;
        this.sortDirection = 'asc';
    }
    this.updatePaginatedCars();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePaginatedCars();
  }
}
