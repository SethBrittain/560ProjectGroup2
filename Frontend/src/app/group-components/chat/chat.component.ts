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

  title: string = '';
  channelId: any = '';
  messages: any[] = [

  ];

  constructor(private api: ApiService, private route: ActivatedRoute) { }


  ngOnInit(): void {
    let id = this.route.snapshot.paramMap.get('id');
    let type = this.route.snapshot.paramMap.get('type');
    this.getMessages(id, type);
    this.title = type + ' ' + id;
    this.channelId = id;


    
  }

  getMessages(id: any, type: any) {
    // Gets the messages from a channelId
    if (type == 'channel') {
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
    else {
      let form = new FormData();
      form.append("UserId", id);
      console.log(form);

      this.api.get("/GetDirectMessages",
        (response) => {
          console.log(response.data);
          this.messages = response.data;
        },
        (error) => { console.log(error.message); },
        form
      );
    }
  }

  //getName

}





