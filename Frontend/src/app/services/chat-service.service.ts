import { Injectable, OnInit } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api-service.service';


@Injectable({
    providedIn: 'root'
})
export class ChatService {
    private ws : WebSocket | null = null;

    constructor() { }

    connectChannel(channelId : number) {
        this.ws = new WebSocket(`ws://localhost:8080/channel/${channelId}`);
        this.ws.onmessage = (e)=>{ console.log("received message"); this.printMessage(e.data); };
        console.log("Connected!");
    }

    connectDirect(directId : number) {
        this.ws = new WebSocket(`ws://localhost:8080/direct/${directId}`);
        this.ws.onmessage = (e)=>{ console.log("received message"); this.printMessage(e.data); };
        console.log("Connected!");
    }

    printMessage(data : MessageEvent) {
        console.log(data);
    }

    sendMessage(message : string) {
        this.ws?.send(JSON.stringify({message}));
    }
}