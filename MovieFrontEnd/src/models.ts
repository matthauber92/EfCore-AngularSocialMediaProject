export class AppUser {
    FirstName?: string;
    LastName?: string;
    DisplayName?: string;
    Bio?: string;
    Picture?: string;
}

export class Posts {
  PostId?: number;
  Content?: string;
  Likes: number;
  User: AppUser;
  Comments: Comments[];
}

export class Comments {
  CommentId?: number;
  Title: string;
  Content?: string;
  User: AppUser;
  Posts: Posts;
}
