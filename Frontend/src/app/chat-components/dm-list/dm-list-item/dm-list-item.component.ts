import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ChatService, ChatType } from 'src/app/services/chat-service.service';
import { Channel, User } from 'src/util';

@Component({
	selector: 'app-dm-list-item',
	templateUrl: './dm-list-item.component.html',
	styleUrls: ['./dm-list-item.component.css']
})
export class DmListItemComponent implements OnInit {
	
	// Display values
	@Input() user!: User;
	active: boolean = false;
	
	constructor(private chatService: ChatService) { }
	
	ngOnInit(): void {
		this.chatService.currentChat.subscribe(value => {
			if (value && this.user)
				this.active = this.user === value;
		});
	}

	selectUser() {
		this.chatService.selectDirect(this.user);
	}
}
