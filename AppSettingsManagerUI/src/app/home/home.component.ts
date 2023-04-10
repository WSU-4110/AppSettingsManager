import { Component, OnInit } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../services/auth/auth.service';
import { BffService } from '../services/bff/bff.service';
import * as models from '../services/bff/models';
import { MatDialog } from '@angular/material/dialog';
import { CreateSettingGroupDialogComponent } from './create-setting-group-dialog/create-setting-group-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  settingGroups: models.SettingGroup[] = [];
  columnsToDisplay: string[] = ['Id', 'CreatedBy', 'LastUpdatedAt'];

  constructor(private bffService: BffService, private router: Router, private auth: AuthService, 
    public dialog: MatDialog) {}

  ngOnInit() {
    if (this.auth.isAuthenticated())
    {
      const data = this.bffService.getAllSettingGroupsForUser(this.auth.getUsername(), this.auth.getPassword());
      data.subscribe((data: models.SettingGroup[]) => this.settingGroups = data);
    }
    else {
      this.router.navigate([``]);
    }
  }

  onRowClick(settingGroup: models.SettingGroup) {
    this.bffService.setSettingsInService(settingGroup.settings);
    
    this.router.navigate([`settings`]);
  }

  openDialog() {
    const dialogRef = this.dialog.open(CreateSettingGroupDialogComponent, {
      width: '500px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.router.navigate(['']);
    });
  }
}


