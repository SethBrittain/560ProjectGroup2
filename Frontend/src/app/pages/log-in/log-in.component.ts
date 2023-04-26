import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import { AuthService } from '@auth0/auth0-angular';
@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})

export class LogInComponent implements OnInit{

  userId: string = '12345';

  constructor(private api: ApiService, private auth : AuthService){}

  ngOnInit(): void { 
    if(this.auth.isAuthenticated$){console.log("Logged In!");}
    //this.auth.loginWithRedirect();
  }

  login() {
    this.auth.loginWithRedirect({appState:{target:"/app"}});
  }
}
