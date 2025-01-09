import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Transaction } from '../../models/transaction';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {

  private readonly apiUrl = environment.apiUrl;
  constructor(private readonly http: HttpClient) {}

  generate(transactions: Transaction[]): Observable<Transaction[]> {
    console.log('Transactions to generate:', transactions);

    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
   // const message = JSON.stringify(transactions); 
    return this.http.post<Transaction[]>(`${this.apiUrl}/transaction/send-message`, { transactions }, { headers });
  }
}