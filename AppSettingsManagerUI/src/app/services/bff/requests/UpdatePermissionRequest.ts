import { PermissionLevel } from '../models/PermissionLevel';

export class UpdatePermissionRequest {
    UserId: string;
    SettingGroupId: string;
    NewPermissionLevel: PermissionLevel;

    constructor(userId: string, settingGroupId: string, newPermissionLevel: PermissionLevel){
        this.UserId = userId;
        this.SettingGroupId = settingGroupId;
        this.NewPermissionLevel = newPermissionLevel;
    }
}