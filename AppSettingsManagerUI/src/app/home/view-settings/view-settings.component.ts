import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { SettingsService } from '../../services/bff/settings.service';
import * as models from '../../services/bff/models';
import { MatDialog } from '@angular/material/dialog';
import { UpdateTargetSettingRequest } from 'src/app/services/bff/models/UpdateTargetSettingRequest';

@Component({
  selector: 'app-view-settings',
  templateUrl: './view-settings.component.html',
  styleUrls: ['./view-settings.component.scss']
})
export class ViewSettingsComponent implements OnInit {
  settings: models.Setting[] = [];
  columnsToDisplay: string[] = ['Id', 'Version', 'Input', 'IsCurrent', 'CreatedBy'];
  newTargetVersion = 0;

  constructor(private route: ActivatedRoute, private router: Router, private settingsService: SettingsService, public dialog: MatDialog, private auth: AuthService) {   }

  ngOnInit() {
    if (!this.auth.isAuthenticated()){
      this.router.navigate(['']);
    }

    const settingGroupId = this.route.snapshot.paramMap.get('id') ?? '';
    this.settingsService.getSettingGroup(this.auth.currentUserValue, this.auth.currentPasswordValue, settingGroupId).subscribe(settingGroup => {
      this.settings = settingGroup.settings;
    });
  }

  onUpdateSetting() {
    this.router.navigate(['/home', this.route.snapshot.paramMap.get('id'), 'settings', 'update']);
  }

  onClickPermissions() {
    this.router.navigate(['/home', this.route.snapshot.paramMap.get('id'), 'permissions']);
  }

  async onUpdateTargetSetting() {
    const request = new UpdateTargetSettingRequest(this.auth.currentUserValue, this.auth.currentPasswordValue, this.route.snapshot.paramMap.get('id') ?? '', this.newTargetVersion);

    this.settingsService.changeTargetSettingVersion(request).subscribe(() => {
      this.ngOnInit();
    });
  }

  onClickBack() {
    this.router.navigate(['/home']);
  }

  async onDeleteSettingGroup() {
    const settingGroupId = this.route.snapshot.paramMap.get('id') ?? '';
    this.settingsService.deleteSettingGroup(this.auth.currentUserValue, this.auth.currentPasswordValue, settingGroupId).subscribe(() => {
      this.router.navigate(['/home']);
    });
  }
}
