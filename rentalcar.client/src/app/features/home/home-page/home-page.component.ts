import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
})
export class HomePageComponent implements OnInit {
  showModal = false;
  userRole: string | null = null;

  // Variables to store search inputs
  location: string = '';
  pickupDate: string = '';
  pickupTime: string = '';
  dropoffDate: string = '';
  dropoffTime: string = '';

  constructor(private router: Router) {}

  getUserRole(): string | null {
    return localStorage.getItem('role');
  }

  ngOnInit() {
    this.userRole = localStorage.getItem('role');
    console.log('User role:', this.userRole);
    this.userRole = this.getUserRole();
  }

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  confirmLogout() {
    this.closeModal();
  }

  gotoSearch() {
    // Pass search parameters to the search page
    this.router.navigate(['/search'], {
      queryParams: {
        location: this.location,
        pickupDate: this.pickupDate,
        pickupTime: this.pickupTime,
        dropoffDate: this.dropoffDate,
        dropoffTime: this.dropoffTime,
      },
    });
  }

  gotoListCar() {
    this.router.navigate(['/my-list-car']);
  }

  gotoBookCar() {
    // Add logic if necessary
  }
}
