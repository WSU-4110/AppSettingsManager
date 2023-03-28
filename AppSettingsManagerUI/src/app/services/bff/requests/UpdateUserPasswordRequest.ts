export class UpdateUserPasswordRequest{
    UserId: string;
    OldPassword: string;
    NewPassword: string;

    constructor(userId: string, oldPassword: string, newPassword: string){
        this.UserId = userId;
        this.OldPassword = oldPassword;
        this.NewPassword = newPassword;
    }
}