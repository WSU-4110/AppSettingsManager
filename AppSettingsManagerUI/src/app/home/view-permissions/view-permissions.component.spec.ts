import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewPermissionsComponent } from './view-permissions.component';

describe('ViewPermissionsComponent', () => {
  let component: ViewPermissionsComponent;
  let fixture: ComponentFixture<ViewPermissionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewPermissionsComponent ]
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
