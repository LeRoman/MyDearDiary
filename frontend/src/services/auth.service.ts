import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, InjectionToken, PLATFORM_ID } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(@Inject(PLATFORM_ID) private platformId: object) {}
  private readonly TOKEN_KEY = 'auth_token';

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId))
      return localStorage.getItem(this.TOKEN_KEY);
    else return null;
  }

  removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }
}
