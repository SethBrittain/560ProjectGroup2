import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {

  searchTerm: string = '';
  constructor(public router: Router){}

  ngOnInit(): void {
      
  }

  getVal(event:any) {

    this.searchTerm = event.target.value;

  }

  search() {
    this.router.navigate(['/app/search', this.searchTerm]);
    
    setTimeout(()=>{
      window.location.reload();
    }, 1);
  }
}
