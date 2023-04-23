import { Injectable } from '@angular/core';
import { AxiosError, AxiosResponse } from 'axios';
import axios from 'axios' ;

@Injectable({
	providedIn: 'root'
})
export class ApiService {

  constructor() { axios.defaults.baseURL = "http://localhost:8080/api" }  

  public get(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data? : object)
  {
    axios.get(endpoint, data).then(callback).catch(onError);
  }

  public put(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data? : object)
  {
    axios.put(endpoint, data).then(callback).catch(onError);
  }
  
}
