import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchResultsHeaderComponent } from './search-results-header.component';

describe('SearchResultsHeaderComponent', () => {
  let component: SearchResultsHeaderComponent;
  let fixture: ComponentFixture<SearchResultsHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchResultsHeaderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchResultsHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
