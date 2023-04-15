import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthService } from '../../../services/auth/auth.service';
import { BffService } from '../../../services/bff/bff.service';
import * as models from '../../../services/bff/models';

@Component({
  selector: 'app-update-setting-dialog',
  templateUrl: './update-setting-dialog.component.html',
  styleUrls: ['./update-setting-dialog.component.scss']
})
export class UpdateSettingDialogComponent {

  @Output() saveClicked: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    public dialogRef: MatDialogRef<UpdateSettingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private bffService: BffService, private auth: AuthService) {}

  onCloseClick(): void {
    this.dialogRef.close();
  }

  async onSaveClick(): Promise<void> {
    const username = this.auth.getUsername();
    const password = this.auth.getPassword();
    const response = await this.bffService.updateSetting(username, this.data.settingGroupId, this.data.input, password);
    this.bffService.getAllSettingGroupsForUser(username, password);
    this.bffService.setSettingsInService(response.settings);
  }

}
