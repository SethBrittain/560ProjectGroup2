import { Component, OnInit, Input } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  @Input() firstName: string = '';
  @Input() lastName: string = '';
  @Input() dateSent: string = '';
  @Input() message: string = '';
  @Input() image: string = '';
  @Input() msgId: any = '';
  @Input() isMine: any;
  aVisible: boolean = false;
  pVisible: boolean = true;
  allVisible: boolean = true;
  editVisible: boolean = false;
comonstructor(private api: ApiService) { }

  ngOnInit(): void { }

  getVal(event: any) {
    this.message = event.target.value;
  }

  editMessage() {
    this.pVisible = false;
    this.aVisible = true;
  }

  deleteMessage() {
    let form = new FormData();
    form.append("msgId", this.msgId);
    console.log(form);

    this.api.put("/DeleteMessage",
      (response) => {
        console.log(response.data);
      },
      (error) => { console.log(error.message); },
      form
    );
    this.allVisible = false;
  }

  updateMessage() {

    let form = new FormData();
    form.append("msgId", this.msgId);
    form.append("message", this.message);
    console.log(form);

    this.api.put("/UpdateMessage",
      (response) => {
        console.log(response.data);
        //this.message = response.data.Message;
      },
      (error) => { console.log(error.message); },
      form
    );
    this.pVisible = true;
    this.aVisible = false;
  }

  updateAvatar() {
    this.image = '/assets/default-avatar.svg'
  }

  showButtons() {
    this.editVisible = true;
    this.deleteVisible = true;
  }

  hideButtons() {
    this.editVisible = false;
    this.deleteVisible = false;
  }

}
