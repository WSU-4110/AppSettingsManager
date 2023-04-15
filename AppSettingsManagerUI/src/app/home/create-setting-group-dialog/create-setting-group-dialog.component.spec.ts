import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSettingGroupDialogComponent } from './create-setting-group-dialog.component';

describe('CreateSettingGroupDialogComponent', () => {
  let component: CreateSettingGroupDialogComponent;
  let fixture: ComponentFixture<CreateSettingGroupDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateSettingGroupDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSettingGroupDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
