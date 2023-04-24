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

  title:string = '';
  channelId: any = '';
  messages:any[] = [
    {id:123, name:'Channel1'},
    {id:124, name:'Channel2'},
    {id:125, name:'Channel3'},
    {id:126, name:'Channel4'},
    {id:127, name:'Channel5'}
  ];

  constructor(private api: ApiService, private route:ActivatedRoute){}

  ngOnInit(): void {
      let id = this.route.snapshot.paramMap.get('id');
      let type = this.route.snapshot.paramMap.get('type');
      this.title = type +' ' + id;
      this.channelId = id;


      if (type == 'channel'){
        
       // this.GetAllChannelMessages(Number(id));
        //messages = this.api.GetAllChannelMessages(id);
        //name = this.api.GetChannelName(id)
      }
      else {
        //messages = this.api.GetDirectMessages(id, this.api.GetUserId());
        //name = this.api.GetUserName(id)
      }

      

      }

      /*
      GetAllChannelMessages(channelId : number) : void
      {
        this.api.put("/GetAllChannelMessages",  (response)=>
        {
          this.messages.push({Message: "lorem 30", FirstName: "sam", LastName: "haynes", CreatedOn: "12/2/1999" })
          console.log(response.data);
        }, (error)=>{console.log(error.message);},
        {
          channelId : channelId
        });
      }
      */

  }





