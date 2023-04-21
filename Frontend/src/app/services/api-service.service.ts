import { Injectable } from '@angular/core';
import { Axios, AxiosError, AxiosResponse } from 'axios';

@Injectable({
	providedIn: 'root'
})
export class ApiService {
	private static apiUrl : string = 'http://localhost:8080/api/';
	public ax : Axios;

    constructor() { this.ax = new Axios(); }

	public test(endpoint : string){//endpoint : string, callback : Promise<AxiosResponse>, onError : Promise<AxiosError>) : void {
		// console.log(`${ApiService.apiUrl}${endpoint}`);
		// this.ax.get(`${ApiService.apiUrl}${endpoint}`).then((response : AxiosResponse) => { console.log(response); });
		return this.ax.get(`${ApiService.apiUrl}${endpoint}`);
	}
}