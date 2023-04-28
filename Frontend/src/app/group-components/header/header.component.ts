import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{

  userId: string = '3'; // THIS IS HARD CODED
  profilePhoto: string = '';
  firstName: string = '';
  lastName: string = '';

  constructor(private api: ApiService, private auth: AuthService) {}

  ngOnInit(): void {
      //this.getUserInfo();
      this.auth.user$.subscribe((user) => {
        this.profilePhoto = user?.picture ?? '/assets/default-avatar.svg'
      });
  }

  getUserInfo(){
    let form = new FormData();
    form.append("userId", this.userId);
    this.api.post("/GetProfilePhoto",
      (response) => {
        this.firstName = response.data[0].FirstName;
        this.lastName = response.data[0].LastName;
        this.profilePhoto = response.data[0].ProfilePhoto;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

}
