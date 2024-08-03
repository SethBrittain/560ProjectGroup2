import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeProfilePictureModalComponent } from './change-profile-picture-modal.component';

describe('ChangeProfilePictureModalComponent', () => {
  let component: ChangeProfilePictureModalComponent;
  let fixture: ComponentFixture<ChangeProfilePictureModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeProfilePictureModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeProfilePictureModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
