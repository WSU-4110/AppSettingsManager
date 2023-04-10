import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, lastValueFrom } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import * as models from './models';
import * as requests from './requests/index';

@Injectable({
  providedIn: 'root'
})
export class BffService {
  settingGroups: models.SettingGroup[] = [];
  settings: models.Setting[] = [];
  
  constructor(private http: HttpClient, @Inject('baseURL') private baseURL: string) {}

  getSettingGroupsFromService(): models.SettingGroup[] {
    return this.settingGroups;
  }

  getSettingsFromService(): models.Setting[] {
    return this.settings;
  }

  setSettingsInService(settings: models.Setting[]) {
    this.settings = settings;
  }

  addSettingInService(setting: models.Setting) {
    this.settings.push(setting);
  }

  getSettingGroup(userId: string, password: string, settingGroupId: string): Observable<models.SettingGroup> {
    const url = this.getSettingsUri() + this.getRequestUri(userId, password, settingGroupId);
    return this.http.get<models.SettingGroup>(url).pipe(catchError(this.handleError));
  }

  getAllSettingGroupsForUser(userId: string, password: string): Observable<models.SettingGroup[]> {
    const url = this.getSettingsUri() + this.getRequestUri(userId, password, ``);
    return this.http.get<models.SettingGroup[]>(url).pipe(
      catchError(this.handleError),
      tap((data: models.SettingGroup[]) => {
        this.settingGroups = data;
      })
    );
  }

  async createSettingGroup(userId: string, settingGroupId: string, input: string): Promise<models.SettingGroup> {
    const url = this.getSettingsUri();

    const request = new requests.CreateSettingRequest(userId, settingGroupId, input, ``);

    let newSettingGroup: models.SettingGroup;
    return lastValueFrom(await this.http.post<models.SettingGroup>(url, request));
  }

  async updateSetting(userId: string, password: string, settingGroupId: string, input: string): Promise<models.SettingGroup> {
    const url = this.getSettingsUri();

    const request = new requests.CreateSettingRequest(userId, settingGroupId, input, password);

    return lastValueFrom(await this.http.put<models.SettingGroup>(url, request).pipe(catchError(this.handleError)));
  }

  changeTargetSettingVersion(userId: string, password: string, settingGroupId: string, newVersion: number): Observable<models.SettingGroup> {
    const url = this.getSettingsUri() + "target";

    const request = new requests.UpdateTargetSettingRequest(userId, password, settingGroupId, newVersion);

    return this.http.put<models.SettingGroup>(url, request).pipe(catchError(this.handleError));
  }

  deleteSettingGroup(userId: string, password: string, settingGroupId: string): Observable<models.SettingGroup> {
    const url = this.getSettingsUri() + this.getRequestUri(userId, password, settingGroupId);
    return this.http.delete<models.SettingGroup>(url).pipe(catchError(this.handleError));
  }

  async authenticateUser(userId: string, password: string): Promise<boolean> {
    const url = this.getUsersUri() + "auth/" + this.getRequestUri(userId, password, ``);
    return lastValueFrom(this.http.get<boolean>(url));
  }

  async createUser(userId: string, password: string, email: string): Promise<models.User> {
    const url = this.getUsersUri();

    const request = new requests.CreateUserRequest(userId, password, email);

    return lastValueFrom(this.http.post<models.User>(url, request));
  }

  updateUserPassword(userId: string, oldPassword: string, newPassword: string): Observable<models.User> {
    const url = this.getUsersUri();

    const request = new requests.UpdateUserPasswordRequest(userId, oldPassword, newPassword);

    return this.http.put<models.User>(url, request).pipe(catchError(this.handleError));
  }

  deleteUser(userId: string, password: string): Observable<models.User> {
    const url = this.getUsersUri() + this.getRequestUri(userId, password, ``);
    return this.http.delete<models.User>(url).pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  }

  private getSettingsUri(): string { 
    return `${this.baseURL}/settings/`;
  }
  private getUsersUri(): string {
    return `${this.baseURL}/users/`;
  }
  private getRequestUri(userId: string, password: string, settingGroupId: string): string {
    const url: string = `userId/${userId}/password/${password}`;
    if (settingGroupId){
      return url + `settingGroupId/${settingGroupId}`;
    }
    return url;
  }
}
