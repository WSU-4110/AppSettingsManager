import { Component, Inject, EventEmitter, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AuthService } from '../../services/auth/auth.service';
import { BffService } from '../../services/bff/bff.service';
import * as models from '../../services/bff/models';

@Component({
  selector: 'app-create-setting-group-dialog',
  templateUrl: './create-setting-group-dialog.component.html',
  styleUrls: ['./create-setting-group-dialog.component.scss']
})
export class CreateSettingGroupDialogComponent {

  @Output() saveClicked: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    public dialogRef: MatDialogRef<CreateSettingGroupDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private bffService: BffService, private auth: AuthService) {}

  onCloseClick(): void {
    this.dialogRef.close();
  }

  async onSaveClick(): Promise<void> {
    const username = this.auth.getUsername();
    const response = await this.bffService.createSettingGroup(username, this.data.settingGroupId, this.data.input);
    this.saveClicked.emit(response);
  }

}
