import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = 'https://localhost:7105/api';

  constructor(private http: HttpClient, private auth: AuthService) {}

  // ✅ Headers com o token JWT
  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${this.auth.getToken()}`
    });
  }

  // 🚗 Buscar veículos ativos (sem saída)
  getActiveVehicles() {
    return this.http.get(`${this.apiUrl}/Vehicles/active`, {
      headers: this.getHeaders()
    });
  }

  // 🚪 Registrar saída de um veículo
  registrarSaida(vehicleId: string, additionalAmount: number | null = null) {
    const body = additionalAmount !== null ? { additionalAmount } : {};
    return this.http.put(`${this.apiUrl}/Vehicles/exit/${vehicleId}`, body, {
      headers: this.getHeaders()
    });
  }
  
   

  // 💰 Obter valor do caixa diário (caso precise exibir depois)
  getCashToday() {
    return this.http.get(`${this.apiUrl}/Cash/today`, {
      headers: this.getHeaders()
    });
  }
}
