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
  name: string = '';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
	  this.name = "";
	  this.profilePhoto = "";
  }

}
