import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { LoginRegister } from '../interfaces/login-register';
import Swal from 'sweetalert2';
import { BehaviorSubject, Observable } from 'rxjs';
import { jwtDecode } from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {
  private tokenKey = 'authToken';
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient, private router: Router) { }

  loginUser(model: LoginRegister) {
    return this.http.post<any>('/api/authenticate/login', model).pipe(
      tap(response =>
      {
        if (response)
        {
          const token = JSON.stringify(response);
          sessionStorage.setItem(this.tokenKey, token);
          this.loggedIn.next(true);
          this.router.navigate(['']);
          Swal.fire('Poprawnie zalogowano');
        }
      })
    ).subscribe();
  }

  registerUser(model: LoginRegister) {
    return this.http.post<any>('/api/authenticate/register', model).subscribe(res =>
    {
      this.router.navigate(['/']);
      Swal.fire('Poprawnie zarejestrowano');
    });
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenKey);
    this.loggedIn.next(false);
    this.router.navigate(['']);
    Swal.fire('Wylogowano');
  }

  getToken(): string | null {
    const sessionToken = sessionStorage.getItem(this.tokenKey);
    if (!sessionToken) {
      return '';
    }
    const tokenObject = JSON.parse(sessionToken);
    return tokenObject.token;
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
