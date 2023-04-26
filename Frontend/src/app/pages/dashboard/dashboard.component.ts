import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{

  startDate: string = '01-01-2022';
  endDate: string = '01-01-2023';
  orgData: any[] = [

  ];

  constructor(private api: ApiService){}

  ngOnInit(): void {
      this.GetOrgData();
  }

  GetOrgData(){
    let form = new FormData();
    form.append("startDate", this.startDate);
    form.append("endDate", this.endDate);
    console.log(form);

    this.api.put("/OrganizationsData",
      (response) => {
        console.log(response.data);
        this.orgData = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

}
