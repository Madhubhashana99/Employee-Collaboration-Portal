import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post, PostCreate } from '../models/post.model';
import { Comment, CommentCreate } from '../models/comment.model';

@Injectable({ providedIn: 'root' })
export class PostService {
  private postApiUrl = 'api/Posts';

  constructor(private http: HttpClient) {}

  // Post Management
  getPosts(authorId?: number, sortBy: 'recent' | 'likes' = 'recent'): Observable<Post[]> {
    let params = new HttpParams().set('sortBy', sortBy);
    if (authorId) {
        params = params.set('authorId', authorId.toString());
    }

    // The backend PostsController.GetPosts returns a slightly different structure, 
    // we'll rely on it matching the Post interface for simplicity here.
    return this.http.get<Post[]>(this.postApiUrl, { params });
  }

  createPost(post: PostCreate): Observable<any> {
    return this.http.post(this.postApiUrl, post);
  }

  updatePost(postId: number, post: PostCreate): Observable<any> {
    return this.http.put(`${this.postApiUrl}/${postId}`, post);
  }

  deletePost(postId: number): Observable<any> {
    return this.http.delete(`${this.postApiUrl}/${postId}`);
  }

  // Comments
  getComments(postId: number): Observable<Comment[]> {
    return this.http.get<Comment[]>(`${this.postApiUrl}/${postId}/comments`);
  }

  addComment(postId: number, comment: CommentCreate): Observable<Comment> {
    return this.http.post<Comment>(`${this.postApiUrl}/${postId}/comments`, comment);
  }

  // Interactions (Likes/Dislikes)
  reactToPost(postId: number, userId: number, interactionType: 'Like' | 'Dislike'): Observable<any> {
    // Note: The backend PostInteractionsController expects a PostInteraction model
    // which includes PostID, UserID, and InteractionType.
    return this.http.post('api/PostInteractions/react', { postId, userId, interactionType });
  }

  getInteractionSummary(postId: number): Observable<{ postId: number, likes: number, dislikes: number }> {
    return this.http.get<{ postId: number, likes: number, dislikes: number }>(`api/PostInteractions/${postId}/summary`);
  }
}