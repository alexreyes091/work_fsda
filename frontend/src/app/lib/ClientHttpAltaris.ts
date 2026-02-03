import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ClientHttpAltaris {

  private apiUrl: string = 'https://localhost:7011/api/v1';
  private http = inject(HttpClient);
  
  get<T>(endpoint: string, params?: HttpParams) {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`, { params });
  }

  post<T>(endpoint: string, body: any) {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, body);
  }

}
