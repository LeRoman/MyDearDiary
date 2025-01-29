import { Component } from '@angular/core';
import { UserLoginDto } from '../../../models/Auth/user-login-dto';
import { MaterialModules } from '../../../material.import';
import { FormsModule, NgModel } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { SnackBarService } from '../../../services/snack-bar.service';

@Component({
  selector: 'app-login',
  imports: [MaterialModules, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  public loginDto: UserLoginDto = new UserLoginDto();

  constructor(
    private snackBarService: SnackBarService,
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    this.authService
      .login(this.loginDto.email, this.loginDto.password)
      .subscribe({
        next: (response) => {
          console.log('Received token:', response.value);
          this.authService.setToken(response.value);
          this.router.navigate(['']);
        },
        error: (err) => {
          const errorMessage =
            err.error?.error || 'An unexpected error occurred.';
          this.snackBarService.showErrorMessage(errorMessage);
          console.error('Login failed:', err);
          this.clearForm();
        },
      });
  }

  clearForm(): void {
    this.loginDto.email = '';
    this.loginDto.password = '';
  }
}
