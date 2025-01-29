import { Component, OnInit } from '@angular/core';
import { UserRegisterDto } from '../../../models/Auth/user-register-dto';
import { AuthService } from '../../../services/auth.service';
import { MaterialModules } from '../../../material.import';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackBarService } from '../../../services/snack-bar.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../enviroments/enviroment';
import { catchError, Observable, switchMap, tap, throwError } from 'rxjs';
import { response } from 'express';
import { error } from 'console';

@Component({
  selector: 'app-register',
  imports: [MaterialModules, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  public newUser: UserRegisterDto = new UserRegisterDto();
  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBarService: SnackBarService,
    private http: HttpClient
  ) {}

  baseUrl: string = environment.apiUrl;
  captchaImage: string = '';
  token: string = '';
  userInput: string = '';

  ngOnInit(): void {
    this.newUser.token = this.route.snapshot.queryParamMap.get('token');
    this.refreshCaptcha();
  }

  refreshCaptcha() {
    this.http.get<any>(this.baseUrl + '/api/captcha/generate').subscribe(
      (response) => {
        this.captchaImage = response.captchaImage;
        this.token = response.token;
      },
      (error) => console.log(error)
    );
  }

  verifyCaptcha(): Observable<any> {
    return this.http.post(
      this.baseUrl + '/api/captcha/verify',
      {
        token: this.token,
        userInput: this.userInput,
      },
      { observe: 'response' }
    );
  }

  register(): void {
    this.verifyCaptcha()
      .pipe(
        catchError((error: HttpErrorResponse) => {
          console.error('Помилка! Статус-код:', error.status);
          return throwError(error);
        })
      )
      .subscribe((data) => {
        this.authService.register(this.newUser).subscribe({
          next: (response) => {
            this.snackBarService.showUsualMessage(response.message);
            this.router.navigate(['']);
          },
          error: (err) => {
            console.error('Error:', err);
            const errorMessage =
              err.error?.message || 'An unexpected error occurred.';
            this.snackBarService.showErrorMessage(errorMessage);
          },
        });
      });
  }
}
