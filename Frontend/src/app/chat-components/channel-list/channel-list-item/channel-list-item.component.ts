import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Channel, User } from 'src/util';
import { ChatService } from 'src/app/services/chat-service.service';

@Component({
	selector: 'app-channel-list-item',
	templateUrl: './channel-list-item.component.html',
	styleUrls: ['./channel-list-item.component.css']
})
export class ChannelListItemComponent {

	// Display values
	@Input() channel!: Channel;
	active: boolean = false;

	constructor(private chatService: ChatService) { }

	ngOnInit(): void {
		this.chatService.currentChat.subscribe(value => {
			if (value && this.channel)
				this.active = this.channel === value;
		});
	}

	selectChannel() {
		this.chatService.selectChannel(this.channel);
	}
}
