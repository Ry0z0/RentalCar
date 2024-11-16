import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Car, SearchResponse } from '../../../features/car/search-result/list-search/list-search.component'; // Đảm bảo đường dẫn chính xác

@Injectable({
  providedIn: 'root',
})
export class CarService {
  private apiUrl = '/api/Car'; // Đặt URL của API

  constructor(private http: HttpClient) {}

  searchCars(startDate: string, endDate: string, location: string): Observable<SearchResponse> {
    let params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate)
      .set('location', location);

    return this.http.get<SearchResponse>(`${this.apiUrl}/Search`, { params });
  }
}
