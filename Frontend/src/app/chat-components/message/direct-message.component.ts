import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { DirectMessage } from 'src/util';
import { Message } from './message';
import { FormControl } from '@angular/forms';
import axios from 'axios';
import { ChatService } from 'src/app/services/chat-service.service';

@Component({
	selector: 'app-direct-message',
	templateUrl: './message.component.html',
	styleUrls: ['./message.component.css']
})
export class DirectMessageComponent extends Message implements OnInit {

	// ViewChilds
	@ViewChild('textArea') textArea!: ElementRef<HTMLTextAreaElement>;

	// Template inputs
	@Input() message!: DirectMessage;
	@Input() isMine: boolean = false;

	// Form Controls
	public textAreaControl: FormControl = new FormControl('');

	constructor(private chatService: ChatService) {
		super();
	}

	ngOnInit(): void {
		this.textAreaControl.setValue(this.message.message);
		this.message.sender.profilePhotoUrl = this.message.sender.profilePhotoUrl || '/static/default-avatar.svg';
	}

	// Handler for when the image fails to load
	onImageError(e: ErrorEvent) {
		if (this.message)
			this.message.sender.profilePhotoUrl = '/static/default-avatar.svg';
	}

	public editVis(value: boolean) {
		this.textVisible = !value;
		this.editVisible = value;

		if (value)
			setTimeout(()=>{
				console.log('test');
				this.textArea.nativeElement.focus();
				this.textArea.nativeElement.selectionStart = this.textAreaControl.value.length;
			},0);
	}

	deleteMessage() {
		this.chatService.deleteMessage(this.message);
	}

	editMessage(event: Event) {
		event.preventDefault();
		var newText : string = this.textAreaControl.value;

		if (newText === this.message.message) {
			this.editVisible = false;
			this.textVisible = true;
			return;
		}
		this.chatService.editMessage(this.message, newText)
			.then(()=>{
				this.message.message = newText;
			})
			.catch(()=>{
				alert('Failed to edit message');
				this.textAreaControl.setValue(this.message.message);
			})
			.finally(()=>{
				this.editVisible = false;
				this.textVisible = true;
			});
	}
}
