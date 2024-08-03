import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import axios from 'axios';
import { response } from 'express';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
	selector: 'app-log-in',
	templateUrl: './log-in.component.html',
	styleUrls: ['./log-in.component.css']
})

export class LogInComponent {

	emailControl: FormControl = new FormControl('');
	passwordControl: FormControl = new FormControl('');

	constructor(private router: Router) { }

	login() {
		let form = new FormData();
		form.append('email', this.emailControl.value);
		form.append('password', this.passwordControl.value);
		axios.post('/auth/login', form)
			.then(response => {
				if (response && response.status === 200)
					this.router.navigate(['/app']);
			})
			.catch(error => {
				if (error && error.response && error.response.status === 403) {
					this.passwordControl.setValue('');
					alert('Invalid email or password');
				}
			})
	}

	loginWithCAS() {

	}
}
