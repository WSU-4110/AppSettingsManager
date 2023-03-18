import { Injectable } from '@angular/core';
import { BffService } from '../bff/bff.service';
import { User } from '../bff/models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = false;
  private user: User = {
    Username: "",
    Password: "",
    Email: ""
  };

  constructor(private bffService: BffService) { }

  login(username: string, password: string): boolean {
    this.bffService.getUser(username)
      .subscribe(user => this.user = user);

    if (this.user.Password == password) {
      this.loggedIn = true;
    }
    else {
      this.loggedIn = false;
    }

    return this.loggedIn;
  }

  logout(): void {
    // Set the user as logged out
    this.loggedIn = false;
  }

  isAuthenticated(): boolean {
    // Return true if the user is logged in, false otherwise
    return this.loggedIn;
  }

}
