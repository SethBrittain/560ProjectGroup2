import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-log-in',
  templateUrl: './log-in.component.html',
  styleUrls: ['./log-in.component.css']
})

export class LogInComponent {
  constructor(private api: ApiService){}

  login(){
	document.location.href = "/auth/cas/login";
  }
}
