import { Injectable, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api-service.service';


@Injectable({
    providedIn: 'root'
})
export class ChatService implements OnInit{

    constructor(private api: ApiService) {
    }

    ngOnInit(): void {
        
    }
}