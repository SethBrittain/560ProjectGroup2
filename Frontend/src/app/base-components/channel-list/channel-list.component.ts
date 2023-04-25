import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-channel-list',
  templateUrl: './channel-list.component.html',
  styleUrls: ['./channel-list.component.css']
})
export class ChannelListComponent implements OnInit {

  channels:any;

  constructor(private api: ApiService){}

  
  ngOnInit(): void {
      //this.channels = this.api.GetAllChannelsOfUser();
      this.GetAllChannelsOfUser();
  }

  GetAllChannelsOfUser(){
    let form = new FormData();
     // form.append("", channelId);
      console.log(form);

      this.api.post("/GetAllChannelsOfUser",
        (response) => {
          console.log(response.data);
          this.channels = response.data;
        },
        (error) => { console.log(error.message); },
        form
      );
  }

}
