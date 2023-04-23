import { Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-dm-list-item',
  templateUrl: './dm-list-item.component.html',
  styleUrls: ['./dm-list-item.component.css']
})
export class DmListItemComponent implements OnInit {

  type: string = 'user';
  @Input() userId: string = '';
  @Input() firstName: string = '';
  @Input() lastName: string = '';
  @Input() image: string = '';

  constructor(){}

  ngOnInit(): void {
      
  }
}
