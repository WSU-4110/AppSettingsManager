import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { SettingsService } from '../../services/bff/settings.service';
import * as models from '../../services/bff/models';
import { MatDialog } from '@angular/material/dialog';
import { PermissionRequestResponse } from 'src/app/services/bff/models/PermissionRequestResponse';

@Component({
  selector: 'app-view-permissions',
  templateUrl: './view-permissions.component.html',
  styleUrls: ['./view-permissions.component.scss']
})
export class ViewPermissionsComponent implements OnInit {
  permissions: models.Permission[] = [];
  columnsToDisplay: string[] = ['UserId', 'CurrentPermissionLevel', 'RequestedPermissionLevel', 'WaitingForApproval', 'ApprovedBy'];

  responseUserId = '';

  constructor(private route: ActivatedRoute, private router: Router, private settingsService: SettingsService, public dialog: MatDialog, private auth: AuthService) {   }

  ngOnInit() {
    if (!this.auth.isAuthenticated()){
      this.router.navigate(['']);
    }

    const settingGroupId = this.route.snapshot.paramMap.get('id') ?? '';
    this.settingsService.getSettingGroup(this.auth.currentUserValue, this.auth.currentPasswordValue, settingGroupId).subscribe(settingGroup => {
      this.permissions = settingGroup.permissions;
    });
  }

  async onClickApprove() {
    const request = new PermissionRequestResponse(this.responseUserId, this.route.snapshot.paramMap.get('id') ?? '', true, this.auth.currentUserValue, this.auth.currentPasswordValue);

    this.settingsService.permissionRequestResponse(request).subscribe(() => {
      this.ngOnInit();
    });
  }

  async onClickDeny() {
    const request = new PermissionRequestResponse(this.responseUserId, this.route.snapshot.paramMap.get('id') ?? '', false, this.auth.currentUserValue, this.auth.currentPasswordValue);

    this.settingsService.permissionRequestResponse(request).subscribe(() => {
      this.ngOnInit();
    });
  }

  onClickBack() {
    this.router.navigate(['/home', this.route.snapshot.paramMap.get('id'), 'settings']);
  }
}
