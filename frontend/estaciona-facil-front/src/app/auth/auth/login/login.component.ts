import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common'; // ✅ importa NgIf
@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [FormsModule,NgIf] // ✅ necessário para o ngModel funcionar
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login({ username: this.username, password: this.password }).subscribe({
      next: (res: any) => {
        localStorage.setItem('token', res.token); // ✅ guarda o token localmente
        this.router.navigate(['/dashboard']); // ✅ redireciona após login
      },
      error: (err) => {
        this.errorMessage = 'Usuário ou senha inválidos';
        console.error(err);
      }
    });
  }
}
