export class PermissionRequestResponse {
    UserId: string;
    SettingGroupId: string;
    Approved: boolean;
    ApproverId: string;
    Password: string;

    constructor(userId: string, settingGroupId: string, approved: boolean, approverId: string, password: string){
        this.UserId = userId;
        this.SettingGroupId = settingGroupId;
        this.Approved = approved;
        this.ApproverId = approverId;
        this.Password = password;
    }
}