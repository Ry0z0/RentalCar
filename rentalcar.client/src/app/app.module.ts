import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';
import { CommonModule, DatePipe } from '@angular/common';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
// @NgModule({
//   imports: [
//     BrowserModule,
//     CommonModule,
//     HttpClientModule,
//     FormsModule,
//     RouterModule,
//     AppRoutingModule, // The routes are defined in the MainRoutingModule
//   ],
// })
// export class AppModule { }
// import { HttpClientModule } from '@angular/common/http';
// import { NgModule } from '@angular/core';
// import { BrowserModule } from '@angular/platform-browser';

// import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConfirmEmailComponent } from './features/auth/forgot-password/confirm-email/confirm-email.component';
import { ResetPasswordComponent } from './features/auth/forgot-password/reset-password/reset-password.component';
import { LoginSignupComponent } from './features/auth/login-signup/login-signup.component';
import { BookCarComponent } from './features/booking/book-car/book-car.component';
import { ListMybookingComponent } from './features/booking/my-booking/list-mybooking/list-mybooking.component';
import { MybookingDetailComponent } from './features/booking/my-booking/mybooking-detail/mybooking-detail.component';
import { EditCarComponent } from './features/car/my-car/edit-car/edit-car.component';
import { ThumbviewComponent } from './features/car/my-car/thumbview/thumbview.component';
import { ListSearchComponent } from './features/car/search-result/list-search/list-search.component';
import { ViewCardetailComponent } from './features/car/search-result/view-cardetail/view-cardetail.component';
import { MyFeedbackComponent } from './features/feedback/my-feedback/my-feedback.component';
import { NgClass, NgFor } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { HeaderComponent } from './shared/components/header/header.component';
import { HomePageComponent } from './features/home/home-page/home-page.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { appConfig } from './app.config';
import { AuthInterceptor } from './core/interceptors/auth.interceptors';
import { AddCarComponent } from './features/car/add-car/add-car.component';
import { AdminComponent } from './features/admin/admin/admin.component';
import { MyProfileComponent } from './features/profile/my-profile/my-profile.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MyWalletComponent } from './features/wallet/my-wallet/my-wallet.component';
import { AdminCarComponent } from './features/admin/admin-car/admin-car.component';
@NgModule({
  declarations: [
    AppComponent,
    AddCarComponent,
    ConfirmEmailComponent,
    ResetPasswordComponent,
    LoginSignupComponent,
    BookCarComponent,
    ListMybookingComponent,
    MybookingDetailComponent,
    HomePageComponent,
    EditCarComponent,
    ThumbviewComponent,
    ListSearchComponent,
    ViewCardetailComponent,
    MyFeedbackComponent,
    MybookingDetailComponent,
    ListMybookingComponent,
    HeaderComponent,
    FooterComponent,
    AdminComponent,
    MyProfileComponent,
    MyWalletComponent,
    AdminCarComponent,
  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    RouterModule, 
    NgClass,
    NgFor,
    RouterOutlet,
    RouterLink,
    FormsModule,
    ReactiveFormsModule
    //HttpClientModule,
  ],
  providers: [DatePipe, provideHttpClient(withInterceptorsFromDi()),  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }, provideAnimationsAsync()],
  bootstrap: [AppComponent],
})
export class AppModule {}