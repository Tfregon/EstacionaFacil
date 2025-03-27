import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.css']
})
export class HistoryComponent implements OnInit {
  historico: any[] = [];
  totalVeiculos: number = 0;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<any[]>('https://localhost:7105/api/Vehicles').subscribe({
      next: (res) => {
        this.historico = res;
        this.totalVeiculos = res.length;
      },
      error: (err) => {
        console.error('Erro ao carregar histórico:', err);
        alert('Erro ao carregar histórico de veículos.');
      }
    });
  }
}

