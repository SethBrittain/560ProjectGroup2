import { Component, OnInit } from '@angular/core';
import { AxiosError, AxiosResponse, AxiosRequestConfig } from 'axios';
import { ApiService } from 'src/app/services/api-service.service';
@Component({
  selector: 'app-message-input',
  templateUrl: './message-input.component.html',
  styleUrls: ['./message-input.component.css']
})
export class MessageInputComponent implements OnInit {

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.api.test("Example").then((response : object) => { console.log(response); });
  }
}
