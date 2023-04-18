import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { SettingsService } from 'src/app/services/bff/settings.service';
import { CreateSettingRequest } from 'src/app/services/bff/models/CreateSettingRequest';

@Component({
  selector: 'app-update-setting',
  templateUrl: './update-setting.component.html',
  styleUrls: ['./update-setting.component.scss']
})
export class UpdateSettingComponent implements OnInit {
  input = '';

  constructor(private settingsService: SettingsService, private auth: AuthService, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    if (!this.auth.isAuthenticated()){
      this.router.navigate(['']);
    }

    this.settingsService.getSettingGroup(this.auth.currentUserValue, this.auth.currentPasswordValue, this.route.snapshot.paramMap.get('id') ?? '').subscribe(settingGroup => {
      const setting = settingGroup.settings.filter(setting => setting.isCurrent)[0];
      this.input = JSON.stringify(setting.input, null, 2);
    });
  }

  onBackClick(): void {
    this.router.navigate(['home', this.route.snapshot.paramMap.get('id'), 'settings']);
  }

  async onSaveClick(): Promise<void> {
    const parsedInput = JSON.parse(this.input);

    const request: CreateSettingRequest = {
      SettingGroupId: this.route.snapshot.paramMap.get('id') ?? '',
      Input: parsedInput,
      UserId: this.auth.currentUserValue,
      Password: this.auth.currentPasswordValue
    }

    this.settingsService.updateSetting(request).subscribe(
      (settingGroup) => {
        this.router.navigate(['home', settingGroup.id, 'settings']);
      }
    );
  }
}
