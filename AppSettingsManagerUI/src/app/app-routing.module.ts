import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { ViewSettingsComponent } from './home/view-settings/view-settings.component';
import { CreateSettingGroupComponent } from './home/create-setting-group/create-setting-group.component';
import { UpdateSettingComponent } from './home/view-settings/update-setting/update-setting.component';

const routes: Routes = [
  {path: '', component: LoginComponent},
  {path: 'home', component: HomeComponent},
  {path: 'home/create', component: CreateSettingGroupComponent},
  {path: 'home/:id/settings', component: ViewSettingsComponent},
  {path: 'home/:id/settings/update', component: UpdateSettingComponent},
  { path: '**', redirectTo: 'home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
