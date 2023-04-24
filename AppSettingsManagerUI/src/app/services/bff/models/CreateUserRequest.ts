export class CreateUserRequest {
    UserId: string;
    Password: string;
    Email: string;

    constructor(userId: string, password: string, email: string){
        this.UserId = userId;
        this.Password = password;
        this.Email = email;
    }
}