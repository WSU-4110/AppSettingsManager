export class CreateSettingRequest {
    SettingGroupId: string;
    Input: string;
    UserId: string;
    Password: string;

    constructor(userId: string, settingGroupId: string, input: string, password: string){
        this.UserId = userId;
        this.SettingGroupId = settingGroupId;
        this.Input = input;
        this.Password = password;
    }
}
