import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { LoginRegister } from '../interfaces/login-register';
import Swal from 'sweetalert2';
import { BehaviorSubject, Observable } from 'rxjs';
import { jwtDecode } from "jwt-decode";
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {
  private tokenKey = 'authToken';
  private refreshTokenKey = 'refresh_token';
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient, private router: Router) { }

  loginUser(model: LoginRegister) {
    return this.http.post<any>(`${environment.backendUrl}/authenticate/login`, model).pipe(
      tap(response =>
      {
        if (response)
        {
          sessionStorage.setItem(this.tokenKey, response.token);
          sessionStorage.setItem(this.refreshTokenKey, response.refreshToken);
          this.loggedIn.next(true);
          this.router.navigate(['']);
          Swal.fire('Poprawnie zalogowano');
        }
      })
    ).subscribe();
  }

  refreshToken() {
    const refreshToken = this.getRefreshToken();
    return this.http.post<any>(`${environment.backendUrl}/authenticate/refresh-token`, { token: this.getToken(), refreshToken }).pipe(
      tap((response: any) => {
        if (response) {
          sessionStorage.setItem(this.tokenKey, response.token);
          sessionStorage.setItem(this.refreshTokenKey, response.refreshToken);
        }
      }));
  }

  registerUser(model: LoginRegister) {
    return this.http.post<any>(`${environment.backendUrl}/authenticate/register`, model).subscribe(res =>
    {
      this.router.navigate(['/']);
      Swal.fire('Poprawnie zarejestrowano');
    });
  }

  forgotPassword(email: string) {
    return this.http.post<any>(`${environment.backendUrl}/authenticate/forgotPassword`, { email: email }).subscribe(res => {
      Swal.fire('Email został wysłany. Jeżeli go nie ma, sprawdź również spam.');
    });
  }

  resetPassword(model: PasswordReset) {
    return this.http.post<any>(`${environment.backendUrl}/authenticate/resetPassword`, model).subscribe(res => {
      this.router.navigate(['authenticate']);
      Swal.fire('Hasło zostało zmienione. Możesz się zalogować');
    });
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenKey);
    sessionStorage.removeItem(this.refreshTokenKey);
    this.loggedIn.next(false);
    this.router.navigate(['']);
    Swal.fire('Wylogowano');
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  getRefreshToken(): string | null {
    return sessionStorage.getItem(this.refreshTokenKey);
  }

  isLoggedIn(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  getUsername(): string | null {
    const decodedToken = this.getDecodedToken();
    return decodedToken ? decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] : null;
  }

  private getDecodedToken(): any {
    const token = this.getToken();   
    if (token) {
      return jwtDecode(token);
    }
    return null;
  }

  private hasToken(): boolean {
    return !!sessionStorage.getItem(this.tokenKey);
  }
}

export interface PasswordReset {
  email: string;
  token: string;
  newPassword: string;
}
