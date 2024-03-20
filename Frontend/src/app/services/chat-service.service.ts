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
        return `${protocol}://${this.host}:${this.port}/ws/channel?id=${channelId}`;
    }
    private getDirectURL(directId : number) {
        let protocol = this.useSSL ? "wss" : "ws";
        return `${protocol}://${this.host}:${this.port}/ws/direct?id=${directId}`;
    }

    private ws : WebSocket | null = null;

    constructor() { }

    connectChannel(channelId : number, handler : (message : MessageEvent<any>)=>void) {
        if (this.ws != null) {
            this.ws.close();
        }
        this.ws = new WebSocket(this.getChannelURL(channelId));
        this.ws.onmessage = handler;
    }

    connectDirect(directId : number, handler : (message : MessageEvent<any>)=>void) {
        if (this.ws != null) {
            this.ws.close();
        }
        this.ws = new WebSocket(this.getDirectURL(directId));
        this.ws.onmessage = handler;
    }

    sendMessage(message : string) {
        if (this.ws == null) return;
        this.ws.send(message);
    }
}