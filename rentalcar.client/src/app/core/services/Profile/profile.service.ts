import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  constructor(private http: HttpClient) {}

  // Method to fetch the user's profile based on type (Customer or CarOwner)
  getUserProfile(
    userType: 'Customer' | 'CarOwner',
    userId: string
  ): Observable<any> {
    const apiUrl = `/api/${userType}/${userId}`;
    return this.http.get(apiUrl);
  }

  // Method to save the user's profile
  saveUserProfile(
    userType: 'Customer' | 'CarOwner',
    userId: string,
    profileData: any
  ): Observable<any> {
    const apiUrl = `/api/${userType}/${userId}`;
    return this.http.put(apiUrl, profileData);
  }
}
