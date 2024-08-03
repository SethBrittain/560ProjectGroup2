import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ApiService } from 'src/app/services/api-service.service';
import { ChatService } from 'src/app/services/chat-service.service';

@Component({
  selector: 'app-message-input',
  templateUrl: './message-input.component.html',
  styleUrls: ['./message-input.component.css']
})
export class MessageInputComponent {

	public messageControl: FormControl = new FormControl();

	constructor(private chatService: ChatService) { console.log('test');}

	sendMessage() {
		if (this.messageControl.value.length > 0) {
			this.chatService.sendMessage(this.messageControl.value);
			this.messageControl.setValue('');
		}
	}
}