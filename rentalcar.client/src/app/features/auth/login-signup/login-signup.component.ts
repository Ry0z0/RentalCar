import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AuthService } from '../../../core/services/Auth/auth.service';
import { filter } from 'rxjs';

@Component({
  selector: 'app-login-signup',
  templateUrl: './login-signup.component.html',
  styleUrls: ['./login-signup.component.scss'], 
})
export class LoginSignupComponent implements OnInit {
  email: string = '';
  password: string = '';
  name: string = '';
  phone: string = '';
  registerPassword: string = '';
  confirmPassword: string = '';
  role: string = 'rent';
  emailLogin: string = '';
  passwordLogin: string = '';

  constructor(private router: Router, private authService: AuthService) {
    // Listen for navigation events
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      if (event.url === '/login-signup') {  // Check URL to identify the login page
        this.logoutIfTokenExists();
      }
    });
  }

  ngOnInit(): void {}

  openTerms(): void {
    window.open('https://www.rentalcars.com/vi/terms-and-conditions', '_blank');
  }

  togglePasswordVisibility(fieldId: string): void {
    const field = document.getElementById(fieldId) as HTMLInputElement;
    field.type = field.type === 'password' ? 'text' : 'password';
  }

  navigateToForgotPassword(): void {
    this.router.navigate(['/forget']);
  }

  // Function to log out if the token exists in localStorage
  logoutIfTokenExists(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.logout().subscribe(() => {
        console.log('Logged out successfully');
        localStorage.clear();  // Clear localStorage after successful logout
      }, (error) => {
        console.error('Logout failed', error);
      });
    }
  }

  async login(): Promise<void> {
    try {
      const response = await (await this.authService.login(this.emailLogin, this.passwordLogin)).toPromise();
      console.log('Login successful', response);
      if (response && response.user && response.user.name) {
        alert('Login successful: ' + response.user.name);
      } else {
        alert('Login successful');
      }
      let role = localStorage.getItem('role');
      console.log('Role at login: ', role);
      if(role === 'admin') {
        this.router.navigate(['/admin']);
      } else {
      this.router.navigate(['/home']);
      }
    } catch (error) {
      console.error('Login failed', error);
      alert("Login failed: " + (error as any).error.message);
    }
  }

  async signUp(): Promise<void> {
    if (this.registerPassword !== this.confirmPassword) {
      alert('Passwords do not match');
      return;
    }

    try {
      const response = await (await this.authService.signUp(this.role, this.name, this.phone, this.email, this.registerPassword)).toPromise();
      console.log('Signup successful', response);
      alert('Signup successful: ' + (response ? response.message : ''));
      this.emailLogin = this.email;
      this.passwordLogin = this.registerPassword;
      await this.login();
    } catch (error) {
      console.error('Signup failed', error);
      alert("Sign up failed: " + (error as any).error.message);
    }
  }
}
