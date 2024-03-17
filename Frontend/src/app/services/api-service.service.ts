import { Injectable } from '@angular/core';
import { AxiosError, AxiosResponse } from 'axios';
import axios from 'axios';

@Injectable({
	providedIn: 'root'
})
export class ApiService {

	constructor() {}

	public post(endpoint: string, callback: (response: AxiosResponse<any, any>) => void, onError: (error: AxiosError) => void, data: FormData = new FormData()) {
		axios.post("/api"+endpoint, data).then(callback).catch(onError);
	}

	public put(endpoint: string, callback: (response: AxiosResponse<any, any>) => void, onError: (error: AxiosError) => void, data: FormData = new FormData()) {
		axios.put("/api"+endpoint, data).then(callback).catch(onError);
	}
}
