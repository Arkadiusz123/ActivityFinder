import { Address } from "./address";

export interface Activity {
  id: number;
  description: string;
  category: string;
  date: Date;

  address: Address;
  displayName: string;
}
