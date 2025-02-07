import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class SnackBarService {
  public constructor(private snackBar: MatSnackBar) {}

  public showErrorMessage(error: any) {
    this.snackBar.open(error, '', {
      duration: 5000,
      panelClass: 'error-snack-bar',
    });
  }

  public showUsualMessage(message: any) {
    this.snackBar.open(message, '', {
      duration: 5000,
      panelClass: 'usual-snack-bar',
    });
  }
}
