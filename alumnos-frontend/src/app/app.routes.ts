import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: 'students', loadComponent: () => import('./features/students/student-dashboard.component').then(m => m.StudentDashboardComponent) },
  { path: '', redirectTo: '/students', pathMatch: 'full' }
];
