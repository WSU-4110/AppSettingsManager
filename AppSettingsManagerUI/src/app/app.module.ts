import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { BffService } from './services/bff/bff.service';
import { CreateSettingGroupDialogComponent } from './home/create-setting-group-dialog/create-setting-group-dialog.component';
import { ViewSettingsComponent } from './home/view-settings/view-settings.component';
import { UpdateSettingDialogComponent } from './home/view-settings/update-setting-dialog/update-setting-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    CreateSettingGroupDialogComponent,
    ViewSettingsComponent,
    UpdateSettingDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatDialogModule
  ],
  providers: [
    BffService,
    { provide: 'baseURL', useValue: 'https://appsettingsmanagerbff.azurewebsites.net/'} //changed from local
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
