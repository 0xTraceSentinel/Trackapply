import { Routes } from '@angular/router';
import { authGuard } from '@core';

export const routes: Routes = [
    {
        path: 'login',
        title: 'Login | Trackapply',
        loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
    },
    {
        path: 'dashboard',
        title: 'Dashboard | Trackapply',
        canActivate: [authGuard],
        loadComponent: () => import('./features/dashboard/dashboard').then((m) => m.Dashboard),
    },
    {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full',
    },
    {
        path: '**',
        redirectTo: 'dashboard',
    },
];
