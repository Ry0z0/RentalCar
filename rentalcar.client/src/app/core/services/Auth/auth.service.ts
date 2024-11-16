import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError, from } from 'rxjs';
import { catchError, tap, map, switchMap } from 'rxjs/operators';

export interface TokenResponse {
  email: string;
  refreshToken: string;
  token: string;
}

export interface DecryptedLoginResponse {
  token: TokenResponse;
  user: UserDetail;
  role: string;
  userId: string;
}

export interface UserDetail {
  address: string;
  dateOfBirth: Date;
  drivingLicense: string;
  email: string;
  id: string;
  name: string;
  nationalIdNo: string;
  passwordHash: string;
  phoneNo: string;
  wallet: string;
}
export interface LoginResponse {
  data: string;
}

export interface SignupResponse {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private tokenSubject: BehaviorSubject<string>;
  private refreshTokenSubject: BehaviorSubject<string>;

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token') || '';
    this.tokenSubject = new BehaviorSubject<string>(token);
    const refreshToken = localStorage.getItem('refreshToken') || '';
    this.refreshTokenSubject = new BehaviorSubject<string>(refreshToken);
  }

  async login(email: string, password: string): Promise<Observable<DecryptedLoginResponse>> {
    const hashedPassword = await this.hashPassword(password); // Await the password hash
    const data = { email, password: hashedPassword };
    console.log("Data: ", data);

    return this.http.post<DecryptedLoginResponse>('/api/Auth/login', data)
      .pipe(
        tap(response => {
          this.setLocal(response);
        }),
        catchError(error => {
          return throwError(error);
        })
      );
  }

  async signUp(role: string, name: string, phone: string, email: string, password: string): Promise<Observable<SignupResponse>> {
    const hashedPassword = await this.hashPassword(password); // Await the password hash
    const signUpUrl = role === 'rent' ? '/api/Auth/signup/customer' : '/api/Auth/signup/carowner';
    const data = {
      id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      name: name,
      dateOfBirth: null,
      nationalIdNo: null,
      phoneNo: phone,
      email: email,
      passwordHash: hashedPassword,
      address: null,
      drivingLicense: null,
      wallet: null
    };
    return this.http.post<SignupResponse>(signUpUrl, data);
  }


  private setLocal(authResult: DecryptedLoginResponse): void {
    localStorage.setItem('token', authResult.token.token);
    localStorage.setItem('refreshToken', authResult.token.refreshToken);
    //localStorage.setItem('user', JSON.stringify(authResult.user));
    localStorage.setItem('role', authResult.role);
    localStorage.setItem('userId', authResult.userId);
    localStorage.setItem('username', authResult.user.name);

    this.tokenSubject.next(authResult.token.token);
    this.refreshTokenSubject.next(authResult.token.refreshToken);
  }

    // Sets the new access token in localStorage and updates the BehaviorSubject
    setAccessToken(token: string): void {
      localStorage.setItem('token', token);
      this.tokenSubject.next(token);
    }
  
    // Retrieves the current access token
    getAccessToken(): string {
      return this.tokenSubject.value;
    }
  
    // Retrieves the current refresh token
    getRefreshToken(): string {
      return this.refreshTokenSubject.value;
    }
  
    // Refreshes the access token using the refresh token
    refreshToken(): Observable<DecryptedLoginResponse> {
      const data = {
        token: this.getAccessToken(),
        refreshToken: this.getRefreshToken()
      };
  
      return this.http.post<DecryptedLoginResponse>('/api/Auth/refresh', data)
        .pipe(
          tap(response => {
            this.setLocal(response);
          }),
          catchError(error => {
            this.logout();
            return throwError(error);
          })
        );
    }

  logout(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    const data = { refreshToken: refreshToken };
  
    return this.http.post('/api/Auth/logout', data)
      .pipe(
        tap(() => {
          // Xóa thông tin từ sessionStorage và cập nhật các BehaviorSubject
          localStorage.clear();
          this.tokenSubject.next('');
          this.refreshTokenSubject.next('');
        }),
        catchError(error => {
          return throwError(error);
        })
      );
  }

  // OTP Sending
  sendOtp(email: string): Observable<any> {
    return this.http.post('/api/Auth/forgetPassword', { email });
  }

  // OTP Verification
  verifyOtp(email: string, otp: string): Observable<{ token: string }> {
    return this.http.post<{ token: string }>('/api/Auth/verifyOtp', { email, otp })
      .pipe(
        tap(response => {
          console.log(response);
          // Store token and email after successful OTP verification
          localStorage.setItem('resetToken', response.token);
          localStorage.setItem('resetEmail', email);
        })
      );
  }

  // Password check
checkPassword(email: string, password: string): Observable<any> {
  return from(this.hashPassword(password)).pipe(
    switchMap(hashedPassword => {
      const data = { email, password: hashedPassword };
      return this.http.post('/api/Auth/checkPassword', data);
    }),
    catchError(error => {
      return throwError('Incorrect password.');
    })
  );
}

changePasswordProfile(email: string, newPassword: string): Observable<any> {
  const token = localStorage.getItem('token');

  if (!email || !token) {
    return throwError('Invalid session. Token or email not found.');
  }

  return from(this.hashPassword(newPassword)).pipe(
    switchMap(hashedPassword => {
      console.log(newPassword);
      console.log(hashedPassword);
      return this.http.post('/api/Auth/changePasswordProfile', { email, newPassword: hashedPassword, token });
    }),
    catchError(error => {
      return throwError('Failed to change password. Please try again.');
    })
  );
}
  // Password Change
  changePassword(newPassword: string): Observable<any> {
    const email = localStorage.getItem('resetEmail');
    const token = localStorage.getItem('resetToken');
    const hashedPassword = this.hashPassword(newPassword);
    if (!email || !token) {
      return throwError('Invalid session. Token or email not found.');
    }
    return from(hashedPassword).pipe(
      switchMap(hashedPassword => {
        console.log(newPassword);
        console.log(hashedPassword);
        return this.http.post('/api/Auth/changePassword', { email, newPassword: hashedPassword, token });
      }),
      catchError(error => {
        return throwError('Failed to change password. Please try again.');
      })
      );
    }

  async hashPassword(password: string): Promise<string> {
    const encoder = new TextEncoder();
    const data = encoder.encode(password);
    const hash = await crypto.subtle.digest('SHA-256', data);
    return this.arrayBufferToBase64(hash);
  }

  private arrayBufferToBase64(buffer: ArrayBuffer): string {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    for (let i = 0; i < bytes.byteLength; i++) {
      binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
  }

  // Method to get role from local storage, returning a default value if null
  getUserRole(): string {
    return localStorage.getItem('role') || ''; // Return empty string if null
  }

  // Method to check if the user is an admin
  isAdmin(): boolean {
    const role = this.getUserRole();
    return role === 'admin';
  }

  // Method to check if the user is a customer
  isCustomer(): boolean {
    const role = this.getUserRole();
    return role === 'customer';
  }

  // Method to check if the user is a car owner
  isCarOwner(): boolean {
    const role = this.getUserRole();
    return role === 'carOwner';
  }
}
