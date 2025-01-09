import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Report } from '../../models/report';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {

  private readonly apiUrl = environment.apiUrl;
  constructor(private readonly http: HttpClient) { }

  get(userId: string): Observable<Report[]> {
    return this.http.get<Report[]>(`${this.apiUrl}/reporting/${userId}`);
  }
}