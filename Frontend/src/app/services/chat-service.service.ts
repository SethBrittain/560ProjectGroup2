import { Injectable, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api-service.service';
import { environment } from 'src/environments/environment';


@Injectable({
    providedIn: 'root'
})
export class ChatService {
    public ws : WebSocket | null = null;

    constructor(private auth : AuthService) { 
        console.log("Constructed!");
    }

    connectChannel(channelId : number, handler : (message : MessageEvent<any>)=>void) {
        this.auth.idTokenClaims$.subscribe((keyToken)=>{
            if(keyToken && keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]){
                this.ws = new WebSocket(environment.WebsocketUrl+`/channel/${channelId}/${keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]}`);
                this.ws.onmessage = handler;
                if (this.ws) console.log("Connected!");
            }
        });
    }

    connectDirect(directId : number, handler : (message : MessageEvent<any>)=>void) {
        this.auth.idTokenClaims$.subscribe((keyToken)=>{
            if(keyToken && keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]){
                this.ws = new WebSocket(environment.WebsocketUrl+`ws://localhost:8080/direct/${directId}/${keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]}`);
                this.ws.onmessage = handler;
                if (this.ws) console.log("Connected!");
            }
        });
    }

    sendMessage(message : string) {
        this.ws?.send(JSON.stringify({message}));
    }
}