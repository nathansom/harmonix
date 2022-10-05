import { User } from "./user";

export class UserParams {
  occupation: string = "";
  skill: string = "";
  genre: string = "";
  city: string = "";
  provinceOrState: string = "";
  country: string = "";
  pageNumber = 1;
  pageSize = 5;
  orderBy = 'lastActive';

  constructor(user: User) {
  }
}