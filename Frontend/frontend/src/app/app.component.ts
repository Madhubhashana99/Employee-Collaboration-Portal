import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="navbar">
      <div class="logo" [routerLink]="['/posts']">
        Employee Portal
      </div>
      <div class="nav-links">
        <a [routerLink]="['/posts']">Posts</a>
        <a *ngIf="authService.isAdmin()" [routerLink]="['/admin/users']">User Management</a>
      </div>
      <div class="user-actions">
        <div *ngIf="authService.isAuthenticated(); else loginButton">
          <span class="welcome-message">
            Welcome, {{ authService.getRole() }}!
          </span>
          <button (click)="authService.logout()" class="btn-logout">Logout</button>
        </div>
        <ng-template #loginButton>
          <button [routerLink]="['/login']" class="btn-login">Login</button>
        </ng-template>
      </div>
    </nav>
    
    <main class="content">
      <router-outlet></router-outlet>
    </main>
  `,
  styles: [`
    .navbar {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding: 15px 30px;
      background-color: #2c3e50;
      color: white;
      box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }
    .logo {
      font-size: 1.5em;
      font-weight: bold;
      cursor: pointer;
    }
    .nav-links a {
      color: white;
      text-decoration: none;
      margin: 0 15px;
      padding: 5px 10px;
      border-radius: 4px;
      transition: background-color 0.2s;
    }
    .nav-links a:hover {
      background-color: #34495e;
    }
    .welcome-message {
      margin-right: 15px;
      font-size: 0.9em;
    }
    .btn-login, .btn-logout {
      padding: 8px 15px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-weight: bold;
    }
    .btn-login {
      background-color: #27ae60;
      color: white;
    }
    .btn-logout {
      background-color: #e74c3c;
      color: white;
    }
    .content {
      padding: 20px;
    }
  `]
})
export class AppComponent {
  authService = inject(AuthService);
}