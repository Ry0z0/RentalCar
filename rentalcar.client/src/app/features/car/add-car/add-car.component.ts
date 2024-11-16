import { HttpClient, HttpHeaders } from '@angular/common/http';

import {
  AfterViewInit,
  Component,
  OnInit,
  ElementRef,
  ViewChildren,
  ViewChild,
  QueryList,
  Renderer2,
  ChangeDetectorRef,
} from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

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

export interface addCarResponse {
  // name: string;
  // licensePlate: string;
  // brand: string;
  // model: string;
  // color: string;
  // numberOfSeats: number;
  // productionYear: number;
  // transmissionType: string;
  // fuelType: string;
  // mileage: number;
  // fuelConsumption: number;
  // basePrice: number;
  // deposit: number;
  // address: string;
  // description: string;
  // additionalFunctions: string;
  // carOwnerId: string;
  message: string;
}

@Component({
  selector: 'app-add-car',
  templateUrl: './add-car.component.html',
  styleUrl: './add-car.component.scss',
})
export class AddCarComponent implements OnInit, AfterViewInit {
  @ViewChild('fuelValueInput') fuelValueInput!: ElementRef;
  @ViewChildren('fuelOption') fuelOptions!: QueryList<ElementRef>;

  @ViewChild('transValueInput') transValueInput!: ElementRef;
  @ViewChildren('transOption') transOptions!: QueryList<ElementRef>;

  @ViewChild('process1') process1!: ElementRef;
  @ViewChild('process2') process2!: ElementRef;
  @ViewChild('process3') process3!: ElementRef;
  @ViewChild('process4') process4!: ElementRef;

  @ViewChild('AddACar1') AddACar1!: ElementRef;
  @ViewChild('AddACar2') AddACar2!: ElementRef;
  @ViewChild('AddACar3') AddACar3!: ElementRef;
  @ViewChild('AddACar4') AddACar4!: ElementRef;

  currentStepIndex = 0;
  steps: ElementRef[] = [];
  contents: ElementRef[] = [];

  // @ViewChild('fuelValueInput') fuelValueInput!: ElementRef;
  // fuelOptions: ElementRef[] = [];

  isOtherChecked = false;

  carData: any = {
    licensePlate: '',
    color: '',
    brandName: '',
    model: '',
    productionYear: '',
    noOfSeat: '',
    tranmission: '',
    fuel: '',
    mileage: '',
    fuelConsumption: '',
    city: '',
    district: '',
    ward: '',
    houseNumber: '',
    description: '',
    additionalFunction: '',
    images: '',
    basePrice: '',
    requiredDeposit: '',
    termOfUse: '',
    nameOfCar: '',
  };
  textarea: string = '';
  cities: City[] = [];
  districts: District[] = [];
  wards: Ward[] = [];
  additionalFunctions: string[] = [];
  termsOfUse: string[] = [];
  cityID: string = '';
  districtID: string = '';

  isFileInputDisabled: boolean = true;

  constructor(
    private renderer: Renderer2,
    private cd: ChangeDetectorRef,
    private http: HttpClient,
    private snackBar: MatSnackBar,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getCity();
    this.carData.tranmission = 'automatic';
    this.carData.fuel = 'gasoline';
  }
  loading: boolean = true;
  ngAfterViewInit() {
    this.setupOptionListeners(this.fuelOptions, this.fuelValueInput);
    this.setupOptionListeners(this.transOptions, this.transValueInput);

    this.steps = [this.process1, this.process2, this.process3, this.process4];
    this.contents = [
      this.AddACar1,
      this.AddACar2,
      this.AddACar3,
      this.AddACar4,
    ];
  }
  setupOptionListeners(options: QueryList<ElementRef>, valueInput: ElementRef) {
    options.forEach((label: ElementRef) => {
      this.renderer.listen(label.nativeElement, 'click', () => {
        options.forEach((option: ElementRef) => {
          this.renderer.removeClass(option.nativeElement, 'active');
          const radio = option.nativeElement.querySelector(
            'input[type="radio"]'
          );
          if (radio) {
            this.renderer.setProperty(radio, 'checked', false); // Bỏ chọn tất cả các radio button
          }
        });

        this.renderer.addClass(label.nativeElement, 'active');
        const selectedRadio = label.nativeElement.querySelector(
          'input[type="radio"]'
        );
        if (selectedRadio) {
          this.renderer.setProperty(selectedRadio, 'checked', true); // Chọn radio button tương ứng
          this.renderer.setProperty(
            valueInput.nativeElement,
            'value',
            selectedRadio.value
          );
        }
      });
    });
  }

