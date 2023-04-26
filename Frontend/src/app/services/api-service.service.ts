import { Injectable } from '@angular/core';
import { AuthService, User } from '@auth0/auth0-angular';
import { AxiosError, AxiosResponse } from 'axios';
import axios from 'axios' ;

@Injectable({
	providedIn: 'root'
})
export class ApiService {

  constructor(private auth : AuthService) { 
    axios.defaults.baseURL = "http://localhost:8080/api"; 
  }  

  public post(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data : FormData = new FormData())
  {
    this.auth.idTokenClaims$.subscribe((keyToken)=>{
        if(keyToken && keyToken["http://kjflaskdjfhaslkdjf.netapi_key"]){
          console.log("Got token");
          console.log(keyToken["http://kjflaskdjfhaslkdjf.netapi_key"]);
          data.append("apiKey",keyToken["http://kjflaskdjfhaslkdjf.netapi_key"]);
        } else { 
          //new user
        }
        axios.post(endpoint,data).then(callback).catch(onError);
    });
  }

  public put(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data : FormData = new FormData())
  {
    this.auth.idTokenClaims$.subscribe((keyToken)=>{
      if(keyToken && keyToken["http://kjflaskdjfhaslkdjf.netapi_key"]){
        console.log("Got token");
        console.log(keyToken["http://kjflaskdjfhaslkdjf.netapi_key"]);
        data.append("apiKey",keyToken["http://kjflaskdjfhaslkdjf.netapi_key"]);
      } else { 
        //new user
      }
      axios.put(endpoint,data).then(callback).catch(onError);
    });
  }
}
