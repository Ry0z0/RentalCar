import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/Auth/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent {
  resetForm: FormGroup;
  loading = false;
  errorMessage: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.resetForm = this.fb.group({
      passnew: ['', [Validators.required, Validators.minLength(7)]],
      passconfirm: ['', [Validators.required, Validators.minLength(7)]]
    }, { validator: this.passwordMatchValidator });
  }

  passwordMatchValidator(form: FormGroup) {
    return form.get('passnew')!.value === form.get('passconfirm')!.value
      ? null : { mismatch: true };
  }

  onSubmit() {
    if (this.resetForm.invalid) {
      return;
    }

    this.loading = true;
    const newPassword = this.resetForm.get('passnew')!.value;

    this.authService.changePassword(newPassword).subscribe(
      (response) => {
        this.loading = false;
        console.log('Password reset successfully:', response);
        localStorage.removeItem('resetToken');  // Clear the reset token after successful reset
        localStorage.removeItem('resetEmail');  // Clear the reset email after successful reset
        this.router.navigate(['/login-signup']);  // Redirect to login after successful reset
      },
      (error) => {
        this.loading = false;
        console.error('Error resetting password:', error);
        this.errorMessage = 'An error occurred while resetting the password. Please try again.';
      }
    );
  }
}
