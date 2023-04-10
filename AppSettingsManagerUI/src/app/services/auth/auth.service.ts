import { Injectable } from '@angular/core';
import { BffService } from '../bff/bff.service';
import { User } from '../bff/models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = false;

  private username: string = ``;
  private password: string = ``;

  constructor(private bffService: BffService) { }

  async login(username: string, password: string): Promise<boolean> {
    this.loggedIn = await this.bffService.authenticateUser(username, password);
    this.username = username;
    this.password = password;
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

  getUsername(): string {
    return this.username;
  }

  getPassword(): string {
    return this.password;
  }
}
