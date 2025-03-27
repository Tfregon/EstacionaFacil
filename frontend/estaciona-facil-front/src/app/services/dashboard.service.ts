import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'https://localhost:7105/api';

  constructor(private http: HttpClient, private auth: AuthService) {}

  getActiveVehicles() {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.auth.getToken()}`
    });

    return this.http.get(`${this.apiUrl}/Vehicles/active`, { headers });
  }
}
