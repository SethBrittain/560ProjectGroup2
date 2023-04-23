import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-dm-list',
  templateUrl: './dm-list.component.html',
  styleUrls: ['./dm-list.component.css']
})
export class DmListComponent implements OnInit{

  users:any[] = [
    {userId:234, firstName:'Sam', lastName:'Haynes', image:'https://robohash.org/dud'},
    {userId:235, firstName:'Collin', lastName:'Hammond', image:'https://robohash.org/her'},
    {userId:236, firstName:'Seth', lastName:'Brittain', image:'https://robohash.org/dfd'},
    {userId:237, firstName:'Heidi', lastName:'Cossins', image:'https://robohash.org/dss'}
  ];

  constructor(private api: ApiService){}

  
  ngOnInit(): void {
      //this.users = this.api.GetAllUsersInOrganization(this.api.GetOrgId(this.api.GetUserId));
      //this.users = response.data;
  }
}
