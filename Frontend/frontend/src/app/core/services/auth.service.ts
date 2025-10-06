import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { LoginRequest, User } from '../models/user.model';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'api/Auth';
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  private hasToken(): boolean {
    return !!localStorage.getItem('authToken');
  }

  login(credentials: LoginRequest): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        localStorage.setItem('authToken', response.token);
        this.loggedIn.next(true);
        // Implement token decoding to get User info/Role for routing later
      }),
      catchError(error => {
        this.loggedIn.next(false);
        throw error;
      })
    );
  }

  logout(): void {
    localStorage.removeItem('authToken');
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  // A basic implementation to get the user's role from the token (for [Authorize(Roles)])
  getRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      // The backend puts the role in the claim `http://schemas.microsoft.com/ws/2008/06/identity/claims/role`
      // You may need to adjust the claim key depending on your JWT setup.
      return payload.role || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    } catch (e) {
      console.error("Failed to decode token:", e);
      return null;
    }
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }
}