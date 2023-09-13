import { Injectable, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { environment } from 'src/environments/environment';
import { RxStomp, RxStompConfig } from '@stomp/rx-stomp';

@Injectable({
    providedIn: 'root'
})
export class ChatService extends RxStomp {

    constructor() { 
        super();
        console.log("connecting to ws");
    }

    connectChannel(channelId : number, handler : (message : MessageEvent<any>)=>void) {
        
    }

    connectDirect(directId : number, handler : (message : MessageEvent<any>)=>void) {
        
    }

    sendMessage(message : string) {
        console.log("sent message");
        this.publish({
            destination: '',
            body: message
        });
    }
}

export const ChatConfig: RxStompConfig = {
    brokerURL: environment.WebsocketUrl,
    heartbeatIncoming: 0,
    heartbeatOutgoing: 20000,
    reconnectDelay: 200
}