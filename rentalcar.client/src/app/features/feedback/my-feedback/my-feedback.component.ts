import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

export interface AvgStarRes {
  averageRatings: number;
}

export interface NumberOfStarsRes {
  starAll: number;
  star5: number;
  star4: number;
  star3: number;
  star2: number;
  star1: number;
}

export interface FeedbackRes {
  feedbacks: FeedbackModel[];
  totalPages: number;
}

export interface FeedbackModel {
  bookingId: string;
  content: string;
  dateTime: Date;
  id: string;
  ratings: number;
  carName: string;
}

export interface CarNameRes{
  carName: string;
}

@Component({
  selector: 'app-my-feedback',
  templateUrl: './my-feedback.component.html',
  styleUrls: ['./my-feedback.component.scss'],
})
export class MyFeedbackComponent implements OnInit {
  avgStar: number = 0;
  starAll: number = 0;
  star5: number = 0;
  star4: number = 0;
  star3: number = 0;
  star2: number = 0;
  star1: number = 0;
  stateOfStar: number = 0; // 0 is all stars, 1-5 is 1-5 star feedbacks
  itemsPerPage: number = 3; // Number of items per page
  currentPage: number = 1; // Track the current page
  totalPages: number = 1; // Total number of pages
  feedbacks: FeedbackModel[] = []; // List of feedbacks
  carOwnerId: string = '';

  constructor(private router: Router, private http: HttpClient) {}

  ngOnInit(): void {
    this.carOwnerId = localStorage.getItem('userId') || '';
    this.getAverageRatings(this.carOwnerId);
    this.getNumbersOfRating(this.carOwnerId);
    this.getListFeedbacks(this.carOwnerId, this.stateOfStar, this.currentPage, this.itemsPerPage);
  }

  getAverageRatings(carOwnerId: string): void {
    const url = `/api/Feedback/GetAverageRatings/${carOwnerId}`;
    this.http.get<AvgStarRes>(url).subscribe(
      (response) => {
        this.avgStar = response.averageRatings;
      },
      (error) => {
        console.error('Get avg rating failed', error);
        alert('Get avg rating failed: ' + error.error.message || error.error);
      }
    );
  }

  getNumbersOfRating(carOwnerId: string): void {
    const url = `/api/Feedback/GetNumbersOfRating/${carOwnerId}`;
    this.http.get<NumberOfStarsRes>(url).subscribe(
      (response) => {
        this.starAll = response.starAll;
        this.star5 = response.star5;
        this.star4 = response.star4;
        this.star3 = response.star3;
        this.star2 = response.star2;
        this.star1 = response.star1;
      },
      (error) => {
        console.error('Get numbers of rating failed', error);
        alert('Get numbers of rating failed: ' + error.error.message || error.error);
      }
    );
  }

  getListFeedbacks(carOwnerId: string, star: number, index: number, feedbackPerPage: number): void {
    const url = `/api/Feedback/GetListFeedbacks/${carOwnerId}?star=${star}&index=${index}&feedbackPerPage=${feedbackPerPage}`;
    this.http.get<FeedbackRes>(url).subscribe(
      (response) => {
        this.feedbacks = response.feedbacks;
        this.totalPages = response.totalPages;
        console.log(response);
        console.log("Feedbacks: ", response.feedbacks);
        console.log("Total: ", response.totalPages);
      },
      (error) => {
        console.error('Get feedbacks failed', error);
        alert('Get feedbacks failed: ' + error.error.message || error.error);
      }
    );
  }


  onStarClick(star: number): void {
    this.stateOfStar = star;
    this.currentPage = 1; // Reset to the first page when changing star filter
    this.getListFeedbacks(this.carOwnerId, this.stateOfStar, this.currentPage, this.itemsPerPage);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.getListFeedbacks(this.carOwnerId, this.stateOfStar, this.currentPage, this.itemsPerPage);
  }

  createArray(length: number): any[] {
    return new Array(length); // Clamp the length between 0 and 5
  }  
}
