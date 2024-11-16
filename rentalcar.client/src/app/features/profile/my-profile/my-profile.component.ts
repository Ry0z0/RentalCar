import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProfileService } from '../../../core/services/Profile/profile.service'; 
import { AuthService } from '../../../core/services/Auth/auth.service'; 
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { CityResponse, DistrictResponse, WardResponse } from '../../car/add-car/add-car.component';
export interface City {
  id: string;
  name: string;
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

export interface UserDetail {
  address: string;
  dateOfBirth: string;
  drivingLicense: string;
  email: string;
  id: string;
  name: string;
  nationalIdNo: string;
  passwordHash: string;
  phoneNo: string;
  wallet: string;
}
export interface ProfileRes {
  customer: UserDetail;
  carOwner: UserDetail;
}
@Component({
  selector: 'app-my-profile',
  templateUrl: './my-profile.component.html',
  styleUrls: ['./my-profile.component.scss'],
})
export class MyProfileComponent implements OnInit {
  profileForm!: FormGroup;
  passwordForm!: FormGroup; // FormGroup for password change
  activeTab: 'personal' | 'security' = 'personal'; // Default to 'personal'
  cities: City[] = [];
  districts: District[] = [];
  wards: Ward[] = [];
  loading: boolean = true;
  isLoading: boolean = false; 
  errorMessage: string | null = null;
  userType: 'Customer' | 'CarOwner' = 'Customer';
  userDetail: UserDetail = {
    address: '',
    dateOfBirth: '',
    drivingLicense: '',
    email: '',
    id: '',
    name: '',
    nationalIdNo: '',
    passwordHash: '',
    phoneNo: '',
    wallet: '',
  };

  constructor(
    private fb: FormBuilder,
    private profileService: ProfileService,
    private authService: AuthService,
    private http: HttpClient,
    private router: Router,
    private datePipe: DatePipe,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.setUserType();
    this.initForms();
    this.getCity();
    this.loadUserProfile();
  }

  // Initialize both forms
  initForms(): void {
    this.profileForm = this.fb.group({
      fullName: [''],
      dateOfBirth: [''],
      drivingLicense: [''],
      email: [''],
      id: [''],
      nationalIdNo: [''],
      phoneNo: [''],
      wallet: [''],
      houseNumber: [''],
      city: [''],
      district: [''],
      ward: [''],
    });

    this.passwordForm = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(7)]],
      confirmNewPassword: ['', Validators.required],
    }, {
      validators: this.passwordsMatchValidator
    });
  }

  // Method to switch between tabs
  switchTab(tab: 'personal' | 'security'): void {
    this.activeTab = tab;
  }

  // Password matching validator
  passwordsMatchValidator(form: FormGroup) {
    const newPassword = form.get('newPassword')?.value;
    const confirmNewPassword = form.get('confirmNewPassword')?.value;
    return newPassword === confirmNewPassword ? null : { mismatch: true };
  }

 
  // Method to change password
  onChangePassword(): void {
    if (this.passwordForm.invalid) {
      return;
    }

    const email = this.profileForm.get('email')?.value;
    const oldPassword = this.passwordForm.get('oldPassword')?.value;
    const newPassword = this.passwordForm.get('newPassword')?.value;

    this.isLoading = true;

    // Check the old password first
    this.authService.checkPassword(email, oldPassword).subscribe(
      (response) => {
        console.log('Password is correct:', response);

        // If old password is correct, proceed to change the password
        this.authService.changePasswordProfile(email, newPassword).subscribe(
          (response) => {
            this.isLoading = false;
            alert('Password changed successfully!');
            this.passwordForm.reset();
            this.switchTab('personal'); // Switch back to the personal tab
          },
          (error) => {
            this.isLoading = false;
            console.error('Error changing password:', error);
            this.errorMessage = 'An error occurred while changing the password. Please try again.';
          }
        );
      },
      (error) => {
        this.isLoading = false;
        console.error('Error checking password:', error);
        this.errorMessage = 'The old password is incorrect.';
      }
    );
  }

  // Discard password change
  onDiscardPasswordChange(): void {
    this.passwordForm.reset();
  }


  // Method to set userType based on role from localStorage
  setUserType(): void {
    const role = localStorage.getItem('role');
    if (role === 'customer') {
      this.userType = 'Customer';
    } else if (role === 'carOwner') {
      this.userType = 'CarOwner';
    }
  }
  // Method to load the user's profile based on the userType
  async loadUserProfile(): Promise<void> {
    this.isLoading = true;
    const userId = localStorage.getItem('userId');
    if (userId) {
      try {
        const data = await this.profileService.getUserProfile(this.userType, userId).toPromise();
        let userProfile: UserDetail;
  
        if (data.carOwner === undefined) {
          userProfile = data.customer;
        } else {
          userProfile = data.carOwner;
        }
  
        if (userProfile) {
          const formattedDate = this.datePipe.transform(userProfile.dateOfBirth, 'yyyy-MM-dd') ?? '';
  
          const addressParts = userProfile.address ? userProfile.address.split('|') : [];
          const cityName = addressParts[0]?.trim() || '';
          const districtName = addressParts[1]?.trim() || '';
          const wardName = addressParts[2]?.trim() || '';
          const houseNumber = addressParts[3]?.trim() || '';
  
          this.profileForm.patchValue({
            fullName: userProfile.name,
            dateOfBirth: formattedDate,
            drivingLicense: userProfile.drivingLicense,
            email: userProfile.email,
            id: userProfile.id,
            nationalIdNo: userProfile.nationalIdNo,
            passwordHash: userProfile.passwordHash,
            phoneNo: userProfile.phoneNo,
            wallet: userProfile.wallet,
            houseNumber: houseNumber
          });
  
          this.userDetail = {
            ...userProfile,
            dateOfBirth: formattedDate,
            address: `${cityName}|${districtName}|${wardName}|${houseNumber}`
          };
  
          this.cdr.detectChanges(); // Force change detection here
  
          const cityId = await this.getCityIdByName(cityName);
          console.log('City ID:', cityId);
          if (cityId) {
            this.profileForm.patchValue({ city: cityId });
  
            await this.getDistrict(cityId);
            this.cdr.detectChanges(); // Force change detection again
  
            const districtId = await this.getDistrictIdByName(districtName);
            console.log('District ID:', districtId);
            if (districtId) {
              this.profileForm.patchValue({ district: districtId });
  
              await this.getWard(districtId);
              this.cdr.detectChanges(); // Force change detection again
  
              const wardId = await this.getWardIdByName(wardName);
              if (wardId) {
                this.profileForm.patchValue({ ward: wardId });
              }
            }
          }
        } else {
          console.error('User profile data is not available for this user type.');
        }
        this.isLoading = false;
      } catch (error) {
        console.error('Error loading user profile', error);
        this.isLoading = false;
      }
    }
  }
  

