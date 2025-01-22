import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Record } from '../models/record';
import { NewRecord } from '../models/new-record';

@Injectable({
  providedIn: 'root',
})
export class RecordService {
  private readonly basicUrl: string = 'https://localhost:7094/api';
  constructor(private http: HttpClient) {}

  addRecord(newRecord: NewRecord): Observable<any> {
    console.log('add record invoke');
    const formData = new FormData();
    formData.append('Content', newRecord.Content);
    return this.http.post(this.basicUrl + '/record', formData);
  }

  getRecords(): Observable<Record[]> {
    return this.http.get<Record[]>(this.basicUrl + '/record');
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
