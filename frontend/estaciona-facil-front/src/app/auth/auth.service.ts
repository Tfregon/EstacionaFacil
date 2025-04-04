// src/app/auth/auth.service.ts

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'estacionafacilapi20250404165631.azurewebsites.net/api';

  constructor(private http: HttpClient, private router: Router) {}

  // 🔐 Login e salvamento do token
  login(credentials: { username: string; password: string }) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          console.log(response)
          localStorage.setItem('token', response.token); // salva o token no navegador
        })
      );
  }

  // ✅ Verifica se usuário está logado
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  // 🚪 Logout
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  // 📦 Recupera o token para enviar nas requisições autenticadas
  getToken(): string | null {
    return localStorage.getItem('token');
  }
}
