import { Component, OnInit } from '@angular/core';
import { RecordService } from '../../../services/record.service';
import { Record } from '../../../models/record';
import { NgFor, NgForOf, NgIf, ViewportScroller } from '@angular/common';
import { MaterialModules } from '../../../material.import';
import { MatDialogContent, MatDialogTitle } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { RecordComponent } from '../record/record.component';
import { NewRecord } from '../../../models/new-record';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { PageEvent } from '@angular/material/paginator';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    MaterialModules,
    FormsModule,
    NgFor,
    NgIf,
    RecordComponent,
    MatFormFieldModule,
    MatDatepickerModule,
  ],
  providers: [provideNativeDateAdapter()],
  //changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  public recordList: Record[] = [];
  public showNewRecordContainer = false;
  newRecord: NewRecord = new NewRecord();

  currentPage = 0;
  pageSize = 5;
  paginatorLength = 0;

  searchField: string = '';
  startDateField: string = '';
  endDateField: string = '';

  constructor(
    private recordService: RecordService,
    private router: Router,
    private authService: AuthService,
    private viewportScroller: ViewportScroller
  ) {}

  addRecord(newRecord: NewRecord) {
    this.recordService.addRecord(newRecord).subscribe(() => {
      this.clearList();
      this.getRecordList();
    });
  }

  clearList(): void {
    this.recordList = [];
  }

  showRecordContainer() {
    this.showNewRecordContainer = !this.showNewRecordContainer;
  }

  closeNewPostContainer(): void {
    this.showRecordContainer();
    this.newRecord.Content = '';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  ngOnInit(): void {
    this.recordService.getRecords().subscribe((data) => {
      this.recordList = data.data;
      this.paginatorLength = data.count;
    });
  }

  getRecordList() {
    console.log('getRecordList');
    this.recordService.getRecords().subscribe((data) => {
      this.recordList = data.data;
      this.closeNewPostContainer();
    });
  }

  gerFilteredRecords() {
    const queryParams = this.buildParamsArray();
    const queryString = this.buildQueryString(queryParams);
    console.log(queryString);
    this.recordService.getParametrizedRecords(queryString).subscribe((data) => {
      this.recordList = data.data;
      this.paginatorLength = data.count;
    });
  }

  handlePageEvent(pageEvent: PageEvent) {
    console.log('handlePageEvent', pageEvent);
    const queryParams = this.buildParamsArray();
    this.pageSize = pageEvent.pageSize;
    this.currentPage = pageEvent.pageIndex;

    queryParams['page'] = this.currentPage.toString();
    queryParams['pageSize'] = this.pageSize.toString();
    this.clearList();
    const queryString = this.buildQueryString(queryParams);
    console.log(queryString);
    this.recordService.getParametrizedRecords(queryString).subscribe((data) => {
      this.recordList = data.data;
      this.paginatorLength = data.count;
    });
    this.viewportScroller.scrollToPosition([0, 0]);
  }

  onPageChange(event: any): void {}

  buildParamsArray() {
    const queryParams: { [key: string]: string } = {};

    if (this.searchField) {
      queryParams['searchFragment'] = this.searchField;
    }
    if (this.startDateField) {
      console.log(this.startDateField);
      queryParams['startDate'] = this.startDateField;
    }
    if (this.endDateField) {
      queryParams['endDate'] = this.endDateField;
    }

    return queryParams;
  }
  buildQueryString(params: { [key: string]: string }): string {
    return Object.entries(params)
      .map(([key, value]) => `${key}=${encodeURIComponent(value)}`)
      .join('&');
  }
}
