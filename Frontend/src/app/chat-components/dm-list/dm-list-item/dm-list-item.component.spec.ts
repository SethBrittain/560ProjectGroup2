import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DmListItemComponent } from './dm-list-item.component';

describe('DmListItemComponent', () => {
  let component: DmListItemComponent;
  let fixture: ComponentFixture<DmListItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DmListItemComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DmListItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
