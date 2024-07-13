import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

interface Chat {
    id: Number,
    type: "channel" | "direct"
}

@Injectable({
    providedIn: 'root'
})
export class ChatService {

    private useSSL : boolean = false;
    private host : string = "localhost";
    private port : number = 5000;

    private getChannelURL(channelId : Number) {
        let protocol = this.useSSL ? "wss" : "ws";
        return `${protocol}://${this.host}:${this.port}/chat?type=channel&id=${channelId}`;
    }
    private getDirectURL(directId : number) {
        let protocol = this.useSSL ? "wss" : "ws";
        return `${protocol}://${this.host}:${this.port}/ws/direct?id=${directId}`;
    }

    private ws : WebSocket | null = null;

    constructor() { }

    getChat(chatId : Number, chatType : "channel" | "direct") : Observable<Chat> {
        return of({id: chatId, type: chatType});
    }

    connectChannel(channelId : Number, handler : (message : MessageEvent<any>)=>void) {
        if (this.ws)
            this.ws.close();
        
        this.ws = new WebSocket(this.getChannelURL(channelId));
        this.ws.onmessage = handler;
        this.ws.onerror = (error) => {
            console.log(error);
        }
    }

    connectDirect(directId : number, handler : (message : MessageEvent<any>)=>void) {
        if (this.ws != null) {
            this.ws.send("close");
            this.ws.close();
        }
        this.ws = new WebSocket(this.getDirectURL(directId));
        this.ws.onmessage = handler;
    }

    sendMessage(message : string) {
        if (this.ws == null) return;
        console.log(message.length);
        this.ws.send(message);
    }

    disconnect() {
        if (this.ws == null) return;
        this.ws.close();
    }
}