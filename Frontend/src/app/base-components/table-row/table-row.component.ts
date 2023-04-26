import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-table-row',
  templateUrl: './table-row.component.html',
  styleUrls: ['./table-row.component.css']
})
export class TableRowComponent {
  @Input() row1: string = '';
  @Input() row2: string = '';
  @Input() row3: string = '';
  constructor(){}
}
