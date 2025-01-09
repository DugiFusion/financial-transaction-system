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
}