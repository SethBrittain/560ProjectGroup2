import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-dm-list',
  templateUrl: './dm-list.component.html',
  styleUrls: ['./dm-list.component.css']
})
export class DmListComponent implements OnInit {

  orgId: string = '1'; // SHOULD GET ORG ID
  users: any;

  constructor(private api: ApiService) { }


  ngOnInit(): void {
    this.populateUsers();
  }

  populateUsers(): void {
    let form = new FormData();
    form.append("organizationId", this.orgId);

    this.api.post("/GetAllUsersInOrganization",
      (response) => {
        this.users = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }
}
