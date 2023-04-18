export class UpdateTargetSettingRequest {
    SettingGroupId: string;
    NewCurrentVersion: number;
    UserId: string;
    Password: string;

    constructor(userId: string, password: string, settingGroupId: string, newCurrentVersion: number){
        this.UserId = userId;
        this.Password = password;
        this.SettingGroupId = settingGroupId;
        this.NewCurrentVersion = newCurrentVersion;
    }
}