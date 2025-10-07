// src/app/features/posts/post-detail/post-detail.component.ts

import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Post } from 'src/app/core/models/post.model';
import { Comment, CommentCreate } from 'src/app/core/models/comment.model';
import { PostsService } from '../posts.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/core/models/auth.model';

@Component({
  selector: 'app-post-detail',
  templateUrl: './post-detail.component.html',
  styleUrls: ['./post-detail.component.css']
})
export class PostDetailComponent implements OnInit {
  @Input() post!: Post;
  @Output() postDeleted = new EventEmitter<number>();
  @Output() interacted = new EventEmitter<{ postId: number, type: 'Like' | 'Dislike' }>();

  comments: Comment[] = [];
  showComments: boolean = false;
  commentForm!: FormGroup; // Initialized in constructor
  isOwner: boolean = false;
  currentUser: User | null = null;

  constructor(
    private postsService: PostsService,
    private authService: AuthService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.currentUser = this.authService.currentUserValue;

    // Determine ownership based on the author information available in the post list
    // NOTE: This comparison relies on the API providing a unique identifier (like username) for comparison
    this.isOwner = this.currentUser ? (this.currentUser.username === this.post.author) : false;

    // Initialize comment form with validation (Bonus Feature) 
    this.commentForm = this.fb.group({
      content: ['', [Validators.required, Validators.maxLength(500)]]
    });
  }

  toggleComments(): void {
    this.showComments = !this.showComments;
    if (this.showComments && this.comments.length === 0) {
      this.loadComments(); // Load comments only when first opening the section
    }
  }

  loadComments(): void {
    // Fetches comments for the post (Core Feature 3) [cite: 23]
    this.postsService.getComments(this.post.postId).subscribe({
      next: (data) => {
        this.comments = data; 
      },
      error: (err) => console.error('Error loading comments:', err)
    });
  }

  onAddComment(): void {
    if (this.commentForm.invalid || !this.currentUser) return;

    const commentCreate: CommentCreate = this.commentForm.value;
    
    // Submits the comment (Core Feature 3) [cite: 23]
    this.postsService.addComment(this.post.postId, commentCreate).subscribe({
      next: (newComment) => {
        this.comments.push(newComment); 
        this.commentForm.reset();
        // Manually mark form as pristine/untouched after reset to clear validation state
        Object.keys(this.commentForm.controls).forEach(key => {
            this.commentForm.get(key)?.setErrors(null);
        });
      },
      error: (err) => console.error('Error adding comment:', err)
    });
  }

  onLike(): void {
    // Emits an event to the parent to handle the interaction (Core Feature 4) [cite: 27]
    this.interacted.emit({ postId: this.post.postId, type: 'Like' });
  }

  onDislike(): void {
    // Emits an event to the parent to handle the interaction (Core Feature 4) [cite: 27]
    this.interacted.emit({ postId: this.post.postId, type: 'Dislike' });
  }

  onDelete(): void {
    // Emits an event to the parent to handle deletion (Core Feature 2) [cite: 21]
    this.postDeleted.emit(this.post.postId); 
  }

  onEdit(): void {
    // Placeholder: In a real app, this would navigate to the edit route.
    // E.g., this.router.navigate(['/posts', this.post.postId, 'edit']);
    console.log(`Maps to edit post: ${this.post.postId}`);
  }
}