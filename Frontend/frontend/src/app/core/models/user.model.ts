export interface User {
  userId: number;
  username: string;
  firstName: string;
  lastName: string;
  role: 'Admin' | 'Employee';
}

export interface UserCreate {
  username: string;
  password: string;
  firstName: string;
  lastName: string;
  role: 'Admin' | 'Employee';
}

export interface LoginRequest {
  username: string;
  password: string;
}