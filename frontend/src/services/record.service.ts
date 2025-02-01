import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Record } from '../models/record';
import { NewRecord } from '../models/new-record';
import { PagedResult } from '../models/paged-result';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RecordService {
  private readonly basicUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  addRecord(newRecord: NewRecord): Observable<any> {
    console.log('add record invoke');
    const formData = new FormData();
    formData.append('Content', newRecord.Content);
    formData.append('Images', newRecord.Image!);
    return this.http.post(this.basicUrl + '/api/record', formData);
  }

  getRecords(): Observable<PagedResult> {
    console.log('getrecords invoked');
    return this.http.get<PagedResult>(this.basicUrl + '/api/record');
  }

  getParametrizedRecords(paremaetrQuery: string): Observable<PagedResult> {
    return this.http.get<PagedResult>(
      this.basicUrl + '/api/record?' + paremaetrQuery
    );
  }

  deleteRecord(id: string): Observable<void> {
    return this.http.delete<void>(this.basicUrl + '/api/record/' + id);
  }
}
