import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  startDate: string = '2022-01-01';
  endDate: string = '2022-08-01';
  orgData: any[] = [];
  traffic: any[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.GetOrgData();
    this.GetMonthlyTraffic();
  }

  RefreshData(start: HTMLInputElement, end: HTMLInputElement) {
    this.startDate = start.value;
    this.endDate = end.value;
    this.GetOrgData();
    this.GetMonthlyTraffic();
  }

  GetOrgData() {
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

  GetMonthlyTraffic() {
    let form = new FormData();
    form.append("startDate", this.startDate);
    form.append("endDate", this.endDate);
    console.log(form);

    this.api.post("/GetMonthlyTraffic",
      (response) => {
        console.log(response.data);
        this.traffic = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }



}
