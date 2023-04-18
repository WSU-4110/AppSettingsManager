import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { SettingsService } from '../../services/bff/settings.service';
import * as models from '../../services/bff/models';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-view-settings',
  templateUrl: './view-settings.component.html',
  styleUrls: ['./view-settings.component.scss']
})
export class ViewSettingsComponent implements OnInit {
  settings: models.Setting[] = [];
  columnsToDisplay: string[] = ['Id', 'Version', 'Input'];

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
}
