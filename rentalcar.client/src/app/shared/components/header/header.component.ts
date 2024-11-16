import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/Auth/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  isLoggedIn: boolean = false;
  role: string | null = null;
  CustomerDetail = {
    name: '',
  };

  constructor(private router: Router, private authService: AuthService) {}

  ngOnInit(): void {
    this.isLoggedIn = this.checkLoginStatus();
    if (this.isLoggedIn) {
      this.getName();
      this.role = localStorage.getItem('role');
    }
  }

  getName() {
    const userName = localStorage.getItem('username');
    if (userName) {
      this.CustomerDetail.name = userName;
    }
  }

  checkLoginStatus(): boolean {
    return !!localStorage.getItem('token');
  }

  gotoAdmin(){
    this.router.navigate(['/admin']);
  }
  gotoAdminCar(){
    this.router.navigate(['/admin-car']);
  }
  gotoProfile() {
    this.router.navigate(['/my-profile']);
  }
  gotoMyWallet() {
    this.router.navigate(['/my-wallet']);
  }
  gotoMyCars() {
    this.router.navigate(['/my-list-car']);
  }
  gotoMyBookings() {
    this.router.navigate(['/my-bookings']);
  }
  gotoReports() {
    this.router.navigate(['/my-feedback']);
  }

  logOut() {
    this.router.navigate(['/login-signup'])  
  }
}
