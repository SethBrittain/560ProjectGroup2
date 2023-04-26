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
    this.auth.user$.subscribe((user)=>{
      var fd = new FormData();
      fd.append("emailAddress", user?.email ?? '',);
      fd.append("firstName", user?.given_name ?? '');
      fd.append("lastName", user?.family_name ?? '');
      fd.append("authId", user?.sub ?? '');
      axios.put("/CreateUserOrGetKey",fd).then((response)=>{
        data.append("ApiKey", response.data.apiKey);
        axios.post(endpoint,data).then(callback).catch(onError);
      }).catch((error)=>{
        console.log("Failed to obtain API key");
        console.log(error.message);
      });
    })
  }

  public put(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data : FormData = new FormData())
  {
    this.auth.user$.subscribe((user)=>{
      var fd = new FormData();
      fd.append("emailAddress", user?.email ?? '',);
      fd.append("firstName", user?.given_name ?? '');
      fd.append("lastName", user?.family_name ?? '');
      fd.append("authId", user?.sub ?? '');
      axios.put("/CreateUserOrGetKey",fd).then((response)=>{
        data.append("ApiKey", response.data.apiKey);
        axios.put(endpoint,data).then(callback).catch(onError);
      }).catch((error)=>{
        console.log("Failed to obtain API key");
        console.log(error.message);
      });
    })
  }
}
