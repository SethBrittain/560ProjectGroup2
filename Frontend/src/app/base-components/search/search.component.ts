import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from 'src/app/services/search-service.service'

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  searchTerm: string = '';
  constructor(public router: Router, private searchService: SearchService){}

  ngOnInit(): void {
      
  }

  getVal(event:any) {

    this.searchTerm = event.target.value;

  }

  search(): void {
    this.router.navigate(['/app/search', this.searchTerm]);
    this.searchService.searchFor(this.searchTerm);

    //setTimeout(()=>{
      //window.location.reload();
    //}, 1);
  }
}
