import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, InjectionToken, PLATFORM_ID } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserRegisterDto } from '../models/Auth/user-register-dto';
import { JwtPayload, jwtDecode } from 'jwt-decode';
import { CustomJwtPayload } from '../models/Auth/jwt-payload-model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private baseUrl: string = environment.apiUrl;
  public loginRoutePrefix = '/api/auth/login';
  public registerRoutePrefix = '/api/auth/registration';
  public refreshToken = '/api/token/refresh';
  public deleteAcc = '/api/auth/delete';
  public restoreAcc: string = '/api/auth/restore';
  public sentInv: string = '/api/admin/sendinvite';

  constructor(
    @Inject(PLATFORM_ID) private platformId: object,
    private httpClient: HttpClient
  ) {}

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
    console.log('token removed');
    this.removeToken();
  }

  sentInvitation(userEmail: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl + this.sentInv}`, {
      email: userEmail,
    });
  }
  deleteAccount(password: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl + this.deleteAcc}`, {
      password: password,
    });
  }
  restoreAccount(password: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl + this.restoreAcc}`, {
      password: password,
    });
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

  refreshTokens(): Observable<any> {
    return this.httpClient.get(this.baseUrl + this.refreshToken);
  }

  isMarkedForDeletion(): boolean {
    const claims = this.getTokenClaims();
    return claims?.Status === 'MarkedForDeletion';
  }

  isAdmin(): boolean {
    const claims = this.getTokenClaims();
    return claims?.Role === 'Admin';
  }
  getTokenClaims(): CustomJwtPayload | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode<CustomJwtPayload>(token);
    } catch (error) {
      console.error('Invalid JWT token', error);
      return null;
    }
  }
}
