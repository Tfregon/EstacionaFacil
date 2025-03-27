import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../services/dashboard.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // 👈 importar

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule], // 👈 adicionar aqui
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})

export class DashboardComponent implements OnInit {
  vehicles: any[] = [];

  // 👇 Armazena os dados do novo veículo a ser registrado
  novoVeiculo = {
    plate: '',
    model: '',
    color: '',
    ownerName: ''
  };

  // 👇 Controla se o formulário de registro está visível
  exibirFormulario = false;

  constructor(
    private dashboardService: DashboardService,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.carregarVeiculos();
  }

  // 🔄 Busca veículos ativos do backend
  carregarVeiculos() {
    this.dashboardService.getActiveVehicles().subscribe({
      next: (data: any) => this.vehicles = data,
      error: (err) => console.error('Erro ao buscar veículos:', err)
    });
  }

  // 👉 Exibe o formulário de registro
  abrirFormulario() {
    this.exibirFormulario = true;
  }

  // ❌ Fecha o formulário sem registrar
  fecharFormulario() {
    this.exibirFormulario = false;
  }

  // ✅ Registra o novo veículo no backend
  registrarEntrada() {
    const { plate, model, color, ownerName } = this.novoVeiculo;

    // ⚠️ Verifica se todos os campos estão preenchidos
    if (!plate || !model || !color || !ownerName) {
      alert('Preencha todos os campos antes de registrar.');
      return;
    }

    this.http.post('https://localhost:7105/api/Vehicles/entry', this.novoVeiculo)
      .subscribe({
        next: () => {
          alert('Veículo registrado com sucesso!');
          this.novoVeiculo = { plate: '', model: '', color: '', ownerName: '' }; // 🔄 limpa campos
          this.carregarVeiculos(); // 🔃 recarrega a lista
          this.fecharFormulario(); // ❌ fecha formulário
        },
        error: (err) => {
          console.error('Erro ao registrar entrada:', err);
          alert('Erro ao registrar veículo.');
        }
      });
  }

  // 🏁 Confirma saída do veículo
  registrarSaida(vehicleId: string) {
    const valorInput = window.prompt('Informe o valor adicional a ser cobrado (em dólares):', '0');

    if (valorInput === null) return;

    const valorAdicional = parseFloat(valorInput);
    if (isNaN(valorAdicional)) {
      alert('Valor inválido. Saída cancelada.');
      return;
    }

    const confirmar = window.confirm(`Deseja confirmar a saída com valor adicional de $${valorAdicional.toFixed(2)}?`);
    if (!confirmar) return;

    this.http.put(`https://localhost:7105/api/Vehicles/exit/${vehicleId}`, valorAdicional)
      .subscribe({
        next: () => {
          alert('Saída registrada com sucesso!');
          this.carregarVeiculos();
        },
        error: (err) => {
          console.error('Erro ao registrar saída:', err);
          alert('Erro ao registrar saída: ' + err.error);
        }
      });
  }

  // 💰 Navega para tela de caixa
  verCaixa() {
    this.router.navigate(['/cash']);
  }

  // 📜 Navega para tela de histórico
  verHistorico() {
    this.router.navigate(['/history']);
  }

  // 🔄 Recarrega a lista
  atualizar() {
    this.carregarVeiculos();
  }
}
