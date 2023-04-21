import { Injectable } from '@angular/core';
import { Axios, AxiosError, AxiosPromise, AxiosResponse } from 'axios';

@Injectable({
	providedIn: 'root'
})
export class ApiService {
	private static apiUrl : string = 'localhost:8080/api/';
	private ax : Axios;

    constructor() { this.ax = new Axios(); }

	public test(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError<any>)=>void){
		this.ax.get(endpoint).then(callback).catch(onError);
	}
}