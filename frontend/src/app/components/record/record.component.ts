import { Component, Input, ViewEncapsulation } from '@angular/core';
import { MaterialModules } from '../../../material.import';

@Component({
  selector: 'app-record',
  standalone: true,
  imports: [MaterialModules],
  templateUrl: './record.component.html',
  styleUrl: './record.component.css',
  encapsulation: ViewEncapsulation.None,
})
export class RecordComponent {
  @Input() record: any;
}
