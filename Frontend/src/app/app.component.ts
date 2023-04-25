import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  constructor(private auth : AuthService){}
  ngOnInit(): void {
    this.auth.user$.subscribe((user)=>{console.log(user);})
  }
}
