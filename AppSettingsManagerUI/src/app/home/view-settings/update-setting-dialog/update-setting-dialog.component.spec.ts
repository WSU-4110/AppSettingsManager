import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSettingDialogComponent } from './update-setting-dialog.component';

describe('UpdateSettingDialogComponent', () => {
  let component: UpdateSettingDialogComponent;
  let fixture: ComponentFixture<UpdateSettingDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateSettingDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateSettingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
