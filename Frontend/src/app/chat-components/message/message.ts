import { ChatService } from "src/app/services/chat-service.service";

export interface IMessageFunctions {
	updateMessage(): void;
	deleteMessage(): void;
}

export enum MessageType {
	Channel,
	Direct
}

export class Message {
	// Display
	editVisible: boolean = false;
	buttonVisible: boolean = false;
	textVisible: boolean = true;

	public buttonVis(value: boolean) {
		this.buttonVisible = value;
	}

	public textVis(value: boolean) {
		this.textVisible = value;
	}
}