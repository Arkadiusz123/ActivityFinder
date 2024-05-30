import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, catchError } from 'rxjs';
import { Router } from '@angular/router';


@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An unknown error occurred!';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Client-side error: ${error.error.message}`;
        } else {
          // Server-side error
          errorMessage = `Server-side error: ${error.status}\nMessage: ${error.message}`;
          if (error.status === 401) {
            // Handle unauthorized errors
            this.router.navigate(['/login']);
          } else if (error.status === 404) {
            // Handle not found errors
            this.router.navigate(['/not-found']);
          }
        }

        // Log the error (can be sent to logging infrastructure)
        console.error(errorMessage);

        // Show an alert or notification to the user
        window.alert(errorMessage);

        return throwError(errorMessage);
      })
    );
  }
}
