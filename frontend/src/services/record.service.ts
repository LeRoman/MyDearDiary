import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Record } from '../models/record';

@Injectable({
  providedIn: 'root',
})
export class RecordService {
  constructor(private http: HttpClient) {}

  getRecords(): Observable<Record[]> {
    return this.http.get<Record[]>('https://localhost:7094/api/record');
  }

  // getRecord(id: number): Observable<Record> {
  //   return this.http.get<Record>(`${this.apiUrl}/${id}`);
  // }
  //
  // createRecord(Record: Record): Observable<Record> {
  //   return this.http.post<Record>(this.apiUrl, Record);
  // }
  //
  // updateRecord(id: number, Record: Record): Observable<Record> {
  //   return this.http.put<Record>(`${this.apiUrl}/${id}`, Record);
  // }
  //
  // deleteRecord(id: number): Observable<void> {
  //   return this.http.delete<void>(`${this.apiUrl}/${id}`);
  // }
}
