import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-search-results-header',
  templateUrl: './search-results-header.component.html',
  styleUrls: ['./search-results-header.component.css']
})
export class SearchResultsHeaderComponent implements OnInit{

  @Input() title: string | null= '';

  constructor(){}

  ngOnInit(): void {
      
  }

}
