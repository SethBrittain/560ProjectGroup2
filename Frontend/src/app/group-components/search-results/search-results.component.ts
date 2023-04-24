import { Component, OnInit } from '@angular/core';
import { SearchService } from 'src/app/services/search-service.service'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  //searchResults = this.searchService.searchResult$; // gets the results from the service
  searchResults: any;
  title: string | null = ''; // this title shown in the header
  constructor(private searchService: SearchService, private route:ActivatedRoute, private router: Router){}

  ngOnInit(): void {
    let term = this.route.snapshot.paramMap.get('terms');
    this.title = term;

    this.searchService.searchResult$.subscribe((results: string[][]) => {
      this.searchResults = results;
      console.log(this.searchResults);
    });
  }

}
