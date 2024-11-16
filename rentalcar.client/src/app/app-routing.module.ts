
// import { HomePageComponent } from './features/home/home-page/home-page.component';
// import { AddCarComponent } from './features/car/add-car/add-car.component';
// import { ListSearchComponent } from './features/car/search-result/list-search/list-search.component';
// import { ViewCardetailComponent } from './features/car/search-result/view-cardetail/view-cardetail.component';
// import { BookCarComponent } from './features/booking/book-car/book-car.component';
// import { MybookingDetailComponent } from './features/booking/my-booking/mybooking-detail/mybooking-detail.component';
// import { ListMybookingComponent } from './features/booking/my-booking/list-mybooking/list-mybooking.component';
// import { MyWalletComponent } from './features/wallet/my-wallet/my-wallet.component';
// import { MyFeedbackComponent } from './features/feedback/my-feedback/my-feedback.component';
// import { LoginSignupComponent } from './features/auth/login-signup/login-signup.component';
// import { MyProfileComponent } from './features/profile/my-profile/my-profile.component';

// import { NgModule } from '@angular/core';
// import { RouterModule, Routes } from '@angular/router';

// export const routes: Routes = [
//   { path: '', redirectTo: '/home', pathMatch: 'full' },
//   { path: 'home', component: HomePageComponent },
//   { path: 'add-car', component: AddCarComponent },
//   { path: 'search', component: ListSearchComponent },
//   { path: 'car-detail/:id', component: ViewCardetailComponent },
//   { path: 'book-car/:id', component: BookCarComponent },
//   { path: 'my-bookings', component: ListMybookingComponent },
//   { path: 'my-booking-detail/:id', component: MybookingDetailComponent },
//   { path: 'my-wallet', component: MyWalletComponent },
//   { path: 'my-feedback', component: MyFeedbackComponent },
//   { path: 'login-signup', component: LoginSignupComponent },
//   { path: 'my-profile', component: MyProfileComponent },
// ];

// @NgModule({
//   imports: [RouterModule.forRoot(routes)],
//   exports: [RouterModule]
// })
// export class AppRoutingModule {}
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './features/home/home-page/home-page.component';
import { AddCarComponent } from './features/car/add-car/add-car.component';
import { ListSearchComponent } from './features/car/search-result/list-search/list-search.component';
import { ViewCardetailComponent } from './features/car/search-result/view-cardetail/view-cardetail.component';
import { BookCarComponent } from './features/booking/book-car/book-car.component';
import { ListMybookingComponent } from './features/booking/my-booking/list-mybooking/list-mybooking.component';
import { MybookingDetailComponent } from './features/booking/my-booking/mybooking-detail/mybooking-detail.component';
import { MyWalletComponent } from './features/wallet/my-wallet/my-wallet.component';
import { MyFeedbackComponent } from './features/feedback/my-feedback/my-feedback.component';
import { LoginSignupComponent } from './features/auth/login-signup/login-signup.component';
import { MyProfileComponent } from './features/profile/my-profile/my-profile.component';
import { AdminComponent } from './features/admin/admin/admin.component';
import { ConfirmEmailComponent } from './features/auth/forgot-password/confirm-email/confirm-email.component';
import { ResetPasswordComponent } from './features/auth/forgot-password/reset-password/reset-password.component';
import { ThumbviewComponent } from './features/car/my-car/thumbview/thumbview.component';
import { AuthGuard } from './core/guards/auth.guard';
import { EditCarComponent } from './features/car/my-car/edit-car/edit-car.component';
import { AdminCarComponent } from './features/admin/admin-car/admin-car.component';

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomePageComponent },
  { path: 'add-car', component: AddCarComponent, canActivate: [AuthGuard], data: { roles: ['customer', 'carOwner'] }},
  { path: 'search', component: ListSearchComponent, canActivate: [AuthGuard], data: { roles: ['customer'] }},
  { path: 'car-detail/:id', component: EditCarComponent, canActivate: [AuthGuard], data: { roles: ['carOwner'] }},
  { path: 'book-car/:id', component: BookCarComponent,canActivate: [AuthGuard], data: { roles: ['customer'] } },
  { path: 'my-bookings', component: ListMybookingComponent, canActivate: [AuthGuard], data: { roles: ['customer'] }},
  { path: 'my-booking-detail/:id', component: MybookingDetailComponent, canActivate: [AuthGuard], data: { roles: ['customer'] }},
  { path: 'my-wallet', component: MyWalletComponent, canActivate: [AuthGuard], data: { roles: ['customer', 'carOwner'] } },
  { path: 'my-feedback', component: MyFeedbackComponent, canActivate: [AuthGuard], data: { roles: ['carOwner'] }},
  { path: 'login-signup', component: LoginSignupComponent, },
  { path: 'my-profile', component: MyProfileComponent, canActivate: [AuthGuard], data: { roles: ['customer', 'carOwner', 'admin '] } },
  { path: 'admin', component: AdminComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
  { path: 'admin-car', component: AdminCarComponent, canActivate: [AuthGuard], data: { roles: ['admin'] } },
  { path: 'forget', component: ConfirmEmailComponent},
  { path: 'reset', component: ResetPasswordComponent},
  { path: 'my-list-car', component: ThumbviewComponent, canActivate: [AuthGuard], data: { roles: ['carOwner'] }},
  { path: 'view-car-detail/:id', component: ViewCardetailComponent, canActivate: [AuthGuard], data: { roles: ['customer'] }},
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
