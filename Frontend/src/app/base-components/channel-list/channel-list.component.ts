import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-channel-list',
  templateUrl: './channel-list.component.html',
  styleUrls: ['./channel-list.component.css']
})
export class ChannelListComponent implements OnInit {

  channels:any[] = [
    {id:123, name:'Channel1'},
    {id:124, name:'Channel2'},
    {id:125, name:'Channel3'},
    {id:126, name:'Channel4'},
    {id:127, name:'Channel5'}
  ];

  constructor(private api: ApiService){}

  
  ngOnInit(): void {
      //this.channels = this.api.getChannels();
  }

}
