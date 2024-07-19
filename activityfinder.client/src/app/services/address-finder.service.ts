import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AddressFinderService {

  constructor(private http: HttpClient) { }

  findAddress(searchInput: string): Observable<any> {
    return this.http.get<any>(`${environment.backendUrl}/address/` + searchInput);
  }
}
