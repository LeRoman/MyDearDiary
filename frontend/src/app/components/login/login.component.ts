import { Component } from '@angular/core';
import { RecordService } from '../../../services/record.service';
import { HttpClient } from '@angular/common/http';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { UserLoginDto } from '../../../models/Auth/user-login-dto';
import { MaterialModules } from '../../../material.import';
import { FormsModule, NgModel } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';

import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [MaterialModules, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  public loginDto: UserLoginDto = new UserLoginDto();

  constructor(
    private recordService: RecordService,
    private authService: AuthService,
    private http: HttpClient,
    private router: Router
  ) {}

  clearForm(): void {
    this.loginDto.email = '';
    this.loginDto.password = '';
  }
  login(): void {
    this.http
      .post<{ value: string }>(' https://localhost:7094/api/auth/login', {
        email: this.loginDto.email,
        password: this.loginDto.password,
      })
      .subscribe({
        next: (response) => {
          console.log('Received token:', response.value);
          this.authService.setToken(response.value);
          this.router.navigate(['']);
        },
        error: (err) => {
          console.error('Login failed:', err);
          this.clearForm();
        },
      });
  }
}
