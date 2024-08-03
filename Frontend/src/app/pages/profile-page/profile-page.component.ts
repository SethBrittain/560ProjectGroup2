import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import axios from 'axios';
import { ConfirmInputComponent } from 'src/app/base-components/confirm-input/confirm-input.component';
import { User } from 'src/util';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent implements OnInit {

	@ViewChild('pfpForm') pfpForm!: ElementRef<HTMLFormElement>;

	defaultPfp: string = '/static/default-avatar.svg';
	user!: User;
	firstNameFormControl: FormControl = new FormControl('');
	lastNameFormControl: FormControl = new FormControl('');
	emailFormControl: FormControl = new FormControl('');
	titleFormControl: FormControl = new FormControl('');

	imageSrc: string | null = null;
	showPfp: boolean = false;

	constructor(private router: Router) { }

	ngOnInit(): void {
		axios.post('/api/User')
			.then((res) => { 
				this.user = res.data;
				this.firstNameFormControl.setValue(this.user.firstName);
				this.lastNameFormControl.setValue(this.user.lastName);
				this.emailFormControl.setValue(this.user.email);
				this.titleFormControl.setValue(this.user.title);
				this.user.profilePhotoUrl = this.user.profilePhotoUrl || this.defaultPfp;
			})
			.catch((error)=>{
				if (error.response)
					if (error.response.status == 401 || error.response.status == 403)
						window.location.href = '/login';
			});
	}

	uploadedFile(event: any): void {
		console.log("uploaded file");
		var reader = new FileReader();
		reader.onload = (e) => this.imageSrc = reader.result as string;
		reader.readAsDataURL(event.target.files[0]);
	}

	updateProfilePicture(): void {
		this.showPfp = !this.showPfp;
		console.log(this.showPfp);
	}

	profilePictureError(event: any): void {
		console.log(event.target);
		event.target.src = this.defaultPfp;
	}

	uploadProfilePicture(): void {
		let data = new FormData(this.pfpForm.nativeElement);
		axios.post('/upload/profile', data)
			.then(()=>{
				this.showPfp = false;
			})
			.catch(console.error);
	}

	updateUser() {
		this.user.firstName = this.firstNameFormControl.value;
		this.user.lastName = this.lastNameFormControl.value;
		this.user.email = this.emailFormControl.value;
		this.user.title = this.titleFormControl.value;

		axios.patch('/api/User', this.user)
			.then(()=>{
				this.router.navigate(['/app']);
			})
			.catch((error) => {
				if (error.response)
					if (error.response.status == 401 || error.response.status == 403)
						window.location.href = '/login';
			});
	}
}
