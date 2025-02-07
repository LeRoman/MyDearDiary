import { Component } from '@angular/core';
import { MaterialModules } from '../../../material.import';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RestoreAccountDiaologComponent } from '../restore-account-diaolog/restore-account-diaolog.component';

@Component({
  selector: 'app-restore',
  imports: [MaterialModules],
  templateUrl: './restore.component.html',
  styleUrl: './restore.component.css',
})
export class RestoreComponent {
  constructor(
    private router: Router,
    private authService: AuthService,
    public dialog: MatDialog
  ) {}

  openConfirmDialog(): void {
    const dialogRef = this.dialog.open(RestoreAccountDiaologComponent);
    dialogRef.afterClosed().subscribe((password) => {
      if (password) {
        console.log('password');
        this.authService.restoreAccount(password).subscribe();
        this.logout();
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
