import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { MatDialogModule } from '@angular/material/dialog';
import { NO_ERRORS_SCHEMA } from '@angular/core';

import { ViewPermissionsComponent } from './view-permissions.component';

describe('ViewPermissionsComponent', () => {
  let component: ViewPermissionsComponent;
  let fixture: ComponentFixture<ViewPermissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewPermissionsComponent ],
      imports: [ HttpClientTestingModule,
                  RouterTestingModule,
                  MatDialogModule ],
      schemas: [ NO_ERRORS_SCHEMA ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewPermissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
