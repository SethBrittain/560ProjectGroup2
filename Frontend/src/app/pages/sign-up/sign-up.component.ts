import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import axios from 'axios';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
	emailFormControl : FormControl<string | null> = new FormControl<string | null>(null);
	passwordFormControl : FormControl<string | null> = new FormControl<string | null>(null);
	confirmPasswordFormControl : FormControl<string | null> = new FormControl<string | null>(null);
	firstNameFormControl : FormControl<string | null> = new FormControl<string | null>(null);
	lastNameFormControl : FormControl<string | null> = new FormControl<string | null>(null);
	usernameFormControl : FormControl<string | null> = new FormControl<string | null>(null);

	errorText : string = '';

	constructor(private router: Router) { }

	signUp() {
		let data = this.getFormData();

		axios.post('/auth/signup',data)
			.then(()=>{
				this.router.navigate(['/login']);
			})
			.catch(error => {
				if (error.response && error.response.data && error.response.data.errors) {
					var errors = error.response.data.errors;
					if (errors.email)
						this.errorText = errors.email;
					else if (errors.password)
						this.errorText = errors.password;
					else if (errors.confirmPassword)
						this.errorText = errors.confirmPassword;
					else if (errors.firstName)
						this.errorText = errors.firstName;
					else if (errors.lastName)
						this.errorText = errors.lastName;
				} else {
					this.errorText = error.response.data;
				}
			});
	}

	getFormData() : FormData {
		console.log(this.passwordFormControl.value);
		console.log(this.confirmPasswordFormControl.value);
		let data = new FormData();

		if (this.emailFormControl.value && this.emailFormControl.value.length)
			data.append('email', this.emailFormControl.value);
		if (this.passwordFormControl && this.confirmPasswordFormControl && this.passwordFormControl.value && this.confirmPasswordFormControl.value) {
			data.append('password', this.passwordFormControl.value);
			data.append('confirmPassword', this.confirmPasswordFormControl.value);
		}
		if (this.firstNameFormControl.value && this.firstNameFormControl.value.length)
			data.append('firstName', this.firstNameFormControl.value);
		if (this.lastNameFormControl.value && this.lastNameFormControl.value.length)
			data.append('lastName', this.lastNameFormControl.value);

		return data;
	}
}
