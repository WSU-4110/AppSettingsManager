import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { SettingsService } from '../services/bff/settings.service';
import * as models from '../services/bff/models';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  settingGroups: models.SettingGroup[] = [];
  columnsToDisplay: string[] = ['Id', 'CreatedBy'];

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
}


