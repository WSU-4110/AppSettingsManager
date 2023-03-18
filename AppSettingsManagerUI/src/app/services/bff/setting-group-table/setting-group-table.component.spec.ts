import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SettingGroupTableComponent } from './setting-group-table.component';

describe('SettingGroupTableComponent', () => {
  let component: SettingGroupTableComponent;
  let fixture: ComponentFixture<SettingGroupTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SettingGroupTableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SettingGroupTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
