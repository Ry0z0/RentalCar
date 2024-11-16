import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

export interface User {
  id: string;
  name: string;
  dateOfBirth: Date;
  nationalIdNo: string;
  phoneNo: string;
  email: string;
  passwordHash: string;
  address: string;
  drivingLicense: string;
  wallet: string;
}
export interface UserResponse {
  customer: User;
  carOwner: User;
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

@Component({
  selector: 'app-my-wallet',
  templateUrl: './my-wallet.component.html',
  styleUrl: './my-wallet.component.scss',
})
export class MyWalletComponent implements OnInit {
  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.getMyWallet();
  }
  showModalTopUp = false;
  showModalWithDraw = false;

  openModalTopUp() {
    this.showModalTopUp = true;
  }
  openModalWithDraw() {
    this.showModalWithDraw = true;
  }

  closeModalTopUp() {
    this.showModalTopUp = false;
  }
  closeModalWithDraw() {
    this.showModalWithDraw = false;
  }

  confirmTopUp() {
    // Handle logout logic here
    this.topUp();
    this.closeModalTopUp();
  }
  confirmWithDraw() {
    // Handle logout logic here
    this.withDraw();
    this.closeModalWithDraw();
  }

  gotoHomePage() {
    this.router.navigate(['/home']);
  }

  myWallet: string = '';
  balance: number = 0;
  getMyWallet(): void {
    const role = localStorage.getItem('role');
    const userId = localStorage.getItem('userId');
    const APIUrl = role === 'customer' ? '/api/Customer' : '/api/CarOwner';
    this.http.get<UserResponse>(`${APIUrl}/${userId}`, {}).subscribe(
      (response) => {
        console.log(response);
        console.log(response);
        if (role === 'customer') {
          this.user = response.customer;
          this.myWallet = response.customer.wallet;

          this.balance = Number(response.customer.wallet) ?? 0;
          this.myWallet = this.myWallet
            ? this.myWallet.replace(/\B(?=(\d{3})+(?!\d))/g, ',')
            : '0';
        } else {
          this.user = response.carOwner;
          this.myWallet = response.carOwner.wallet;
          this.balance = Number(response.carOwner.wallet);
          this.myWallet = this.myWallet
            ? this.myWallet.replace(/\B(?=(\d{3})+(?!\d))/g, ',')
            : '0';
        }
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
  user: User = {
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
  topUp(): void {
    const role = localStorage.getItem('role');
    const newWalletBalancee =
      Number(this.balance) + Number(this.selectedAmountTU);
    const userId = localStorage.getItem('userId');
    const APIUrl = role === 'customer' ? '/api/Customer' : '/api/CarOwner';
    console.log(typeof newWalletBalancee);

    // let updatedUser!: User;

    // if (role === 'customer') {
    //   updatedUser = {
    //     ...this.user, // Sao chép tất cả các trường hiện tại
    //     wallet: newWalletBalancee.toString(), // Cập nhật trường wallet
    //   };
    // } else {
    //   updatedUser = {
    //     ...this.user, // Sao chép tất cả các trường hiện tại
    //     wallet: newWalletBalancee.toString(), // Cập nhật trường wallet
    //   };
    // }
    // this.user.wallet = newWalletBalance + '';
    this.http
      .patch<User>(`${APIUrl}/${userId}/wallet`, {
        wallet: newWalletBalancee.toString(),
      })
      .subscribe(
        (response) => {
          this.getMyWallet();
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
  selectedAmountWD: number = 0;
  selectedAmountTU: number = 0;
  Amounts = [100000, 200000, 500000, 1000000, 2000000, 5000000, 10000000]; // Các tùy chọn số tiền rút

  get filteredAmounts() {
    const filtered = this.Amounts.filter((amount) => amount <= this.balance);

    return filtered;
  }
  formatCurrency(amount: number): string {
    return `${amount.toLocaleString()} VND`;
  }

  withDraw(): void {
    const role = localStorage.getItem('role');
    const newWalletBalance = this.balance - this.selectedAmountWD;
    const userId = localStorage.getItem('userId');
    const APIUrl = role === 'customer' ? '/api/Customer' : '/api/CarOwner';

    // let updatedUser!: User;

    // if (role === 'customer') {
    //   updatedUser = {
    //     ...this.user, // Sao chép tất cả các trường hiện tại
    //     wallet: newWalletBalance.toString(), // Cập nhật trường wallet
    //   };
    // } else {
    //   updatedUser = {
    //     ...this.user, // Sao chép tất cả các trường hiện tại
    //     wallet: newWalletBalance.toString(), // Cập nhật trường wallet
    //   };
    // }
    // this.user.wallet = newWalletBalance + '';
    this.http
      .patch<User>(`${APIUrl}/${userId}/wallet`, {
        wallet: newWalletBalance.toString(),
      })
      .subscribe(
        (response) => {
          // Cập nhật lại giá trị ví trong component sau khi server trả về phản hồi
          // this.balance = Number(response.wallet); // Chuyển đổi lại thành số
          // this.myWallet = response.wallet.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
          this.getMyWallet();
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
}
