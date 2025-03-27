import { Routes } from '@angular/router';
import { LoginComponent } from './auth/auth/login/login.component';
import { DashboardComponent } from './dash/dashboard/dashboard.component';
import { authGuard } from './auth/auth/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./dash/dashboard/dashboard.component').then(m => m.DashboardComponent)
  }  
];
