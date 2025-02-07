import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MaterialModules } from '../../../material.import';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-restore-account-diaolog',
  imports: [MaterialModules, FormsModule],
  templateUrl: './restore-account-diaolog.component.html',
  styleUrl: './restore-account-diaolog.component.css',
})
export class RestoreAccountDiaologComponent {
  constructor(public dialogRef: MatDialogRef<RestoreAccountDiaologComponent>) {}
  public password: string = '';

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(this.password);
  }
}
