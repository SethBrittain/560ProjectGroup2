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
  areaVisible: string = 'hidden';
  pVisible: string = 'visible';

  constructor(){}

  ngOnInit(): void {
      
  }

  getVal(event:any) {
    this.message = event.target.value;
  }

  editMessage() {

  }

  deleteMessage() {
    
  }

  updateMessage() {
    
  }

  updateUrl(){
    this.image = '/assets/default-avatar.svg'
  }

}
