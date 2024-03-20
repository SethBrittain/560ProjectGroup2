import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import axios from 'axios';

type OrgDataType = { 
  name: string; 
  activeUserCount: number;
  messageCount: number;
};

type TrafficType = {
  month: string;
  year: string;
  messagesSent: number;
  rank: number;
};

type GrowthType = {
  numberOfActiveUsers : number,
  numberOfInactiveUsers : number,
  numberOfActiveOrgs : number,
  numberOfInactiveOrgs : number
};

type GroupActivityType = {
  groupId : number,
  name : string,
  messagesSent : number,
  highestSender : string
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  orgId: string = '1'; // HARD CODED
  startDate: string = '2022-01-01';
  endDate: string = '2023-01-01';
  
  // orgData: OrgDataType = undefined;
  orgData: OrgDataType | undefined;
  traffic: TrafficType[] | undefined;
  growth: GrowthType | undefined;
  groups: GroupActivityType[] | undefined;

  constructor(private api: ApiService) {}

  ngOnInit() : void {
    this.GetOrgData();
    this.GetMonthlyTraffic();
    this.GetGrowthData();
    this.GetGroupData();
  }

  RefreshData(start: HTMLInputElement, end: HTMLInputElement) {
    this.startDate = start.value;
    this.endDate = end.value;
    this.GetOrgData();
    this.GetMonthlyTraffic();
    this.GetGrowthData();
    this.GetGroupData();
  }

  GetOrgData = () => {
    let data = new FormData();
    data.append("startDate", this.startDate);
    data.append("endDate", this.endDate);
    
    fetch("/api/OrganizationsData", {
      "method": "POST",
      "body": data
    })
    .then(response => response.json())
    .then(data => this.orgData = data)
    .catch(error => console.log(error.message));
    // let form = new FormData();
    // form.append("startDate", this.startDate);
    // form.append("endDate", this.endDate);

    // fetch("/api/OrganizationsData", {
    //   "method": "POST",
    //   body: form
    // })
    // .then(async (response : Response)=>{
    //   let data = await response.json();
    //   orgData.name = data.name;
    //   orgData.activeUserCount = data.activeUserCount;
    //   orgData.messageCount = data.messageCount;
    // })
    // .catch(error => console.log(error.message));;
    // console.log(orgData);
  }

  GetMonthlyTraffic() {
    let form = new FormData();
    form.append("startDate", this.startDate);
    form.append("endDate", this.endDate);

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

  GetGroupData() {
    let form = new FormData();
    form.append("organizationId", this.orgId)
    form.append("startDate", this.startDate);
    form.append("endDate", this.endDate);

    this.api.post("/GetGroupActivity",
      (response) => {
        console.log(response.data);
        this.groups = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }
}
