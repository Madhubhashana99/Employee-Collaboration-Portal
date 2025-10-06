export interface Comment {
  commentID: number;
  content: string;
  createdAt: string;
  authorID: number;
  authorName: string;
}

export interface CommentCreate {
  content: string;
}