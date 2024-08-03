import { Component, OnInit } from '@angular/core';
import { SearchService } from 'src/app/services/search-service.service'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ApiService } from 'src/app/services/api-service.service';
import axios from 'axios';
import { User, ChannelMessage, DirectMessage } from 'src/util';

@Component({
	selector: 'app-search-results',
	templateUrl: './search-results.component.html',
	styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {
	// Search results for direct messages matching the search term
	directMessageResults: DirectMessage[] = [];

	// Search results for direct messages matching the search term
	channelMessageResults: ChannelMessage[] = [];

	// Search results for users matching the search term
	userResults: User[] = [];

	title: string | null = ''; // this title shown in the header
	constructor(private route: ActivatedRoute, private searchService: SearchService) { }

	ngOnInit(): void {
		this.searchService.searchTermSubject.subscribe(term => this.title = term);
		this.searchService.channelMessageResults.subscribe(
			(results: ChannelMessage[]) => this.channelMessageResults = results
		);
		this.searchService.directMessageResults.subscribe(
			(results: DirectMessage[]) => this.directMessageResults = results
		);
		this.searchService.userResults.subscribe(
			(results: User[]) => this.userResults = results
		);
	}


}