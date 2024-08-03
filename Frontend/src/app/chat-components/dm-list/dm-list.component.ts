import { Component, Input, OnInit } from '@angular/core';
import axios from 'axios';
import { response } from 'express';
import { ApiService } from 'src/app/services/api-service.service';
import { Channel, User } from 'src/util';

@Component({
	selector: 'app-dm-list',
	templateUrl: './dm-list.component.html',
	styleUrls: ['./dm-list.component.css']
})
export class DmListComponent implements OnInit {
	@Input() select!: ((a: User | Channel)=>void);
	@Input() current!: User | Channel | undefined;
	
	orgId: string = '1'; // SHOULD GET ORG ID
	users: any;

	constructor(private api: ApiService) { }


	ngOnInit(): void {
		this.populateUsers();
	}

	populateUsers(): void {
		axios.get('/api/User?limit=5')
			.then(response => response.data)
			.then((data) => this.users = data)
			.catch(error => {
				if (error.response)
					if (error.response.status === 401 || error.response.status === 403)
						window.location.href = '/login';
			})
	}
}
