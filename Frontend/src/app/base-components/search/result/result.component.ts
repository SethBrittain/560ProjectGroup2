import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/util';

@Component({
	selector: 'app-result',
	templateUrl: './result.component.html',
	styleUrls: ['./result.component.css']
})
export class ResultComponent {
	@Input() title!: string;
	@Input() dateSent!: string;
	@Input() sender!: User;
	@Input() message!: string;

	constructor(public router: Router) { }
}
