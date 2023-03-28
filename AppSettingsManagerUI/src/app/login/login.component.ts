import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { BffService } from '../services/bff/bff.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  signUpForm: FormGroup;
  constructor(private authService: AuthService, private router: Router, private bffService: BffService) {
    this.loginForm = new FormGroup({
      usernameLogin: new FormControl('', [Validators.required, Validators.minLength(4)]),
      passwordLogin: new FormControl('', [Validators.required, Validators.minLength(4)])
    });
    this.signUpForm = new FormGroup({
      username: new FormControl('', [Validators.required, Validators.minLength(4)]),
      password: new FormControl('', [Validators.required, Validators.minLength(4)]),
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
    const loggedIn = await this.authService.login(username, password);
    if (!loggedIn) {
      alert('Invalid username or password');
    }
    else {
       this.router.navigate(['/home'])
    }
  }

  async onSignUp() {
    const username = this.signUpForm.get('username')?.value ?? '';
    const password = this.signUpForm.get('password')?.value ?? '';
    const email = this.signUpForm.get('email')?.value ?? '';
    const user = await this.bffService.createUser(username, password, email);

    alert(`Account with username ${username} added!`);
  }
}
