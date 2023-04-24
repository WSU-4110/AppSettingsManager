import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from './models/User';
import { CreateUserRequest } from './models/CreateUserRequest';
import { UpdateUserPasswordRequest } from './models/UpdateUserPasswordRequest';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private apiUrl = 'https://appsettingsmanagerbff.azurewebsites.net/users';

  constructor(private http: HttpClient) {}

  authenticateUser(userId: string, password: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/auth/userId/${userId}/password/${password}`);
  }

  createUser(request: CreateUserRequest): Observable<User> {
    return this.http.post<User>(this.apiUrl, request);
  }

  updateUserPassword(request: UpdateUserPasswordRequest): Observable<User> {
    return this.http.put<User>(this.apiUrl, request);
  }

  deleteUser(userId: string, password: string): Observable<User> {
    return this.http.delete<User>(`${this.apiUrl}/userId/${userId}/password/${password}`);
  }
}
