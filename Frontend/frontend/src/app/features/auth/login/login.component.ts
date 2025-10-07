import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="login-container">
      <h2>Employee Portal Login</h2>
      <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
        <div class="form-group">
          <label for="username">Username</label>
          <input id="username" type="text" formControlName="username" required>
          <div *ngIf="loginForm.controls['username'].invalid && loginForm.controls['username'].touched" class="error">
            Username is required.
          </div>
        </div>
        
        <div class="form-group">
          <label for="password">Password</label>
          <input id="password" type="password" formControlName="password" required>
          <div *ngIf="loginForm.controls['password'].invalid && loginForm.controls['password'].touched" class="error">
            Password is required.
          </div>
        </div>

        <button type="submit" [disabled]="loginForm.invalid || loading">
          {{ loading ? 'Logging In...' : 'Login' }}
        </button>
        <p *ngIf="error" class="error-message">{{ error }}</p>
      </form>
      
      <div class="hint">
        <p>Hint: Use a pre-created Admin or Employee user from your database.</p>
        <p>Example: User: admin / Pass: password</p>
      </div>
    </div>
  `,
  styles: [`
    .login-container { max-width: 400px; margin: 50px auto; padding: 20px; border: 1px solid #ccc; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); background-color: white; }
    h2 { text-align: center; color: #2c3e50; margin-bottom: 30px; }
    .form-group { margin-bottom: 15px; }
    label { display: block; margin-bottom: 5px; font-weight: bold; color: #333; }
    input { width: 100%; padding: 10px; border: 1px solid #ddd; border-radius: 4px; box-sizing: border-box; }
    input:focus { border-color: #007bff; outline: none; }
    button { 
      width: 100%; padding: 10px; background-color: #27ae60; color: white; 
      border: none; border-radius: 4px; cursor: pointer; font-size: 1.1em; 
      transition: background-color 0.2s;
    }
    button:hover:not(:disabled) { background-color: #2ecc71; }
    button:disabled { background-color: #ccc; cursor: not-allowed; }
    .error { color: #e74c3c; font-size: 0.85em; margin-top: 5px; }
    .error-message { color: #e74c3c; margin-top: 15px; text-align: center; font-weight: bold; }
    .hint { margin-top: 20px; padding: 10px; background-color: #f0f0f0; border-radius: 4px; font-size: 0.9em; }
  `]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loading = false;
  error: string | null = null;

  // Form Validation is implemented here (Bonus feature)
  loginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  onSubmit() {
    this.error = null;
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    const { username, password } = this.loginForm.value;

    if (username && password) {
      this.authService.login(username, password).subscribe({
        next: (res) => {
          this.authService.setToken(res.token);
          // Navigate to the main post list after successful login
          this.router.navigate(['/posts']); 
        },
        error: (err) => {
          this.error = 'Invalid username or password';
          this.loading = false;
        },
        complete: () => {
          this.loading = false;
        }
      });
    }
  }
}