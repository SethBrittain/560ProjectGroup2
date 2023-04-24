import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-result',
  templateUrl: './result.component.html',
  styleUrls: ['./result.component.css']
})
export class ResultComponent implements OnInit {

  @Input() name: string = 'test name';
  @Input() messageId: string = '';

  constructor(){}

  ngOnInit(): void {
      
  }

}
