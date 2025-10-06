export interface Post {
  postID: number;
  title: string;
  content: string;
  author: string; // From backend's Select (Author.Username or full name)
  createdAt: string; // ISO date string
  updatedAt?: string; // Optional
  likes: number;
  dislikes: number;
}

export interface PostCreate {
  title: string;
  content: string;
}