import { Injectable } from '@angular/core';
import { AxiosError, AxiosResponse } from 'axios';
import axios from 'axios' ;

@Injectable({
	providedIn: 'root'
})
export class ApiService {

  constructor() { axios.defaults.baseURL = "http://localhost:8080/api" }  

  public test(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void)
  {
    axios.get(endpoint).then(callback).catch(onError);
  }
}
