import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../services/dashboard.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})

export class DashboardComponent implements OnInit {
  vehicles: any[] = [];

  // Dados do novo veículo
  novoVeiculo = {
    plate: '',
    model: '',
    color: '',
    ownerName: ''
  };


  exibirFormulario = false;

  // 💰 Caixa diário
  cashToday: number | null = null;
  mostrarCaixa = false;

  constructor(
    private dashboardService: DashboardService,
    public router: Router,
    private http: HttpClient
  ) {}

  ngOnInit() {
    this.carregarVeiculos();
  }

  carregarVeiculos() {
    this.dashboardService.getActiveVehicles().subscribe({
      next: (data: any) => this.vehicles = data,
      error: (err) => console.error('Erro ao buscar veículos:', err)
    });
  }

  abrirFormulario() {
    this.exibirFormulario = true;
  }

  fecharFormulario() {
    this.exibirFormulario = false;
  }

  registrarEntrada() {
    const { plate, model, color, ownerName } = this.novoVeiculo;

    if (!plate || !model || !color || !ownerName) {
      alert('Preencha todos os campos antes de registrar.');
      return;
    }

    this.http.post('https://estacionafacilapi20250404165631.azurewebsites.net/api/Vehicles/entry', this.novoVeiculo)
      .subscribe({
        next: () => {
          alert('Veículo registrado com sucesso!');
          this.novoVeiculo = { plate: '', model: '', color: '', ownerName: '' };
          this.carregarVeiculos();
          this.fecharFormulario();
        },
        error: (err) => {
          console.error('Erro ao registrar entrada:', err);
          alert('Erro ao registrar veículo.');
        }
      });
  }

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

    this.http.put(`https://estacionafacilapi20250404165631.azurewebsites.net/api//Vehicles/exit/${vehicleId}`, valorAdicional)
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

  // 💰 Carrega o caixa de hoje e exibe na tela
  carregarCaixaHoje() {
    this.http.get<any>('https://estacionafacilapi20250404165631.azurewebsites.net/api/api/Cash/today').subscribe({
      next: (res) => {
        this.cashToday = res.total;
        this.mostrarCaixa = true;
      },
      error: (err) => {
        console.error('Erro ao carregar caixa do dia:', err);
        alert('Erro ao carregar caixa do dia');
      }
    });
  }

  verCaixa() {
    this.carregarCaixaHoje();
  }

  // adicione no início
  filtroData: string = '';
  valorCaixa: number | null = null;

  buscarCaixaPorData() {
    if (!this.filtroData) {
      alert('Selecione uma data para filtrar.');
      return;
    }

    const dataFormatada = new Date(this.filtroData).toISOString().split('T')[0]; // yyyy-mm-dd

    this.http.get<any>(`https://estacionafacilapi20250404165631.azurewebsites.net/api/Cash/${dataFormatada}`)
      .subscribe({
        next: (res) => {
          this.valorCaixa = res.total;
        },
        error: (err) => {
          console.error('Erro ao buscar caixa por data:', err);
          alert('Erro ao buscar caixa.');
        }
      });
  }

  
  verHistorico() {
    this.router.navigate(['/history']);
  }

  sistemaStatus: any = null;
  mostrarStatus = false;

  sistemaVersao: any = null;
  mostrarVersao = false;

  // 🔧 Carrega status rápido do sistema
  carregarStatusSistema() {
    this.http.get<any>('https://estacionafacilapi20250404165631.azurewebsites.net/api/Settings/status').subscribe({
      next: (res) => {
        this.sistemaStatus = res;
        this.mostrarStatus = true;
      },
      error: (err) => {
        console.error('Erro ao buscar status:', err);
        alert('Erro ao buscar status.');
      }
    });
  }

  // 📦 Carrega info de versão do sistema
  carregarVersaoSistema() {
    this.http.get<any>('https://estacionafacilapi20250404165631.azurewebsites.net/api/Settings/version').subscribe({
      next: (res) => {
        this.sistemaVersao = res;
        this.mostrarVersao = true;
      },
      error: (err) => {
        console.error('Erro ao buscar versão:', err);
        alert('Erro ao buscar versão.');
      }
    });
  }

  // ❌ Oculta
  fecharStatus() {
    this.mostrarStatus = false;
  }

  fecharVersao() {
    this.mostrarVersao = false;
  }


  atualizar() {
    this.carregarVeiculos();
  }
}
