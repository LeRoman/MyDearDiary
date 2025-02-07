import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { MaterialModules } from '../../../material.import';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  imports: [MaterialModules, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
})
export class AdminDashboardComponent {
  userEmail: string = '';
  constructor(private router: Router, private authService: AuthService) {}

  sendInvitation() {
    console.log(this.userEmail);
    this.authService.sentInvitation(this.userEmail).subscribe();
    this.userEmail = '';
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
