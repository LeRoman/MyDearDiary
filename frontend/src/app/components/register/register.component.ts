import { Component, OnInit } from '@angular/core';
import { UserRegisterDto } from '../../../models/Auth/user-register-dto';
import { AuthService } from '../../../services/auth.service';
import { MaterialModules } from '../../../material.import';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackBarService } from '../../../services/snack-bar.service';

@Component({
  selector: 'app-register',
  imports: [MaterialModules, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  public newUser: UserRegisterDto = new UserRegisterDto();
  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBarService: SnackBarService
  ) {}
  ngOnInit(): void {
    this.newUser.token = this.route.snapshot.queryParamMap.get('token');
  }

  register(): void {
    this.authService.register(this.newUser).subscribe({
      next: (response) => {
        this.snackBarService.showUsualMessage(response.message);
        this.router.navigate(['']);
      },
      error: (err) => {
        const errorMessage =
          err.error?.error || 'An unexpected error occurred.';
        console.error('Login failed:', err);
        this.snackBarService.showUsualMessage(errorMessage);
        //this.clearForm();
      },
    });
  }
}
