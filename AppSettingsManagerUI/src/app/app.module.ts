import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { BffService } from './services/bff/bff.service';
import { SettingGroupTableComponent } from './services/bff/setting-group-table/setting-group-table.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SettingGroupTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    BffService,
    { provide: 'baseURL', useValue: 'https://localhost:5202'}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
