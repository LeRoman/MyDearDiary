import { Component } from '@angular/core';
import { MaterialModules } from '../../../material.import';
import { MatDialogRef } from '@angular/material/dialog';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-delete-account-dialog',
  imports: [MaterialModules, FormsModule],
  templateUrl: './delete-account-dialog.component.html',
  styleUrl: './delete-account-dialog.component.css',
})
export class DeleteAccountDialogComponent {
  constructor(public dialogRef: MatDialogRef<DeleteAccountDialogComponent>) {}
  public password: string = '';

  onNoClick(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(this.password);
  }
}
