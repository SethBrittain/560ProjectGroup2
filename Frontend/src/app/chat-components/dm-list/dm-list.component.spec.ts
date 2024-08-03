import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DmListComponent } from './dm-list.component';

describe('DmListComponent', () => {
  let component: DmListComponent;
  let fixture: ComponentFixture<DmListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DmListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DmListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
