import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/Auth/auth.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent {
  showModal = false;
  email = '';
  otp = '';
  loading = false; 

  constructor(private authService: AuthService, private router: Router) {}

  openModal() {
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  onSubmit() {
    this.loading = true;
    this.authService.sendOtp(this.email).subscribe(
      (response) => {
        this.loading = false;
        this.openModal();
      },
      (error) => {
        this.loading = false;
        console.error('Error sending OTP:', error);
      }
    );
  }

  confirmEmail() {
    this.loading = true;
    this.authService.verifyOtp(this.email, this.otp).subscribe(
      (response) => {
        this.loading = false;
        this.closeModal();
        this.router.navigate(['/reset']);  // Redirect to the password reset page
      },
      (error) => {
        this.loading = false;
        console.error('Error verifying OTP:', error);
      }
    );
  }
}
