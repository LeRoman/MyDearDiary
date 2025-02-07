import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(): boolean {
    const token = this.authService.getToken();
    if (token) {
      if (this.authService.isMarkedForDeletion()) {
        this.router.navigate(['/restore']);
        return false;
      }
      if (this.authService.isAdmin()) {
        this.router.navigate(['/admin']);
        return false;
      }
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
