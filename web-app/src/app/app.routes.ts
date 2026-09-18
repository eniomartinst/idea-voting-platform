import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { Dashboard } from './features/ideas/dashboard/dashboard';
import { Layout } from './shared/components/layout/layout';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'register', component: Register },
  {
    path: '',
    component: Layout,
    children: [
      { path: 'dashboard', component: Dashboard },
    ]
  },
  { path: '**', redirectTo: 'login' }
];