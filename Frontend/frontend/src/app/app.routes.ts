import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { PostListComponent } from './features/posts/post-list/post-list.component';
import { PostCreateComponent } from './features/posts/post-create/post-create.component';
import { PostDetailsComponent } from './features/posts/post-details/post-details.component';
import { UserListComponent } from './features/admin/user-list/user-list.component';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';

// NOTE: You would need to create AuthGuard and AdminGuard files as well.

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'posts', component: PostListComponent, canActivate: [AuthGuard] },
  { path: 'posts/create', component: PostCreateComponent, canActivate: [AuthGuard] },
  { path: 'posts/:id', component: PostDetailsComponent, canActivate: [AuthGuard] },
  // Admin-only route for User Management
  { path: 'admin/users', component: UserListComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: '', redirectTo: '/posts', pathMatch: 'full' },
  { path: '**', redirectTo: '/posts' } // Fallback
];