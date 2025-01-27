import { Component, Input, ViewEncapsulation } from '@angular/core';
import { MaterialModules } from '../../../material.import';
import { Record } from '../../../models/record';
import { RecordService } from '../../../services/record.service';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-record',
  standalone: true,
  imports: [MaterialModules],
  templateUrl: './record.component.html',
  styleUrl: './record.component.css',
  encapsulation: ViewEncapsulation.None,
})
export class RecordComponent {
  constructor(private recordService: RecordService) {}
  deleteRecord(id: string) {
    this.recordService.deleteRecord(id).subscribe();
    this.triggerPage();
  }
  @Input() record: Record = new Record();
  @Input() triggerPage!: () => void;
}
