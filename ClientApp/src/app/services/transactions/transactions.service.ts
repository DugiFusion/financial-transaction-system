import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { Transaction } from '../../models/transaction';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TransactionsService {

  constructor(private readonly http: HttpClient) {}
  get(): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(
      `http://localhost:5001/gateway/transaction/12cd25aa-4022-4a0c-a173-e597b390dcc3`
    );
  }
}
