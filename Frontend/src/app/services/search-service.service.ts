import { Injectable, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api-service.service';


@Injectable({
    providedIn: 'root'
})
export class SearchService implements OnInit{

    channelId: string = '446'; // FOR TESTING ONLY
    private searchSubject = new Subject<string>();
    searchResult$: Observable<any>;

    constructor(private api: ApiService) {
        this.searchResult$ = this.searchSubject.pipe(
            debounceTime(300), // wait 300ms after each keystroke before calling the api
            distinctUntilChanged(), // ignore the same search terms
            switchMap((term: string) => this.search(term)) // cancels previous searches
        )
    }

    ngOnInit(): void {
        
    }

    // asking for string and channel id. 
    // can we just ask for a substring?
    search(term: string): Observable<any> {
        console.log(term + '2');

        let form = new FormData();
        form.append("substring", term);
        form.append("channelId", this.channelId);
        console.log(form);

        this.api.put("/MessagesMatchingSubstring",
        //this.api.get("/MessagesMatchingSubstring?substring="+term+"?channelId="+this.channelId,  
            (response) => {
                console.log(response.data);
                this.searchResult$ = response.data; }, 
            (error) => { console.log(error.message); },
            form
            //{ substring: term, channelId: this.channelId }
        );

        return this.searchResult$; // fix this. This should return the api result
    }

    // triggers the searchResult$ observable and emits the result of the search
    searchFor(term: string): void {
        this.searchSubject.next(term);
        console.log(term)
    }
}
