import { Component, OnInit, Input, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import { ActivatedRoute } from '@angular/router';
import { ChatService } from 'src/app/services/chat-service.service';

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
  updater : any;

  constructor(private api: ApiService, private route: ActivatedRoute, private chat : ChatService) { }

  updateMessages() {
    if (!this.messages) return;
    let data = new FormData();
    let latest = this.messages[this.messages.length-1];


    if (this.type == "channel") {
      data.append("sinceDateTime",(latest?.CreatedOn)??'1753-01-01');
      data.append("channelId",this.channelId);
      this.api.post("/GetNewChannelMessages",(response)=>{
        response.data.forEach((element:any)=> {
          if (!element) return;
          else if (!element.CreatedOn) return;

          if (!latest) {
            this.messages.push(element);
            window.scrollTo(0, document.body.scrollHeight);
          }
          else if (element.CreatedOn!=latest.CreatedOn) {
            this.messages.push(element);
            window.scrollTo(0, document.body.scrollHeight);
          }
        });
      }, (error)=>{
        console.log(error);
      }, data);
    } else {
      data.append("sinceDateTime",(latest?.CreatedOn)??'1753-01-01');
      data.append("otherUserId",this.channelId);
      this.api.post("/GetNewDirectMessages",(response)=>{
        response.data.forEach((element:any)=> {
          if (!element) return;
          else if (!element.CreatedOn) return;

          if (!latest) {
            this.messages.push(element);
            window.scrollTo(0, document.body.scrollHeight);
          }
          else if (element.CreatedOn!=latest.CreatedOn) {
            this.messages.push(element);
            window.scrollTo(0, document.body.scrollHeight);
          }
        });
      }, (error)=>{
        console.log(error);
      }, data);
    }

    // this.messages.push({
    //   "ProfilePhoto":"https://lh3.googleusercontent.com/a/AGNmyxY38W8aDtBM_BT-wkqP2KybvYWu1vDF3wlu-4M73w=s96-c",
    //   "IsMine":"0",
    //   "SenderId":"1065",
    //   "LastName":"Haynes",
    //   "Message":"hey seth",
    //   "FirstName":"Sam",
    //   "UpdatedOn":"2023-04-26 01:33:43.1595796 -05:00",
    //   "RecipientId":"1008",
    //   "MsgId":"5106"
    // });
    // console.log("added");
  }

  ngOnInit(): void {

    let id = this.route.snapshot.paramMap.get('id');
    this.type = this.route.snapshot.paramMap.get('type');
    
    if (this.type == "channel") {
      this.GetMessages(id);
      this.GetChannelName(id);
      if (id) this.chat.connectChannel(Number.parseInt(id));
    }
    else {
      this.GetDirectMessages(id);
      this.GetUserName(id);
      if (id) this.chat.connectDirect(Number.parseInt(id));
    }

    //this.title = this.type + ' ' + id;
    this.channelId = id;
    this.updater = setInterval(()=>{this.updateMessages()}, 1000);
  }

  GetMessages(id: any) {
    // Gets the messages from a channelId
    let form = new FormData();
    form.append("channelId", id);

    this.api.post("/GetAllChannelMessages",
      (response) => {
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
    this.api.post("/GetDirectMessages",
      (response) => {
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
        this.title = response.data[0].FirstName + ' ' + response.data[0].LastName;
      },
      (error) => { console.log(error.message); },
      form
    );
    this.title = this.type + ' ' + this.channelId;
  }
}





