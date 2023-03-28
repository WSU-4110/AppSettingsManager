import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { BffService } from '../../services/bff/bff.service';
import * as models from '../../services/bff/models';
import { MatDialog } from '@angular/material/dialog';
import { UpdateSettingDialogComponent } from './update-setting-dialog/update-setting-dialog.component';

@Component({
  selector: 'app-view-settings',
  templateUrl: './view-settings.component.html',
  styleUrls: ['./view-settings.component.scss']
})
export class ViewSettingsComponent {
  settings: models.Setting[] = [];
  columnsToDisplay: string[] = ['Id', 'Version', 'Input'];

  constructor(private router: Router, private bffService: BffService, public dialog: MatDialog, private auth: AuthService) {   }

  ngOnInit() {
    if (!this.auth.isAuthenticated()){
      this.router.navigate(['']);
    }

    this.settings = this.bffService.getSettingsFromService();
  };

  openDialog() {
    const dialogRef = this.dialog.open(UpdateSettingDialogComponent, {
      width: '500px',
      data: {}
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      this.router.navigate(['home']);
    });
  }
}
