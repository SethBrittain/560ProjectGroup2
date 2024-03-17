import { Injectable, OnInit } from '@angular/core';
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
        this.activate()
    }

    connectDirect(directId : number, handler : (message : MessageEvent<any>)=>void) {
        
    }

    sendMessage(message : string) {
        console.log("sent message");
        this.publish({
            destination: 'ws',
            body: message
        });
    }
}

export const ChatConfig: RxStompConfig = {
    brokerURL: "localhost/ws",
    heartbeatIncoming: 0,
    heartbeatOutgoing: 20000,
    reconnectDelay: 200
}