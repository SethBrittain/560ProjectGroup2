import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-dm-list',
  templateUrl: './dm-list.component.html',
  styleUrls: ['./dm-list.component.css']
})
export class DmListComponent implements OnInit {

  orgId: string = '1'; // SHOULD GET ORG ID
  users: any[] = [
    { userId: 234, firstName: 'Sam', lastName: 'Haynes', image: 'https://robohash.org/dud' },
    { userId: 235, firstName: 'Collin', lastName: 'Hammond', image: 'https://robohash.org/her' },
    { userId: 236, firstName: 'Seth', lastName: 'Brittain', image: 'https://robohash.org/dfd' },
    { userId: 237, firstName: 'Heidi', lastName: 'Cossins', image: 'https://robohash.org/dss' }
  ];

  constructor(private api: ApiService) { }


  ngOnInit(): void {
    this.populateUsers();
  }

  populateUsers(): void {
    let form = new FormData();
    form.append("organizationId", this.orgId);
    console.log(form);

    this.api.put("/GetAllUsersInOrganization",
      (response) => {
        console.log(response.data);
        //this.users = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }
}
