export class UpdatePermissionRequest {
    UserId: string;
    SettingGroupId: string;
    NewPermissionLevel: number;

    constructor(userId: string, settingGroupId: string, newPermissionLevel: number){
        this.UserId = userId;
        this.SettingGroupId = settingGroupId;
        this.NewPermissionLevel = newPermissionLevel;
    }
}
