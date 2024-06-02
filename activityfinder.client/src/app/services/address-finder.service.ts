import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddressFinderService {

  constructor(private http: HttpClient) { }

  findAddress(searchInput: string): Observable<any> {
    return this.http.get<any>('/api/address/' + searchInput);
  }
}
