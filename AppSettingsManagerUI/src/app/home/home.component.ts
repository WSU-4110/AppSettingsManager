import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { SettingsService } from '../services/bff/settings.service';
import * as models from '../services/bff/models';
import { UpdatePermissionRequest } from '../services/bff/models/UpdatePermissionRequest';
import { PermissionLevel } from '../services/bff/models/PermissionLevel';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  settingGroups: models.SettingGroup[] = [];
  columnsToDisplay: string[] = ['Id', 'CreatedBy'];

  settingGroupId = '';
  requestedPermissionLevel = 0;

  constructor(private settingsService: SettingsService , private router: Router, private auth: AuthService) {}

  ngOnInit() {
    if (this.auth.isAuthenticated())
    {
      this.settingsService.getAllSettingGroupsForUser(this.auth.currentUserValue, this.auth.currentPasswordValue).subscribe(settingGroups => {
        this.settingGroups = settingGroups;
      });
    }
    else {
      this.router.navigate([``]);
    }
  }

  onRowClick(settingGroup: models.SettingGroup): void {
    this.router.navigate(['/home', settingGroup.id, 'settings']);
  }

  onCreateSettingGroup() {
    this.router.navigate(['/home/create']);
  }

  async onRequestNewAccess() {
    const request = new UpdatePermissionRequest(
      this.auth.currentUserValue,
      this.settingGroupId,
      this.requestedPermissionLevel);

    console.log(request);

    this.settingsService.updatePermission(request).subscribe(() => {
      alert('Request sent!');
      this.ngOnInit();
    });
  }
}


