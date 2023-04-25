import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import { ActivatedRoute } from '@angular/router';
import { ParamMap } from '@angular/router'

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  title: string = 'test';
  id: any;
  type: any;
  messages: any[] = [

  ];

  constructor(private api: ApiService, private route: ActivatedRoute) { }


  ngOnInit(): void {
    
    this.id = this.route.snapshot.paramMap.get('id');
    this.type = this.route.snapshot.paramMap.get('type');
    console.log(this.type);
    console.log(this.id);
    console.log(this.type == "chat");
    if (this.type == "channel") {
      this.GetMessages();
      this.GetChannelName();
    }
    else {
      this.GetDirectMessages();
      this.GetUserName();
    }
  }

  GetMessages() {
    // Gets the messages from a channelId
    let form = new FormData();
    form.append("channelId", this.id);

    this.api.post("/GetAllChannelMessages",
      (response) => {
        console.log(response.data);
        this.messages = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  // Gets the messages from a userId
  // THIS QUERY NEEDS FIXING (DEPENDENT ON USERID)
  GetDirectMessages() {
    let form = new FormData();
    form.append("userBId", this.id)
    this.api.post("/GetDirectMessages",
      (response) => {
        console.log(response.data);
        this.messages = response.data;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  /**
   * Gets the channel name to display in the header
   */
  GetChannelName(){
    let form = new FormData();
    form.append("channelId", this.id);
    this.api.post("/GetChannelName",
    (response) => {
      console.log(response.data);
      //this.title = response.data.ChannelName;
    },
    (error) => { console.log(error.message); },
    form
    );
  }

  GetUserName() {
    let form = new FormData();
    form.append("userId", this.id);
    this.api.post("/GetProfilePhoto",
    (response) => {
      console.log(response.data);
      //this.title = response.data.ChannelName;
    },
    (error) => { console.log(error.message); },
    form
    );
    console.log(this.type);
    this.title = this.type + ' ' + this.id;
  }
}





