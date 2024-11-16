import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CarRespone } from '../../../features/car/search-result/view-cardetail/view-cardetail.component';

@Injectable({
  providedIn: 'root',
})
export class CardetailService {
  constructor(private http: HttpClient) {}

  getCarById(carId: string | null): Observable<CarRespone> {
    return this.http.get<CarRespone>(`/api/Car/${carId}`);
  }

  getNumberOfRatings(carOwnerId: string): Observable<number> {
    return this.http.get<number>(
      `/api/Feedback/GetNumbersOfRating/${carOwnerId}`
    );
  }

  getAverageRatings(carOwnerId: string): Observable<number> {
    return this.http.get<number>(
      `/api/Feedback/GetAverageRatings/${carOwnerId}`
    );
  }
}
