import { Component, OnInit } from '@angular/core';
import { RecordService } from '../../../services/record.service';
import { Record } from '../../../models/record';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { UserLoginDto } from '../../../models/Auth/user-login-dto';
import { MaterialModules } from '../../../material.import';
import { MatDialogContent, MatDialogTitle } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { RecordComponent } from '../record/record.component';
import { NewRecord } from '../../../models/new-record';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MaterialModules, FormsModule, NgFor, NgIf, RecordComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  public recordList: Record[] = [];
  public showNewRecordContainer = false;
  newRecord: NewRecord = new NewRecord();

  constructor(
    private recordService: RecordService,
    private router: Router,
    private authService: AuthService
  ) {}

  clearList(): void {
    this.recordList = [];
  }

  addRecord(newRecord: NewRecord) {
    this.recordService.addRecord(newRecord).subscribe(() => {
      this.clearList();
      this.getRecordList();
    });
  }

  showRecordContainer() {
    this.showNewRecordContainer = !this.showNewRecordContainer;
  }

  ngOnInit(): void {
    this.recordService
      .getRecords()
      .subscribe((data) => (this.recordList = data));
  }

  getRecordList() {
    this.recordService.getRecords().subscribe((data) => {
      this.recordList = data;
      this.closeNewPostContainer();
    });
  }

  closeNewPostContainer(): void {
    this.showRecordContainer();
    this.newRecord.Content = '';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
