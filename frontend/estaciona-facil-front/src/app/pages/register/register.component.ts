import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  name = '';
  username = '';
  password = '';
  confirmPassword = '';
  errorMessage = '';

  constructor(private http: HttpClient, public router: Router) {}

  register() {
    
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'As senhas não coincidem.';
      return;
    }

    const newUser = {
      name: this.name,
      username: this.username,
      password: this.password
    };

    this.http.post('estacionafacilapi20250404165631.azurewebsites.net/api/Users/register', newUser).subscribe({
      next: () => {
        alert('Funcionário registrado com sucesso!');
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        console.error('Erro ao registrar:', err);
        this.errorMessage = 'Erro ao registrar funcionário.';
      }
    });
  }
  voltarAoDashboard() {
    this.router.navigate(['/dashboard']);
  }
}
