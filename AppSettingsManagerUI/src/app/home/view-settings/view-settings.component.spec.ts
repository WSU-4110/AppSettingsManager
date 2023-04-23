import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NO_ERRORS_SCHEMA } from '@angular/core';

import { ViewSettingsComponent } from './view-settings.component';

describe('ViewSettingsComponent', () => {
  let component: ViewSettingsComponent;
  let fixture: ComponentFixture<ViewSettingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewSettingsComponent ],
      imports: [ HttpClientTestingModule,
                 RouterTestingModule,
                 MatDialogModule ],
      schemas: [ CUSTOM_ELEMENTS_SCHEMA,
                 NO_ERRORS_SCHEMA ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
