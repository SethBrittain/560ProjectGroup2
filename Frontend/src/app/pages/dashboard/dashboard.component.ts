import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  startDate: string = '2022-01-01';
  endDate: string = '2023-01-01';
  orgData: any[] = [];
  traffic: any[] = [];
  growth: any[] = [];

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.GetOrgData();
    this.GetMonthlyTraffic();
    this.GetGrowthData();
  }

  RefreshData(start: HTMLInputElement, end: HTMLInputElement) {
    this.startDate = start.value;
    this.endDate = end.value;
    this.GetOrgData();
    this.GetMonthlyTraffic();
    this.GetGrowthData();
  }

  GetOrgData() {
    let form = new FormData();
    form.append("startDate", this.startDate);
    form.append("endDate", this.endDate);

    this.api.post("/OrganizationsData",
      (response) => {
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
        this.traffic = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  GetGrowthData() {
    let form = new FormData();
    form.append("startDate", this.startDate);
    form.append("endDate", this.endDate);

    this.api.post("/GetAppGrowth",
      (response) => {
        this.growth = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }



}