  onNext() {
    if (this.currentStepIndex < this.steps.length - 1) {
      this.currentStepIndex++;
      this.goToStep(this.currentStepIndex);
      this.cd.detectChanges(); // Buộc Angular kiểm tra và cập nhật lại view
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
  other: string = ''; // Giá trị của textarea nếu "Other" được chọn
  toggleTextarea() {
    this.isOtherChecked = !this.isOtherChecked;
    // if (!this.isOtherChecked) {
    //   this.other = ''; // Xóa nội dung nếu "Other" bị bỏ chọn
    // }
  }

  selectBrand(brandName: string): void {
    this.carData.brandName = brandName;
  }
  selectColor(color: string): void {
    this.carData.color = color;
  }
  selectModel(model: string): void {
    this.carData.model = model;
  }
  selectProductionYear(productionYear: string): void {
    this.carData.productionYear = Number(productionYear);
  }
  selectNOS(nos: string): void {
    this.carData.noOfSeat = Number(nos);
  }
  selectCity(city: City): void {
    this.carData.city = city.name;
    this.carData.district = '';
    this.carData.ward = '';
    this.districts = [];
    this.wards = [];
    this.getDistrict(city.id);
  }
  selectDistrict(district: District): void {
    this.carData.district = district.name;
    this.carData.ward = '';
    this.wards = [];
    this.getWard(district.id);
  }
  selectWard(ward: Ward): void {
    this.carData.ward = ward.name;
  }

  get carTitle(): string {
    let title: string = `${this.carData.brandName} ${this.carData.model} ${this.carData.productionYear}`;
    if (title === '  ') {
      title = 'NAME OF CAR';
    }
    return title;
  }
  get FormattedBasePrice(): string {
    let bPrice = `${this.carData.basePrice}`;

    // Kiểm tra xem bPrice có phải là một chuỗi hợp lệ không (chỉ chứa số)
    if (/^\d+$/.test(bPrice)) {
      return bPrice.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    } else {
      return 'No base price yet'; // Trả về giá trị mặc định nếu bPrice không hợp lệ
    }
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

  get getAddress(): string {
    let address: string = `${this.carData.city}|${this.carData.district}|${this.carData.ward}|${this.carData.houseNumber}`;
    return address;
  }
  gotoHomePage() {
    this.router.navigate(['/home']);
  }

  getCity(): void {
    this.http.get<CityResponse>('/api/Address/GetAddressCitys', {}).subscribe(
      (response) => {
        this.cities = response.addressCity;
        console.log(response);
        this.loading = false;
      },
      (error) => {
        // Handle login error response
        //console.error('Login failed', error);
        if (error.error.message !== undefined) {
          alert('get city failed ' + error.error.message);
        } else {
          alert('get city failed ' + error.error);
        }
        this.loading = false;
      }
    );
  }

  getDistrict(cityID: string): void {
    this.http
      .get<DistrictResponse>(
        `/api/Address/GetAllAddressDistrictOfCity?id=${cityID}`,
        {}
      )
      .subscribe(
        (response) => {
          this.districts = response.addressDistrict;
          console.log(response);
        },
        (error) => {
          // Handle login error response
          //console.error('Login failed', error);
          if (error.error.message !== undefined) {
            alert('Login failed: ' + error.error.message);
          } else {
            alert('Login failed: ' + error.error);
          }
        }
      );
  }
  getWard(districtID: string): void {
    this.http
      .get<WardResponse>(
        `/api/Address/GetAllAddressWardOfDistrict?id=${districtID}`,
        {}
      )
      .subscribe(
        (response) => {
          this.wards = response.addressWard;
          console.log(response);
        },
        (error) => {
          // Handle login error response
          //console.error('Login failed', error);
          if (error.error.message !== undefined) {
            alert('Login failed: ' + error.error.message);
          } else {
            alert('Login failed: ' + error.error);
          }
        }
      );
  }

  onCheckboxChangeAD(option: string, event: any): void {
    if (event.target.checked) {
      // Thêm vào mảng nếu được chọn
      this.additionalFunctions.push(option);
    } else {
      // Loại bỏ nếu không được chọn
      const index = this.additionalFunctions.indexOf(option);
      if (index > -1) {
        this.additionalFunctions.splice(index, 1);
      }
    }
    this.carData.additionalFunction = this.additionalFunctions.join('|');
  }

  onCheckboxChangeTEU(option: string, event: any): void {
    if (event.target.checked) {
      // Thêm vào mảng nếu được chọn
      this.termsOfUse.push(option);
    } else {
      // Loại bỏ nếu không được chọn
      const index = this.termsOfUse.indexOf(option);
      if (index > -1) {
        this.termsOfUse.splice(index, 1);
      }
    }
    this.carData.termOfUse = this.termsOfUse.join('|');
  }

  get getTermOfUse(): string {
    let ta = '';
    if (this.carData.termOfUse.includes('other')) {
      ta = 'textarea: ' + this.textarea;
    }

    return this.carData.termOfUse + '|' + ta;
  }

  addCar(): void {
    const userId = localStorage.getItem('userId');
    const carData = {
      name: this.carTitle,
      licensePlate: this.carData.licensePlate,
      brand: this.carData.brandName,
      model: this.carData.model,
      color: this.carData.color,
      numberOfSeats: this.carData.noOfSeat,
      productionYears: this.carData.productionYear,
      transmissionType: this.carData.tranmission,
      fuelType: this.carData.fuel,
      mileage: this.carData.mileage,
      fuelConsumption: this.carData.fuelConsumption,
      basePrice: this.carData.basePrice,
      deposit: this.carData.requiredDeposit,
      address: this.getAddress,
      description: this.carData.description,
      additionalFunctions: this.carData.additionalFunction,
      termsOfUse: this.getTermOfUse,
      images: '1.jpg',
      carOwnerId: userId,
    };

    console.log('Car Data Object:', carData);

    this.http.post<any>('/api/Car', carData, {}).subscribe(
      (response: any) => {
        this.router.navigate(['/home']);
        console.log('AddCar successful', response);
        this.snackBar.open('Car added successfully!', 'Close', {
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
        console.log('Car Data Sent:', carData);
      }
    );
  }
}
