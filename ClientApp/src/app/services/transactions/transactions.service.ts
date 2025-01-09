import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Transaction } from '../../models/transaction';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class TransactionsService {
  private readonly apiUrl = environment.apiUrl;
  constructor(private readonly http: HttpClient) {}

  get(userId: string): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${this.apiUrl}/transaction/${userId}`);
  }
  delete(id: string): Observable<number> {
    return this.http.delete<number>(`${this.apiUrl}/transaction/${id}`);
  }

  insert(transaction: Transaction): Observable<Transaction> {
    // console.log('Transaction to insert:', transaction);

    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Transaction>(`${this.apiUrl}/transaction`, transaction, { headers });
  }

  generateReport(transactions: Transaction[]): Observable<Transaction[]> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<Transaction[]>(`${this.apiUrl}/transaction/send-message`, { transactions }, { headers });
  }

}
