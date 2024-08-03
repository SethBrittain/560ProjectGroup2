import { Injectable, OnInit } from '@angular/core';
import axios, { Axios } from 'axios';
import { Observable, Subject } from 'rxjs';
import { Channel, ChannelMessage, DirectMessage, User } from 'src/util';

export enum ChatType {
	Channel,
	Direct
}

@Injectable({
    providedIn: 'root'
})
export class ChatService {
	// Observables
	public message: Subject<ChannelMessage | DirectMessage> = new Subject();
	public currentChat: Subject<Channel | User> = new Subject();
	
	// The type of the chat the user is connected to. 
	// Used to determine which API to call and how to cast message objects
	public chatType: ChatType | undefined = undefined;

	// The page of messages the user is currently on
	private page: number = 0;

	// The chat the user is connected to
	private chat : Channel | User | undefined = undefined;
	
	// The WebSocket connection to the chat
	private connection: WebSocket = new WebSocket("ws://localhost");

	private handler: (m: MessageEvent) => void;
	private wsAddress!: string;

	constructor(){
		let self = this;
		let wsProtocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
		this.wsAddress = `${wsProtocol}//${window.location.host}`;
		
		this.handler = (e: MessageEvent) => {	
			console.log(e);	
			if (e.data)
				if (this.chatType === ChatType.Channel)
					this.message.next(JSON.parse(e.data) as ChannelMessage)
				else if (this.chatType === ChatType.Direct)
					this.message.next(JSON.parse(e.data) as DirectMessage);
				else
					throw new Error('Invalid chat type');
			}
	}

	/**
	 * This function checks what type of chat the user is connected 
	 * to and returns the messages from the next page of the chat.
	 * The result is an array of either ChannelMessage or DirectMessage objects.
	 * Using the chatType, the function determines which API to call and how to 
	 * cast the message objects.
	 * @returns The messages from the next page of the chat
	 */
	getMessages(handler: (message : any)=>void) : ChannelMessage[] | DirectMessage[] {
		var result: ChannelMessage[] | DirectMessage[] = [];
		var uri = 
			this.chatType == ChatType.Channel ? 
			'/api/ChannelMessage' : 
			'/api/DirectMessage';

		var params = this.chatType == ChatType.Channel 
			? {
				channelId: (this.chat as Channel).channelId,
				limit: 20,
				offset: this.page
			} : {
				dmId: (this.chat as User).id,
				limit: 20,
				offset: this.page
			};
		
		axios.get(uri, {
				params: params
			})
			.then(response => response.data)
			.then((data) => {
				if (data && data.length > 0) {
					data.forEach((message: any) => {
						if (this.chatType === ChatType.Channel)
							handler(message);
						else if (this.chatType === ChatType.Direct)
							handler(message);
					});
					this.page++;
				}
			})
			.catch(error => {
				console.log(error.message);
			});
		return result;
	}

	/**
	 * This function deletes a message from the chat the user is connected to.
	 * @param message The message to send to the chat
	 */
	deleteMessage(message: ChannelMessage | DirectMessage) : void {
		var id : number = 
			this.chatType == ChatType.Channel ? 
			(message as ChannelMessage).channelMessageId : 
			(message as DirectMessage).directMessageId;

		var uri = 
			this.chatType == ChatType.Channel ? 
			`/api/ChannelMessage/${id}` : 
			`/api/DirectMessage/${id}`;
		axios.delete(uri)
			.catch(error => console.error(error.message));
	}

	/**
	 * This function sets up the service to communicate 
	 * with the chat. Used for channel messages.
	 * @param chat The chat to handle incoming and 
	 * outgoing messages for
	 */
	selectChannel(chat: Channel) {
		this.disconnectFromChat();

		this.chatType = ChatType.Channel;
		this.chat = chat;
		this.currentChat.next(chat);
		
		this.connection = new WebSocket(
			this.wsAddress + '/chat/channel/' + chat.channelId
		);
		this.connection.onmessage = this.handler;
		this.page = 0;
	}

	/**
	 * This function sets up the service to communicate 
	 * with the chat. Used for direct messages.
	 * @param chat The chat to handle incoming and 
	 * outgoing messages for
	 */
	selectDirect(chat: User) {
		this.disconnectFromChat();
		
		this.chatType = ChatType.Direct;
		this.chat = chat;
		this.currentChat.next(chat);
		console.log(chat.id);
		this.connection = new WebSocket(
			this.wsAddress + '/chat/direct/' + chat.id
		);
		this.connection.onmessage = this.handler;
		this.page = 0;
	}

	editMessage(message: ChannelMessage | DirectMessage, newMessage: string) : Promise<void> {
		var id : number = 
			this.chatType == ChatType.Channel ? 
			(message as ChannelMessage).channelMessageId : 
			(message as DirectMessage).directMessageId;

		var uri = 
			this.chatType == ChatType.Channel ? 
			`/api/ChannelMessage/${id}` : 
			`/api/DirectMessage/${id}`;

		var a = new FormData();
		a.append('message', newMessage);
		return axios.patch(uri, a, {
				headers: {
					'Content-Type': 'multipart/form-data'
				}
			});
	}

	/**
	 * This function disconnects from the current chat.
	 */
	disconnectFromChat() {
		this.connection?.close();
	}
	
	sendMessage(message: string) {
		console.log(this.connection.onmessage);
		if (this.connection) {
			this.connection.send(message);
		}
	}
}