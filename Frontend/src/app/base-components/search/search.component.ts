import { Component } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  
  searchTerm: string = '';

  constructor(){}

  search(val:string) {

    console.warn(val);
    this.searchTerm = val;

    setTimeout(()=>{
      window.location.reload();
    }, 1);
  }
}
