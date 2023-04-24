import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { UserService } from '../services/bff/users.service';
import { CreateUserRequest } from '../services/bff/models/CreateUserRequest';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  showSignUp = false;
  loginForm: FormGroup;
  signUpForm: FormGroup;
  constructor(private authService: AuthService, private router: Router, private userService: UserService) {
    this.loginForm = new FormGroup({
      usernameLogin: new FormControl('', [Validators.required]),
      passwordLogin: new FormControl('', [Validators.required])
    });
    this.signUpForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required])
    });
  }

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
       this.router.navigate(['/home']);
    }
  }

  async onSubmit(): Promise<void> {
    const username = this.loginForm.get('usernameLogin')?.value ?? '';
    const password = this.loginForm.get('passwordLogin')?.value ?? '';

    await this.authService.login(username, password);

    const isAuthenticated = this.authService.isAuthenticated();

    if (!isAuthenticated) {
      alert('Invalid username or password');
    }
    else {
       this.router.navigate(['/home'])
    }
  }

  async onSignUp() {
    const userId = this.signUpForm.get('username')?.value ?? '';
    const password = this.signUpForm.get('password')?.value ?? '';
    const email = this.signUpForm.get('email')?.value ?? '';
    const request: CreateUserRequest = {
      UserId: userId,
      Password: password,
      Email: email
    };

    this.userService.createUser(request).subscribe(() => {
      alert(`Account with username ${userId} added!`);
    });

    await this.authService.login(userId, password);

    const isAuthenticated = this.authService.isAuthenticated();

    if (!isAuthenticated) {
      alert('Please try signing in with your new credentials');
    }
    else {
      this.router.navigate(['/home'])
    }
  }
}
