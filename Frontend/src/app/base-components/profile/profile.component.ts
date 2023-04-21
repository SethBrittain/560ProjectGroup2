import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit{

  @Input() firstName: string = '';
  @Input() lastName: string = '';
  @Input() image: string = '';

  constructor(){}

  ngOnInit(): void {
      
  }

}
