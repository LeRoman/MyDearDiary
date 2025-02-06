import { Component, Input, ViewEncapsulation } from '@angular/core';
import { MaterialModules } from '../../../material.import';
import { Record } from '../../../models/record';
import { RecordService } from '../../../services/record.service';
import { MatPaginator } from '@angular/material/paginator';
import { NgIf } from '@angular/common';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-record',
  standalone: true,
  imports: [MaterialModules, NgIf],
  templateUrl: './record.component.html',
  styleUrl: './record.component.css',
  encapsulation: ViewEncapsulation.None,
})
export class RecordComponent {
  constructor(private recordService: RecordService) {
    this.imageUrl = environment.production
      ? 'http://165.232.75.90/'
      : `${environment.apiUrl}/`;
  }
  imageUrl: string = '';
  deleteRecord(id: string) {
    this.recordService.deleteRecord(id).subscribe((data) => this.triggerPage());
  }
  @Input() record: Record = new Record();
  @Input() triggerPage!: () => void;
}
