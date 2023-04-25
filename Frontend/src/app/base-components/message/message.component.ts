import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  @Input() firstName: string = '';
  @Input() lastName: string = '';
  @Input() dateSent: string = '';
  @Input() message: string = '';
  @Input() image: string = '';

  constructor(){}

  ngOnInit(): void {
      
  }

  updateUrl(){
    this.image = '/assets/default-avatar.svg'
  }

}
