import { Address } from "./address";

export interface Activity {
  id: number;
  description: string;
  category: string;
  date: Date;
  otherInfo: string;

  address: Address;
  displayName: string;
}
