import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{

  @Input() name: string = '';
  @Input() image: string = '';

  constructor(private auth: AuthService){}

  ngOnInit(): void {
      
  }

  logout(){
    this.auth.logout();
  }

}
