import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{

  userId: string = '3'; // THIS IS HARD CODED
  profilePhoto: string = '';
  name: string = '';

  constructor(private api: ApiService, private auth: AuthService) {}

  ngOnInit(): void {
      this.auth.user$.subscribe((user) => {
        this.profilePhoto = user?.picture ?? '/assets/default-avatar.svg'
      });
      this.auth.user$.subscribe((user) => {
        this.name = user?.name ?? '/assets/default-avatar.svg'
      });
  }

}
