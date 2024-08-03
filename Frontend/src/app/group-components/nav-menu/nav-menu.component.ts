import { Component, OnInit } from '@angular/core';
import { User, Channel } from 'src/util';

@Component({
	selector: 'app-nav-menu',
	templateUrl: './nav-menu.component.html',
	styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

	orgName: string = 'Kansas State University';

	constructor() { }

	ngOnInit(): void {
	}
}
