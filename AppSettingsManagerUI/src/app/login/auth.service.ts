import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loggedIn = false;

  constructor() { }

  login(username: string, password: string): boolean {
    // Check if the username and password are valid
    if (username === 'example' && password === 'password') {
      // Set the user as logged in
      this.loggedIn = true;
      return true;
    }
    return false;
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
