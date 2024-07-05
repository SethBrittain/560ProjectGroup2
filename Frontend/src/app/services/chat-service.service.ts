import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class ChatService {

    private useSSL : boolean = false;
    private host : string = "localhost";
    private port : number = 5000;

    private getChannelURL(channelId : number) {
        let protocol = this.useSSL ? "wss" : "ws";
        return `${protocol}://${this.host}:${this.port}/api/Channels/${channelId}/ws`;
    }
    private getDirectURL(directId : number) {
        let protocol = this.useSSL ? "wss" : "ws";
        return `${protocol}://${this.host}:${this.port}/ws/direct?id=${directId}`;
    }

    private ws : WebSocket | null = null;

    constructor() { }

    connectChannel(channelId : number, handler : (message : MessageEvent<any>)=>void) {
        if (this.ws)
            this.ws.close();
        
        this.ws = new WebSocket(this.getChannelURL(channelId));
        
        setInterval(() => {
            if (this.ws)
                if (this.ws.readyState === WebSocket.OPEN) {
                    this.ws.send('');
                }
          }, 30000);
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
}