import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface IUser {
  id: string;
  name: string;
  dateOfBirth?: Date;         // Optional field
  nationalIdNo?: string;      // Optional field
  phoneNo: string;
  email: string;
  passwordHash: string;
  address?: string;           // Optional field
  drivingLicense?: string;    // Optional field
  wallet?: string;            // Optional field
}
export interface ICar {
    id: string;
    name: string;
    licensePlate: string;
    brand: string;
    model: string;
    color: string;
    numberOfSeats: number;
    productionYears: number;
    transmissionType: string;
    fuelType: string;
    mileage: number;
    fuelConsumption: number;
    basePrice: number;
    deposit: number;
    address: string;
    description?: string;
    additionalFunctions?: string;
    termsOfUse?: string;
    images?: string;
    active: boolean;
    carOwnerId: string;
    noOfRides: number;
    ratings: number;
  }

  export interface UserDetail {
    id: string;
    name: string;
    dateOfBirth?: Date;        
    nationalIdNo?: string;      
    phoneNo: string;
    email: string;
    address?: string;          
    drivingLicense?: string;   
    wallet?: string;           
    role?: string;              
  }
  
  @Injectable({
    providedIn: 'root'
  })
  export class AdminService {
  
    constructor(private http: HttpClient) { }
  
    // User management methods
    getAllUsers(): Observable<any> {
      return this.http.get(`/api/Admin/Users`);
    }
  
    getUsersByType(type: string): Observable<any> {
      return this.http.get(`/api/Admin/Users/${type}`);
    }
    
    deleteUser(id: string): Observable<any> {
      return this.http.delete(`/api/Admin/Users/${id}`);
    }
    
    searchUsers(query: string): Observable<{ users: IUser[] }> {
      return this.http.get<{ users: IUser[] }>(`/api/Admin/SearchUsers`, { params: { query } });
    }
  
    updateUser(user: UserDetail): Observable<any> {
      return this.http.put(`/api/Admin/UpdateUser`, user);
    }
  
    // Car management methods
    getAllCars(): Observable<ICar[]> {
      return this.http.get<ICar[]>(`/api/Admin/getAllCarToAdmin`);
    }
  
    updateCar(car: ICar): Observable<any> {
      return this.http.put(`/api/Admin/UpdateCarAdmin`, car);
    }
  
    deleteCar(carId: string): Observable<any> {
      return this.http.delete(`/api/Car/${carId}`);
    }
  
    searchCars(query: string): Observable<{ cars: ICar[] }> {
      return this.http.get<{ cars: ICar[] }>(`/api/Admin/SearchCars`, { params: { query } });
    }
  }