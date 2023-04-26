import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-dm-list-item',
  templateUrl: './dm-list-item.component.html',
  styleUrls: ['./dm-list-item.component.css']
})
export class DmListItemComponent implements OnInit {

  type: string = 'chat';
  @Input() userId: string = '';
  @Input() firstName: string = '';
  @Input() lastName: string = '';
  @Input() image: string = '/assets/default-avatar.svg';

  constructor() { }

  ngOnInit(): void {
    
  }

  updateUrl(){
    this.image = '/assets/default-avatar.svg'
  }

  reloadPage() {
    setTimeout(() => {
      window.location.reload();
    }, 1);
  }
}
