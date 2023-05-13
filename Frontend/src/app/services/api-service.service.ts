import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthService, User } from '@auth0/auth0-angular';
import { AxiosError, AxiosResponse } from 'axios';
import axios from 'axios' ;

@Injectable({
	providedIn: 'root'
})
export class ApiService {

  constructor(private auth : AuthService) { 
    axios.defaults.baseURL = environment.ApiUrl; 
  }  

  public post(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data : FormData = new FormData())
  {
    this.auth.idTokenClaims$.subscribe((keyToken)=>{
        if(keyToken && keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]){
          data.append("apiKey",keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]);
        } else {
          this.auth.user$.subscribe((user)=>{
            var fd = new FormData();
            fd.append("emailAddress", user?.email ?? '',);
            fd.append("firstName", user?.given_name ?? '');
            fd.append("lastName", user?.family_name ?? '');
            fd.append("authId", user?.sub ?? '');
            axios.put("/CreateUserOrGetKey",fd);
            // .then((response)=>{
            //   this.auth.logout();
            // }).catch((error)=>{
            //   console.log("Failed to obtain new API key");
            //   console.log(error.message);
            // });
          });
        }
        axios.post(endpoint,data).then(callback).catch(onError);
    });
  }

  public put(endpoint : string, callback : (response : AxiosResponse<any,any>)=>void, onError : (error : AxiosError)=>void, data : FormData = new FormData())
  {
    this.auth.idTokenClaims$.subscribe((keyToken)=>{
      if(keyToken && keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]){
        data.append("apiKey",keyToken["https://pidgin.dev-nhscnbma.com/apiKey"]);
      } else {
        this.auth.user$.subscribe((user)=>{
          var fd = new FormData();
          fd.append("emailAddress", user?.email ?? '',);
          fd.append("firstName", user?.given_name ?? '');
          fd.append("lastName", user?.family_name ?? '');
          fd.append("authId", user?.sub ?? '');
          axios.put("/CreateUserOrGetKey",fd);//.then((response)=>{
          //   this.auth.logout();
          // }).catch((error)=>{
          //   console.log("Failed to obtain API key");
          //   console.log(error.message);
          // });
        });
      }
      axios.put(endpoint,data).then(callback).catch(onError);
  });
  }
}
