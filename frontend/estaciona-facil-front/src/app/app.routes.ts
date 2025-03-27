import { Routes } from '@angular/router';
import { LoginComponent } from './auth/auth/login/login.component';
import { authGuard } from './auth/auth/auth.guard';
import { RegisterComponent } from './pages/register/register.component';


export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'history',
    loadComponent: () => import('./pages/history/history.component').then(m => m.HistoryComponent)
  },  
  {
    path: 'dashboard',
    canActivate: [authGuard],
    loadComponent: () => import('./dash/dashboard/dashboard.component').then(m => m.DashboardComponent)
  }  
];
