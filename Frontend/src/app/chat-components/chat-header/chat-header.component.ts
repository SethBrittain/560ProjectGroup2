import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-chat-header',
  templateUrl: './chat-header.component.html',
  styleUrls: ['./chat-header.component.css']
})
export class ChatHeaderComponent implements OnInit {
  @Input() title: string = '';

  constructor(){}

  ngOnInit(): void {
      
  }
}
