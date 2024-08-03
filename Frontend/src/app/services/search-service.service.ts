import { Injectable } from '@angular/core';
import axios from 'axios';
import { Subject } from 'rxjs';
import { ChannelMessage, DirectMessage, User } from 'src/util';


@Injectable({
    providedIn: 'root'
})
export class SearchService {
	// Search results for direct messages matching the search term
	public directMessageResults: Subject<DirectMessage[]> = new Subject();

	// Search results for direct messages matching the search term
	public channelMessageResults: Subject<ChannelMessage[]> = new Subject();

	// Search results for users matching the search term
	public userResults: Subject<User[]> = new Subject();
	
	// Subject for the search term
	public searchTermSubject: Subject<string> = new Subject();

    // asking for string and channel id. 
    // can we just ask for a substring?
    public search(term: string) : void {
		this.searchTermSubject.next(term);
		this.searchDirectMessages(term);
		this.searchChannelMessages(term);
		this.searchUsers(term);
    }

	// Search for direct messages matching the search term
	private searchDirectMessages(term: string) {
		axios.get('/api/DirectMessage', {
			params: {
				query: term,
				limit: 10
			}})
		.then(response => response.data)
		.then(data => this.directMessageResults.next(data))
		.catch(error => {
			if (error.response)
				if (error.response.status === 401 || error.response.status === 403)
					window.location.href = '/login';
			this.directMessageResults.next([]);
		});
	}

	// Search for channel messages matching the search term
	private searchChannelMessages(term: string) {
		axios.get('/api/ChannelMessage', {
			params: {
				message: term,
				limit: 10
			}})
		.then(response => response.data)
		.then(data => this.channelMessageResults.next(data))
		.catch(error => {
			if (error.response)
				if (error.response.status === 401 || error.response.status === 403)
					window.location.href = '/login';
			this.channelMessageResults.next([]);
		});
	}

	// Search for users matching the search term
	private searchUsers(term: string) {
		axios.get('/api/User', {
			params: {
				query: term,
				limit: 10
			}})
		.then(response => response.data)
		.then(data => this.userResults.next(data))
		.catch(error => {
			if (error.response)
				if (error.response.status === 401 || error.response.status === 403)
					window.location.href = '/login';
			this.userResults.next([]);
		});
	}
}
