import { TransactionType } from "./enums/transaction-type";

export interface Transaction {
  id: string;
  accountId: string;
  customerId: string;
  note: string;
  amount: number;
  type: TransactionType;
  createdDate: Date;
}
