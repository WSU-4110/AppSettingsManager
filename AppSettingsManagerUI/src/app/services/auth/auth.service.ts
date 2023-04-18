import { Injectable } from '@angular/core';
import { UserService } from '../bff/users.service';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<string>;
  public currentUser: Observable<string>;
  private currentPasswordSubject: BehaviorSubject<string>;
  public currentPassword: Observable<string>;

  constructor(private userService: UserService) {
    this.currentUserSubject = new BehaviorSubject<string>(localStorage.getItem('currentUser') ?? '');
    this.currentUser = this.currentUserSubject.asObservable();
    this.currentPasswordSubject = new BehaviorSubject<string>(localStorage.getItem('currentPassword') ?? '');
    this.currentPassword = this.currentPasswordSubject.asObservable();
  }

  public get currentUserValue(): string {
    return this.currentUserSubject.value;
  }

  public get currentPasswordValue(): string {
    return this.currentPasswordSubject.value;
  }

  isAuthenticated(): boolean {
    return this.currentUserValue !== '';
  }

  async login(userId: string, password: string): Promise<boolean> {
    try {
      const isAuthenticated = this.userService.authenticateUser(userId, password);
      if (isAuthenticated) {
        localStorage.setItem('currentUser', userId);
        localStorage.setItem('currentPassword', password);
        this.currentUserSubject.next(userId);
        this.currentPasswordSubject.next(password);
        return true;
      } else {
        return false;
      }
    } catch (error) {
      console.error('Login failed:', error);
      return false;
    }
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('currentPassword');
    this.currentUserSubject.next('');
    this.currentPasswordSubject.next('');
  }
}
