import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSettingGroupComponent } from './create-setting-group.component';

describe('CreateSettingGroupComponent', () => {
  let component: CreateSettingGroupComponent;
  let fixture: ComponentFixture<CreateSettingGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateSettingGroupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateSettingGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
