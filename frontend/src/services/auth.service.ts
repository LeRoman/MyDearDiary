import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, InjectionToken, PLATFORM_ID } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserRegisterDto } from '../models/Auth/user-register-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  refreshTokens() {
    throw new Error('Method not implemented.');
  }
  constructor(
    @Inject(PLATFORM_ID) private platformId: object,
    private httpClient: HttpClient
  ) {}

  private readonly TOKEN_KEY = 'auth_token';
  private baseUrl: string = environment.apiUrl;
  public loginRoutePrefix = '/api/auth/login';
  public registerRoutePrefix = '/api/auth/registration';

  register(newUser: UserRegisterDto): Observable<any> {
    return this.httpClient.post(`${this.baseUrl + this.registerRoutePrefix}`, {
      token: newUser.token,
      email: newUser.email,
      name: newUser.nickName,
      password: newUser.password,
    });
  }

  login(email: string, password: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl + this.loginRoutePrefix}`, {
      email: email,
      password: password,
    });
  }

  logout() {
    this.removeToken();
  }

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
