import { Component, Input, OnInit } from '@angular/core';
import axios from 'axios';
import { User, Channel } from 'src/util';

@Component({
	selector: 'app-channel-list',
	templateUrl: './channel-list.component.html',
	styleUrls: ['./channel-list.component.css']
})
export class ChannelListComponent implements OnInit {

	channels: Channel[] = [];

	constructor() {}


	ngOnInit(): void {
		this.GetAllChannelsOfUser();
	}



	GetAllChannelsOfUser() {
		axios.get("/api/Channel?limit=5")
			.then(response => response.data)
			.then((data) => this.channels = data)
			.catch(error => {
				console.error(error);
				if (error.response)
					if (error.response.status === 401 || error.response.status === 403)
						window.location.href = "/login";
			});
	}
}