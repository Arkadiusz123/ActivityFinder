import { Address } from "./address";

export interface Activity {
  id: number;
  description: string;
  title: string;
  category: string;
  date: Date;
  otherInfo: string;
  usersLimit: number;

  address: Address;
  displayName: string;
}

export interface ActivityListItem {
  id: number;
  date: Date;
  title: string;
  address: string;
  createdByUser: boolean;
  joinedUsers: number;
  usersLimit?: number;
  alreadyJoined: boolean;
  description: string;

  usersCount?: string;
}
