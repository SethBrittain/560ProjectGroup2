import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import axios from 'axios';
import { FormControl } from '@angular/forms';

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
	numberOfActiveUsers: number,
	numberOfInactiveUsers: number,
	numberOfActiveOrgs: number,
	numberOfInactiveOrgs: number
};

type GroupActivityType = {
	groupId: number,
	name: string,
	messagesSent: number,
	highestSender: string
}

@Component({
	selector: 'app-dashboard',
	templateUrl: './dashboard.component.html',
	styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

	orgId: number = 1; // HARD CODED
	endDate: FormControl = new FormControl<string>(new Date().toISOString().split('T')[0]);
	startDate: FormControl = new FormControl<string>(
		new Date(new Date().setDate(new Date().getDate() - 365 * 4)).toISOString().split('T')[0]
	);

	// orgData: OrgDataType = undefined;
	orgData: OrgDataType | undefined;
	traffic: TrafficType[] | undefined;
	growth: GrowthType | undefined;
	groups: GroupActivityType[] | undefined;

	constructor(private api: ApiService) { }

	ngOnInit(): void {
		this.RefreshData();
	}

	RefreshData(event: any = null) {
		console.log("start: ", this.startDate.value);
		console.log("end: ", this.endDate.value);

		const start: Date = new Date(Date.parse(this.startDate.value)) ?? new Date();
		const end: Date = new Date(Date.parse(this.endDate.value)) ?? new Date();

		this.GetOrgData(start, end);
		this.GetMonthlyTraffic(start, end);
		this.GetGrowthData(start, end);
		this.GetGroupData(start, end);
	}

	GetOrgData = (start: Date, end: Date) => {
		axios.get('/api/Analytics/OrganizationsData', {
			params: {
				startDate: this.startDate.value,
				endDate: this.endDate.value
			}
		})
			.then((res) => {
				this.orgData = res.data;
			})
			.catch((error) => {
				if (error.response)
					if (error.response.status === 401 || error.response.status === 403)
						window.location.href = '/login';
					else if (error.response.status === 404)
						this.orgData = undefined;
			});
	}

	GetMonthlyTraffic(start: Date, end: Date) {
		axios.get('/api/Analytics/MonthlyTraffic', {
			params: {
				startDate: this.startDate.value,
				endDate: this.endDate.value
			}
		})
		.then((res)=>{
			this.traffic = res.data;
		})
		.catch(error=>{
			if (error.response)
				if (error.response.status === 401 || error.response.status === 403)
					window.location.href = '/login';
				else if (error.response.status === 404)
					this.groups = undefined;
		});
	}

	GetGrowthData(start: Date, end: Date) {
		axios.get('/api/Analytics/AppGrowth', {
			params: {
				startDate: this.startDate.value,
				endDate: this.endDate.value
			}
		})
		.then((res)=>{
			this.growth = res.data;
		})
		.catch(error=>{
			if (error.response)
				if (error.response.status === 401 || error.response.status === 403)
					window.location.href = '/login';
				else if (error.response.status === 404)
					this.groups = undefined;
		});
	}

	GetGroupData(start: Date, end: Date) {
		axios.get('/api/Analytics/GroupActivity', {
			params: {
				orgId: this.orgId,
				startDate: this.startDate.value,
				endDate: this.endDate.value
			}
		})
		.then((res)=>{
			this.groups = res.data;
		})
		.catch(error=>{
			if (error.response)
				if (error.response.status === 401 || error.response.status === 403)
					window.location.href = '/login';
				else if (error.response.status === 404)
					this.groups = undefined;
		});
	}
}
