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
  userResults : any;
  title: string | null = ''; // this title shown in the header
  constructor(private searchService: SearchService, private route: ActivatedRoute, private router: Router, private api: ApiService) { }



  ngOnInit(): void {
    let term = this.route.snapshot.paramMap.get('terms');
    this.title = term;

    this.searchService.searchResult$.subscribe((results: any) => {
      this.searchResults = results;
    });
    this.search(term);
<<<<<<< HEAD
=======
    this.searchUsers(term,1);



>>>>>>> master
  }

  search(term: any) {
    let form = new FormData();
    form.append("subString", term);

    this.api.post("/SearchUserMessages",
      (response) => {
        this.searchResults = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  
  searchUsers(term: any, organizationId : any) {
    console.log(term + '2');

    let form = new FormData();
    form.append("subString", term);
    form.append("organizationId", organizationId);
    console.log(form);

    this.api.post("/SearchUsersInOrganization",
      (response) => {
        // console.log("below is the response data");
        // console.log(response.data);
        this.userResults = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  










}
