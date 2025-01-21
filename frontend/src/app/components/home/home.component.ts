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

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MaterialModules, FormsModule, NgFor, NgIf, RecordComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent implements OnInit {
  showRecordContainer() {
    this.showNewRecordContainer = !this.showNewRecordContainer;
  }
  public recordList: Record[] = [];
  public showNewRecordContainer = false;
  constructor(private recordService: RecordService) {}

  body: string = '';

  ngOnInit(): void {
    console.log('invoked!');
    this.recordService
      .getRecords()
      .subscribe((data) => (this.recordList = data));
  }
  
  getRecordList() {
    this.recordService
      .getRecords()
      .subscribe((data) => (this.recordList = data));
  }
}
