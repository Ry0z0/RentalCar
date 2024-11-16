import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/Auth/auth.service'; // Import AuthService

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const expectedRoles = route.data['roles'] as Array<string>; // Ensure expectedRoles is an array
    const userRole = this.authService.getUserRole(); // Get current user's role (always a string)

    // Ensure expectedRoles is defined and is an array
    if (Array.isArray(expectedRoles) && expectedRoles.includes(userRole)) {
      return true;
    } else {
      this.router.navigate(['/home']); // Redirect to home page if the role is not valid
      return false;
    }
  }
}
