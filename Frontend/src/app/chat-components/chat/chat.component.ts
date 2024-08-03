import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ChatService, ChatType } from 'src/app/services/chat-service.service';
import { Channel, ChannelMessage, DirectMessage, User } from 'src/util';
import axios from 'axios';

@Component({
	selector: 'app-chat',
	templateUrl: './chat.component.html',
	styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
	@ViewChild('scrollMe') scrollContainer!: ElementRef;
	
	public title: string = '';
	public messages: any[] = [];
	public type: ChatType | undefined = undefined;

	public currentUser!: User;

	constructor(private chatService: ChatService) { }

	getCurrentUser() : void {
		axios.post('/api/User')
			.then(response => response.data)
			.then((data) => this.currentUser = data)
			.catch(error => {
				if (error.response)
					if (error.response.status === 401 || error.response.status === 403)
						window.location.href = '/login';
			})
	}

	ngOnInit(): void {
		this.chatService.currentChat.subscribe((chat: Channel | User) => {
			this.getCurrentUser();
			this.messages = [];
			this.chatService.getMessages((message: any)=>{
				this.messages.push(message);
			});
			setTimeout(() => {
				document.documentElement.scrollTop = document.documentElement.scrollHeight;
			}, 100);
			if (this.chatService.chatType == ChatType.Channel) {
				this.title = (chat as Channel).name;
			} else if (this.chatService.chatType == ChatType.Direct) {
				var u = chat as User;
				this.title = `${u.firstName} ${u.lastName}`;
			}
		});
		this.chatService.message.subscribe((message: ChannelMessage | DirectMessage) => {
			this.messages.forEach((m) => {
				if (this.chatService.chatType == ChatType.Channel) {
					if (m.channelMessageId === (message as ChannelMessage).channelMessageId) {
						m = message;
					}
				} else if (this.chatService.chatType == ChatType.Direct) {
					if (m.directMessageId === (message as DirectMessage).directMessageId) 
						m = message;
				}
			})
			this.messages.push(message);
			setTimeout(() => {
			document.documentElement.scrollTop = document.documentElement.scrollHeight;
			}, 10);
		});
	}
}