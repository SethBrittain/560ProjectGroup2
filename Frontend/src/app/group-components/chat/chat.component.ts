import { Component, OnInit, Input, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, AfterViewInit{

  title: any;
  channelId: any = '';
  messages: any[] = [];
  type: any;
  updater : any;
  @ViewChild('scrollContent') content: any;

  constructor(private api: ApiService, private route: ActivatedRoute, private _elementRef : ElementRef) { }

  /**
   * Initializes the component
   */
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

    this.channelId = id;
    this.updater = setInterval(()=>{this.updateMessages()}, 1000);
  }

  /**
   * Scrolls to the bottom of the page upon load
   */
  ngAfterViewInit(): void {
    this.scrollToBottom();
  }

  /**
   * Updates a message
   * @returns 
   */
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
      this.scrollToBottom();
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

  /**
   * Gets the messages from a channel
   * @param id The channel Id
   */
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

  /**
   * Gets messages between the current user
   * and a specified user
   * @param userBId The userId of the recipient
   */
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

  /**
   * Gets the user name from a specified Id
   * @param id The user id to get user info
   */
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

  /**
   * Scrolls to the bottom of the chat frame
   */
  scrollToBottom() { 
    this.content.scrollTop = this.content.scrollHeight;
  }
}





