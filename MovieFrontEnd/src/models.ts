export class AppUser {
    id?: number;
    userName?: string;
    firstName?: string;
    lastName?: string;
    displayName?: string;
    bio?: string;
    picture?: string;
}

export class Posts {
  postId?: number;
  content?: string;
  likes?: number;
  rePostUser?: string;
  //User: AppUser;
  showMe?: false;
  comments?: Comments[];
}

export class Comments {
  commentId?: number;
  title?: string;
  content?: string;
  //User: AppUser;
  userName?: string;
  posts?: Posts;
}

export class Friends {
  friendId?: number;
  isFriend?: boolean;
  user?: AppUser;
  showRequests?: boolean;
}

export class Notifications {
  notificationId?: number;
  messages?: number;
  friendRequests?: number;
  friendSentId?: number;
  user?: AppUser;
}
