import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { SettingsService } from 'src/app/services/bff/settings.service';
import { CreateSettingRequest } from 'src/app/services/bff/models/CreateSettingRequest';

@Component({
  selector: 'app-create-setting-group',
  templateUrl: './create-setting-group.component.html',
  styleUrls: ['./create-setting-group.component.scss']
})
export class CreateSettingGroupComponent implements OnInit {
  input = '';
  settingGroupId = '';

  constructor(
    private settingsService: SettingsService, private auth: AuthService, private router: Router) {}

  ngOnInit(): void {
    if (!this.auth.isAuthenticated()){
      this.router.navigate(['']);
    }

    this.input = JSON.stringify(
      {
        key1: 'value1',
        key2: 'value2',
        key3: 'value3',
      },
      null,
      2
    );
  }

  onBackClick(): void {
    this.router.navigate(['home']);
  }

  async onSaveClick(): Promise<void> {
    const parsedInput = JSON.parse(this.input);

    const request = new CreateSettingRequest(this.auth.currentUserValue, this.settingGroupId, parsedInput, this.auth.currentPasswordValue);

    this.settingsService.createSettingGroup(request).subscribe(() => this.router.navigate(['home']));
  }

}
