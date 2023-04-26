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

  title: any;
  channelId: any = '';
  messages: any[] = [

  ];
  type: any;

  constructor(private api: ApiService, private route: ActivatedRoute) { }


  ngOnInit(): void {

    let id = this.route.snapshot.paramMap.get('id');
    this.type = this.route.snapshot.paramMap.get('type');
    //this.GetDirectMessages(id);
    console.log("this is the type of the control you just clicked: " + this.type);
    if (this.type == "channel") {
      this.GetMessages(id);
      this.GetChannelName(id);
    }
    else {
      this.GetDirectMessages(id);
      this.GetUserName(id);
    }

    //this.title = this.type + ' ' + id;
    this.channelId = id;
  }

  GetMessages(id: any) {
    // Gets the messages from a channelId
    let form = new FormData();
    form.append("channelId", id);
    console.log(form);

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
  GetDirectMessages(userBId: any) {
    let form = new FormData();
    form.append("userBId", userBId)
    console.log(form);
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
  GetChannelName(id: any) {
    let form = new FormData();
    form.append("channelId", id);
    this.api.post("/GetChannelName",
      (response) => {
        console.log(response.data);
        this.title = response.data[0].Name;
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  GetUserName(id: any) {
    let form = new FormData();
    form.append("userId", id);
    this.api.post("/GetProfilePhoto",
      (response) => {
        console.log(response.data);
        this.title = response.data[0].FirstName + ' ' + response.data[0].LastName;
      },
      (error) => { console.log(error.message); },
      form
    );
    console.log(this.type);
    this.title = this.type + ' ' + this.channelId;
  }
}





