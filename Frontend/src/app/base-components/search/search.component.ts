import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { SearchService } from 'src/app/services/search-service.service'

@Component({
	selector: 'app-search',
	templateUrl: './search.component.html',
	styleUrls: ['./search.component.css']
})
export class SearchComponent {
	searchField: FormControl = new FormControl();
	constructor(private router: Router, private searchService: SearchService) { }

	search(): void {
		this.searchService.search(this.searchField.value);
		this.router.navigate(['app', 'search', this.searchField.value]);
	}
}
