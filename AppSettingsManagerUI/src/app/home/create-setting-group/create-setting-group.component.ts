import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthService } from '../../services/auth/auth.service';
import { SettingsService } from 'src/app/services/bff/settings.service';
import { CreateSettingRequest } from 'src/app/services/bff/models/CreateSettingRequest';

@Component({
  selector: 'app-create-setting-group',
  templateUrl: './create-setting-group.component.html',
  styleUrls: ['./create-setting-group.component.scss']
})
export class CreateSettingGroupComponent {
  constructor(
    public dialogRef: MatDialogRef<CreateSettingGroupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private settingsService: SettingsService, private auth: AuthService, private router: Router) {}

  onBackClick(): void {
    this.router.navigate(['home']);
  }

  async onSaveClick(): Promise<void> {
    const request: CreateSettingRequest = {
      SettingGroupId: this.data.settingGroupId,
      Input: this.data.input,
      UserId: this.auth.currentUserValue,
      Password: this.auth.currentPasswordValue
    };
    this.settingsService.createSettingGroup(request);
    this.router.navigate(['home']);
  }

}
