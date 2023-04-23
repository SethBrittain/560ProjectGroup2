import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-message-input',
  templateUrl: './message-input.component.html',
  styleUrls: ['./message-input.component.css']
})
export class MessageInputComponent implements OnInit {

  constructor(private api: ApiService) { }

  ngOnInit(): void {

  }

  test() : void {
    this.api.test("/Example", (response)=>{console.log(response.data);}, (error)=>{console.log(error.message);});
  }
}

// {startDate:"2013-03-22", endDate:"-----"} optional param
// response.data.startDate to access it
// test is the event.
