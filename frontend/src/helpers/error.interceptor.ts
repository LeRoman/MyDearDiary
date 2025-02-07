import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { SnackBarService } from '../services/snack-bar.service';
import { catchError, switchMap, throwError } from 'rxjs';

export const ErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const snackBar = inject(SnackBarService);

  return next(req).pipe(
    catchError((response) => {
      if (response.status === 401) {
        return authService.refreshTokens().pipe(
          switchMap((resp) => {
            console.log(resp.value);
            authService.setToken(resp.value);
            return next(
              req.clone({
                setHeaders: { Authorization: `Bearer ${resp.value}` },
                body: req.body,
              })
            );
          })
        );
      }

      if (response.status === 401) {
        authService.logout();
        router.navigate(['/']);

        snackBar.showUsualMessage('Session expired');
      }

      console.log(response);
      const error = response.error
        ? response.error.error || response.error.message
        : response.message || `${response.status} ${response.statusText}`;

      return throwError(() => new Error(error));
    })
  );
};
