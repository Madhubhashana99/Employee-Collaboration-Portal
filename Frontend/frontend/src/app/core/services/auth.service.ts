import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';
// You might need to install 'jwt-decode': npm install jwt-decode
import { jwtDecode } from 'jwt-decode'; 

// --- Models (Assuming these interfaces are defined in src/app/models/user.ts) ---
export interface TokenResponse {
  token: string;
}
// --------------------------------------------------------------------------------

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/Auth';

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string) {
    // Backend endpoint: POST /api/Auth/login
    return this.http.post<TokenResponse>(`${this.apiUrl}/login`, { username, password });
  }

  setToken(token: string): void {
    localStorage.setItem('auth_token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  getUserId(): number | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      // Ensure this claim key matches what your .NET backend uses for User ID (typically 'nameid')
      return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
        ? parseInt(decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']) 
        : null;
    } catch (e) {
      return null;
    }
  }

  getUserRole(): 'Admin' | 'Employee' | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const decoded: any = jwtDecode(token);
      // Ensure this claim key matches what your .NET backend uses for Role
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return (role === 'Admin' || role === 'Employee') ? role : null;
    } catch (e) {
      return null;
    }
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token; // Check if token exists
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    this.router.navigate(['/login']);
  }
}