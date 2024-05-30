import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdresCompleteService {

  constructor(private http: HttpClient) { }

  //search(query: string): Observable<any> {
  //  const url = `https://nominatim.openstreetmap.org/search?format=json&q=${query}`;
  //  return this.http.get(url);
  //}
}
