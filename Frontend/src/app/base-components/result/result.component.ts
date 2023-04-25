import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.css']
})
export class ResultComponent implements OnInit {

  @Input() name: string = 'test name';
  @Input() messageId: string = '';

  
  @Input() firstName: string = '';
  @Input() lastName: string = '';
  @Input() dateSent: string = '';
  @Input() message: string = '';
  @Input() image: string = '';

  constructor(){}

  ngOnInit(): void {
      
  }

  updateUrl(){
    this.image = '/assets/default-avatar.svg'
  }

}
