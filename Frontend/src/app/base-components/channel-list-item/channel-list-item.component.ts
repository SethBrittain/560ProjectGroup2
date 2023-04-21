import { Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-channel-list-item',
  templateUrl: './channel-list-item.component.html',
  styleUrls: ['./channel-list-item.component.css']
})
export class ChannelListItemComponent implements OnInit {
  @Input() name: string = '';
  
  constructor() {}

  ngOnInit(): void {
      
  }
}