// Các hàm phụ trợ để lấy ID từ tên city, district, và ward
getCityIdByName(name: string): Promise<string | null> {
  return new Promise((resolve) => {
    const city = this.cities.find(c => c.name === name);
    resolve(city ? city.id : null);
  });
}

getDistrictIdByName(name: string): Promise<string | null> {
  return new Promise((resolve) => {
    const district = this.districts.find(d => d.name === name);
    resolve(district ? district.id : null);
  });
}

getWardIdByName(name: string): Promise<string | null> {
  return new Promise((resolve) => {
    const ward = this.wards.find(w => w.name === name);
    resolve(ward ? ward.id : null);
  });
}


  
  
  

  // Method to save the user's profile
  onSave(): void {
    if (this.profileForm.valid) {
      this.isLoading = true; // Bắt đầu loading
      const userId = localStorage.getItem('userId');
      const updatedUserDetail = {
        ...this.userDetail,
        name: this.profileForm.get('fullName')?.value || '',
      phoneNo: this.profileForm.get('phoneNo')?.value || '',
      nationalIdNo: this.profileForm.get('nationalIdNo')?.value || '',
      dateOfBirth: this.profileForm.get('dateOfBirth')?.value || null,
      email: this.profileForm.get('email')?.value || '',
      drivingLicense: this.profileForm.get('drivingLicense')?.value || '',
      address: this.address,
      };
      if (userId) {
        this.profileService
          .saveUserProfile(this.userType, userId, updatedUserDetail)
          .subscribe(
            (response: any) => {
              console.log(response);
              alert('User details updated successfully!');
              this.isLoading = false; // Kết thúc loading
            },
            (error: any) => {
              console.error('Error updating user details', error);
              this.isLoading = false; // Kết thúc loading khi có lỗi
            }
          );
      }
    } else {
      console.warn('Form is not valid');
    }
  }

  get address(): string {
    const cityElement = document.querySelector(
      '#city option:checked'
    ) as HTMLOptionElement;
    const districtElement = document.querySelector(
      '#district option:checked'
    ) as HTMLOptionElement;
    const wardElement = document.querySelector(
      '#ward option:checked'
    ) as HTMLOptionElement;

    const cityName = cityElement ? cityElement.textContent : '';
    const districtName = districtElement ? districtElement.textContent : '';
    const wardName = wardElement ? wardElement.textContent : '';

    return `${cityName}|${districtName}|${wardName}|${
      this.profileForm.get('houseNumber')?.value
    }`;
  }

  selectCity(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const cityId = selectElement.value;
    const cityControl = this.profileForm.get('city');
    if (cityControl) {
      cityControl.setValue(cityId);
      this.getDistrict(cityId); // Tải lại danh sách District sau khi chọn City
    }
  }
  
  selectDistrict(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const districtId = selectElement.value;
    const districtControl = this.profileForm.get('district');
    if (districtControl) {
      districtControl.setValue(districtId);
      this.getWard(districtId); // Tải lại danh sách Ward sau khi chọn District
    }
  }
  
  selectWard(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const wardId = selectElement.value;
    const wardControl = this.profileForm.get('ward');
    if (wardControl) {
      wardControl.setValue(wardId);
    }
  }
  

  getCity(): void {
    this.http.get<CityResponse>('/api/Address/GetAddressCitys', {}).subscribe(
      (response) => {
        this.cities = response.addressCity;
        this.loading = false;
      },
      (error) => {
        alert('Failed to load cities: ' + error.message);
        this.loading = false;
      }
    );
}

getDistrict(cityID: string): Promise<void> {
    return new Promise((resolve, reject) => {
        this.http
            .get<DistrictResponse>(`/api/Address/GetAllAddressDistrictOfCity?id=${cityID}`, {})
            .subscribe(
                (response) => {
                    this.districts = response.addressDistrict;
                    console.log(response);
                    resolve(); // Resolve khi hoàn tất
                },
                (error) => {
                    alert('Failed to load districts: ' + error.message);
                    reject(error); // Reject nếu gặp lỗi
                }
            );
    });
}

getWard(districtID: string): Promise<void> {
    return new Promise((resolve, reject) => {
        this.http
            .get<WardResponse>(`/api/Address/GetAllAddressWardOfDistrict?id=${districtID}`, {})
            .subscribe(
                (response) => {
                    console.log(response);
                    this.wards = response.addressWard;
                    resolve(); // Resolve khi hoàn tất
                },
                (error) => {
                    alert('Failed to load wards: ' + error.message);
                    reject(error); // Reject nếu gặp lỗi
                }
            );
    });
}

  gotoHomePage() {
    this.router.navigate(['/home']);
  }
}
