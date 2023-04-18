import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SettingGroup } from './models/SettingGroup';
import { Permission } from './models/Permission';
import { CreateSettingRequest } from './models/CreateSettingRequest';
import { UpdateTargetSettingRequest } from './models/UpdateTargetSettingRequest';
import { UpdatePermissionRequest } from './models/UpdatePermissionRequest';
import { PermissionRequestResponse } from './models/PermissionRequestResponse';

@Injectable({
  providedIn: 'root',
})
export class SettingsService {
  // private apiUrl = 'https://appsettingsmanagerbff.azurewebsites.net/settings';
  private apiUrl = 'https://localhost:7160/settings';

  constructor(private http: HttpClient) {}

  getSettingGroup(userId: string, password: string, settingGroupId: string): Observable<SettingGroup> {
    return this.http.get<SettingGroup>(`${this.apiUrl}/userId/${userId}/password/${password}/settingGroupId/${settingGroupId}`);
  }

  getAllSettingGroupsForUser(userId: string, password: string): Observable<SettingGroup[]> {
    return this.http.get<SettingGroup[]>(`${this.apiUrl}/userId/${userId}/password/${password}`);
  }

  createSettingGroup(request: CreateSettingRequest): Observable<SettingGroup> {
    console.log(request);
    return this.http.post<SettingGroup>(this.apiUrl, request);
  }

  updateSetting(request: CreateSettingRequest): Observable<SettingGroup> {
    return this.http.put<SettingGroup>(this.apiUrl, request);
  }

  changeTargetSettingVersion(request: UpdateTargetSettingRequest): Observable<SettingGroup> {
    return this.http.put<SettingGroup>(`${this.apiUrl}/target`, request);
  }

  deleteSettingGroup(userId: string, password: string, settingGroupId: string): Observable<SettingGroup> {
    return this.http.delete<SettingGroup>(`${this.apiUrl}/userId/${userId}/password/${password}/settingGroupId/${settingGroupId}`);
  }

  updatePermission(request: UpdatePermissionRequest): Observable<Permission> {
    return this.http.put<Permission>(`${this.apiUrl}/permission`, request);
  }

  permissionRequestResponse(request: PermissionRequestResponse): Observable<Permission> {
    return this.http.put<Permission>(`${this.apiUrl}/permission/response`, request);
  }
}
