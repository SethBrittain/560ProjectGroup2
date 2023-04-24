import { Component, OnInit, Input } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-message-input',
  templateUrl: './message-input.component.html',
  styleUrls: ['./message-input.component.css']
})
export class MessageInputComponent implements OnInit {

  message : string = '';

  @Input()
  senderId: string = '';
  
  @Input()
  channelId: string = '';


  constructor(private api: ApiService) { 
    
  }

  ngOnInit(): void {
    
  }

  /**
   *  testing, testing, 1.2.3.
   * @param username 
   */
  test(username : String) : void {
    this.api.get("/Example", (response)=>
    {
      console.log(response.data);
    }, (error)=>{console.log(error.message);},
    {
      username: username 
    });
  
  }

  GetVal(event:any){
    this.message = event?.target.value;
  }

  SendMessageHandler(){
    //this.InsertMessageIntoChannel(this.message, "23", "124");
  this.GetAllChannelsInGroup("1");
   // this.GetAllChannelMessages("1");
    console.log("SendMessageHandlerHit");
  }

  /**
	 * Insert Message into channel
	 * @param message Message to insert
	 * @param senderId sender id
	 * @param channelId recipient id
	 */
  InsertMessageIntoChannel( message : string, senderId : string, channelId : string) : void
  {
    let form = new FormData();
    form.append("message", message);
    form.append("senderId", senderId);
    form.append("channelId", channelId);
    this.api.put("/InsertMessageIntoChannel",  (response)=>
    {
      console.log(response.data);
    }, (error)=>{console.log(error.message);},
     form);
  }

  	/**
	 * Insert direct message
	 * @param message message to insert
	 * @param senderId sender id
	 * @param recipientId recipient id 
	 */
  InsertDirectMessage( message : String, senderId : number, recipientId : number) : void
  {
    
    this.api.put("/InsertDirectMessage",  (response)=>
    {
      console.log(response.data);
    }, (error)=>{console.log(error.message);},
    {
      message : message,
      senderId : senderId,
      recipientId: recipientId
    });
  }

  /* Testing other kinds of requests */ 
  
  GetAllChannelsInGroup( groupId : string) : void
  {
    
    let form = new FormData();
    form.append("groupId", groupId);
    console.log(form);
    console.log(groupId);
    this.api.put("/GetAllChannelsInGroup",  (response)=>
    {
      let fruit : string[][] = response.data;
      console.log(response.data);
    }, (error)=>{console.log(error.message);},
     form);
  }


  GetAllChannelMessages( channelId : string){
    let form = new FormData();
    form.append("channelId", channelId);
    console.log(form);
    console.log(channelId);
    this.api.get("/GetAllChannelMessages",  (response)=>
    {
      console.log(response.data);
    }, (error)=>{console.log(error.message);},
     {
      channelId : channelId
     });
  }

}

// {startDate:"2013-03-22", endDate:"-----"} optional param
// response.data.startDate to access it
// test is the event.
