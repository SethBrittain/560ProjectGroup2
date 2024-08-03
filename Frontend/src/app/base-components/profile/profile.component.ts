import { Component, OnInit, Input, HostListener } from '@angular/core';
import axios from 'axios';
import { error } from 'console';
import { User } from 'src/util';

@Component({
	selector: 'app-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

	user!: User;
	show: boolean = false;

	@HostListener('document:click', ['$event']) onDocumentClick(event: MouseEvent) {
		this.show=false;
		event.stopPropagation();
	}
	constructor() { }

	ngOnInit(): void {
		axios.post('/api/User')
			.then(res => {
				this.user = res.data;
			})
			.catch(error => {
				if (error.response)
					if (error.response.status == 401 || error.response.status == 403)
						window.location.href = '/login';
			});
	}

	toggleShow(): void {
		this.show = !this.show;
	}

	pfpError(event: any) {
		this.user.profilePhotoUrl = '/static/default-avatar.svg';
	}

	logout() {
		axios.post('/auth/logout')
			.then(() => window.location.href = '/login')
			.catch(console.error);
	}

}
