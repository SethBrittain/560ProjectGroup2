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
    this.InsertMessageIntoChannel(this.message, 2, Number(this.channelId));
  }

  /**
	 * Insert Message into channel
	 * @param message Message to insert
	 * @param senderId sender id
	 * @param channelId recipient id
	 */
  InsertMessageIntoChannel( message : String, senderId : number, channelId : number) : void
  {
    
    this.api.put("/InsertMessageIntoChannel",  (response)=>
    {
      console.log(response.data);
    }, (error)=>{console.log(error.message);},
    {
      message : message,
      senderId : senderId,
      channelId : channelId
    });
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
}

// {startDate:"2013-03-22", endDate:"-----"} optional param
// response.data.startDate to access it
// test is the event.
