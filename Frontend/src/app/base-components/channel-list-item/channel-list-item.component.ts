import { Component, OnInit, Input} from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-channel-list-item',
  templateUrl: './channel-list-item.component.html',
  styleUrls: ['./channel-list-item.component.css']
})
export class ChannelListItemComponent implements OnInit {

  type: string = 'channel';
  @Input() name: string = '';
  @Input() channelId: string = '';
  
  constructor(private api: ApiService) {}

  ngOnInit(): void {
      
  }
}
