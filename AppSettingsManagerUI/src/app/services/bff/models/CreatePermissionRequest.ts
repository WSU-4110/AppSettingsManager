import { PermissionLevel } from '../models/PermissionLevel';

export class CreatePermissionRequest {
  UserId: string;
  SettingGroupId: string;
  StartingPermissionLevel: PermissionLevel;

  constructor(userId: string, settingGroupId: string, permissionLevel: PermissionLevel){
    this.UserId = userId;
    this.SettingGroupId = settingGroupId;
    this.StartingPermissionLevel = permissionLevel;
  }
}
