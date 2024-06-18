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
import Swal from 'sweetalert2';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {

        let errorMessage = 'An unknown error occurred!';

        if (error.error instanceof ErrorEvent) {
          // Client-side error
          errorMessage = `Błąd aplikacji: ${error.error.message}`;
        } else {
          // Server-side error
          //console.error(error);
          errorMessage = error.error;
          if (error.status === 401) {
            // Handle unauthorized errors
            this.router.navigate(['/login']);
          }
          else if (error.status === 404) {
            // Handle not found errors
            this.router.navigate(['/not-found']);
          }
          else if (error.status === 500 || error.status === 504) {
            errorMessage = 'Wystąpił nieznany błąd. Spróbuj ponownie później.'
          }
        }

        // Show an alert or notification to the user

        Swal.fire(errorMessage);

        return throwError(errorMessage);
      })
    );
  }
}
