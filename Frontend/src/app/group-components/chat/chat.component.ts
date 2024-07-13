import { Component, OnInit, Input, ElementRef, ViewChild, AfterViewInit, OnDestroy, AfterViewChecked, AfterContentChecked } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ChatService } from 'src/app/services/chat-service.service';
import { Observable, of, switchMap } from 'rxjs';

interface Chat {
  id: Number,
  type: "channel" | "direct"
}

interface Message {
  msgId : number,
  profilePhoto : string,
  firstName : string,
  lastName : string,
  updatedOn : Date,
  message : string,
  isMine : boolean
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  messages : Message[] = [];

  channelId: any = '';
  title : any = "";
  type : any;

  chatId : Number | undefined;
  chatType : "channel" | "direct" | undefined;

  constructor(private route : ActivatedRoute, private api : ApiService, private chat : ChatService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log('test');
      this.chatId = Number.parseInt(params['id']);
      this.chatType = window.location.href.includes("channel") ? "channel" : "direct";

      if (this.chatId == undefined || this.chatType == undefined) return;
      this.loadChat(this.chatId, this.chatType);
    });
  }

  loadChat(chatId : Number, chatType : "channel" | "direct") {
    if (chatType == "channel" )
    {
      this.getMessages(chatId);
      this.getChannelName(chatId);

      let self = this;
      this.chat.connectChannel(chatId, (message : MessageEvent<any>)=>{
        self.messages.push(JSON.parse(message.data));
        console.log("messages: ", self.messages);
        // this.scrollToBottom();
      });
    }
    else
    {
      // console.log(Number.parseInt(id));
      // this.GetDirectMessages(id);
      // this.GetUserName(id);
      // this.chat.connectDirect(Number.parseInt(id), (message : MessageEvent<any>)=>{
      //   self.messages.push(JSON.parse(message.data));
      //   this.scrollToBottom();
      // });
    }
  }

  getMessages(id: any) {
    // Gets the messages from a channelId
    let form = new FormData();
    form.append("channelId", id);

    this.api.post("/GetAllChannelMessages",
      (response) => {
        this.messages = response.data;
        // this.scrollToBottom();
      },
      (error) => { console.log(error.message); },
      form
    );
  }

  getChannelName(id: any) {
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
}





// import axios from 'axios';

// type Message = {
//   messageId : number,
//   profilePhoto : string,
//   firstName : string,
//   lastName : string,
//   updatedOn : Date,
//   message : string,
//   isMine : boolean
// }

// @Component({
//   selector: 'app-chat',
//   templateUrl: './chat.component.html',
//   styleUrls: ['./chat.component.css']
// })
// export class ChatComponent implements OnInit  {
//   @ViewChild('scrollMe') private myScrollContainer!: ElementRef;

//   title: any;
//   channelId: any = '';
//   messages: any[] = [];
//   type: any;
//   updater : any;

//   constructor(private api: ApiService, private route: ActivatedRoute, private chat : ChatService) {
//   }
  
//   /**
//    * Initializes the component
//    */
//   ngOnInit(): void {
//     console.log('test');
//     let id = this.route.snapshot.paramMap.get('id');
//     let type  = window.location.href.includes("channel") ? "channel" : "direct";
//     let self = this;

//     if (id == null || id == undefined || type == null || type == undefined) return;
//     this.type = type;

//     if (this.type == "channel" )
//     {
//       this.GetMessages(id);
//       this.GetChannelName(id);
//       this.chat.connectChannel(Number.parseInt(id), (message : MessageEvent<any>)=>{
//         self.messages.push(JSON.parse(message.data));
//         this.scrollToBottom();
//       });
//     }
//     else
//     {
//       console.log(Number.parseInt(id));
//       this.GetDirectMessages(id);
//       this.GetUserName(id);
//       this.chat.connectDirect(Number.parseInt(id), (message : MessageEvent<any>)=>{
//         self.messages.push(JSON.parse(message.data));
//         this.scrollToBottom();
//       });
//     }
//   }

//   scrollToBottom(): void {
//     console.log("Scrolling to bottom");
//     console.log(document.documentElement.scrollHeight)
//     console.log(this.myScrollContainer.nativeElement.scrollHeight);
//     try {
//       setTimeout(() => {
//       window.scroll({
//         top: document.documentElement.scrollHeight + this.myScrollContainer.nativeElement.scrollHeight,
//         left: 0,
//         behavior: 'smooth'
//       });
//     }, 0);
//     } catch(err) {
//       console.error("Failed to scroll to bottom");
//     }                 
//   }

  

//   /**
//    * Gets the messages from a channel
//    * @param id The channel Id
//    */
//   GetMessages(id: any) {
//     // Gets the messages from a channelId
//     let form = new FormData();
//     form.append("channelId", id);

//     this.api.post("/GetAllChannelMessages",
//       (response) => {
//         this.messages = response.data;
//         this.scrollToBottom();
//       },
//       (error) => { console.log(error.message); },
//       form
//     );
//   }

//   /**
//    * Gets messages between the current user
//    * and a specified user
//    * @param userBId The userId of the recipient
//    */
//   GetDirectMessages(userBId: any) {
//     let form = new FormData();
//     form.append("userBId", userBId)
//     this.api.post("/GetDirectMessages",
//       (response) => {
//         this.messages = response.data;
//       },
//       (error) => { console.log(error.message); },
//       form
//     );
//   }

//   /**
//    * Gets the channel name to display in the header
//    */
//   GetChannelName(id: any) {
//     let form = new FormData();
//     form.append("channelId", id);
//     this.api.post("/GetChannelName",
//       (response) => {
//         this.title = response.data[0].Name;
//       },
//       (error) => { console.log(error.message); },
//       form
//     );
//   }

//   /**
//    * Gets the user name from a specified Id
//    * @param id The user id to get user info
//    */
//   GetUserName(id: any) {
//     let form = new FormData();
//     form.append("userId", id);
//     this.api.post("/GetProfilePhoto",
//       (response) => {
//         this.title = response.data[0].FirstName + ' ' + response.data[0].LastName;
//       },
//       (error) => { console.log(error.message); },
//       form
//     );
//     this.title = this.type + ' ' + this.channelId;
//   }

// }

