import { Component, OnInit } from '@angular/core';
import { SearchService } from 'src/app/services/search-service.service'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ApiService } from 'src/app/services/api-service.service';



@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  //searchResults = this.searchService.searchResult$; // gets the results from the service
  searchResults: any;
  title: string | null = ''; // this title shown in the header
  constructor(private searchService: SearchService, private route: ActivatedRoute, private router: Router, private api: ApiService) { }



  ngOnInit(): void {
    console.error();
    let term = this.route.snapshot.paramMap.get('terms');
    this.title = term;



    this.searchService.searchResult$.subscribe((results: any) => {
      this.searchResults = results;
      console.log(results);

    });

    // this.searchResults = this.searchService.searchResult$; 

    console.log(this.searchResults);
    this.search(term);



  }

  search(term: any) {
    console.log(term + '2');

    let form = new FormData();
    form.append("subString", term);
    console.log(form);

    this.api.post("/SearchUserMessages",
      (response) => {
        // console.log("below is the response data");
        // console.log(response.data);
        this.searchResults = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }


  







}
