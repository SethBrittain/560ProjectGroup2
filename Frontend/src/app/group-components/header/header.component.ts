import { Component, OnInit } from '@angular/core';
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

  constructor(private api: ApiService) {}

  ngOnInit(): void {
      this.getUserInfo();
  }

  getUserInfo(){
    let form = new FormData();
    form.append("userId", this.userId);
    this.api.post("/GetProfilePhoto",
      (response) => {
        console.log(response.data);
        this.firstName = response.data[0].FirstName;
        this.lastName = response.data[0].LastName;
        this.profilePhoto = response.data[0].ProfilePhoto;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

}
