import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-message-input',
  templateUrl: './message-input.component.html',
  styleUrls: ['./message-input.component.css']
})
export class MessageInputComponent implements OnInit {

  message: string = '';

  @Input()
  channelId: string = '';

  @Input()
  type: string = '';

  @ViewChild('sendInput',{static:false}) inputElement : ElementRef | null = null; 

  constructor(private api: ApiService) {

  }

  ngOnInit(): void {


  }

  GetVal(event: any) {
    this.message = event?.target.value;
  }

  SendMessageHandler() {

    if (this.type == "channel") {
      this.InsertMessageIntoChannel(this.message, this.channelId);
    }
    else {
      this.InsertDirectMessage(this.message, this.channelId);
    }
    console.log(this.inputElement);
    if (this.inputElement) this.inputElement.nativeElement.value='';

    console.log("SendMessageHandlerHit");
  }

  /**
   * Insert Message into channel
   * @param message Message to insert
   * @param senderId sender id
   * @param channelId recipient id
   */
  InsertMessageIntoChannel(message: string, channelId: string): void {
    let form = new FormData();
    form.append("message", message);
    form.append("channelId", channelId);
    this.api.put("/InsertMessageIntoChannel", (response) => { }, (error) => { console.log(error.message); },
      form);
  }



  /**
 * Insert direct message
 * @param message message to insert
 * @param senderId sender id
 * @param recipientId recipient id 
 */
  ///NEED TO PASS API KEY TO USERCONTROLLER
  InsertDirectMessage(message: any, recipientId: any): void {
    let form = new FormData();
    form.append("message", message);
    form.append("recipientId", recipientId);
    this.api.put("/InsertDirectMessage", (response) => {
      console.log(response.data);
    }, (error) => { console.log(error.message); },
      form);
  }

  /* Testing other kinds of requests */

  GetAllChannelsInGroup(groupId: string): void {

    let form = new FormData();
    form.append("groupId", groupId);
    console.log(form);
    this.api.put("/GetAllChannelsInGroup", (response) => {
      let fruit: string[][] = response.data;
      //console.log(response.data);
      console.log(response.data[5]);
    }, (error) => { console.log(error.message); },
      form);
  }
}
// {startDate:"2013-03-22", endDate:"-----"} optional param
// response.data.startDate to access it
// test is the event.