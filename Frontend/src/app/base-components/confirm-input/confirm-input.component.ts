import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-confirm-input',
  templateUrl: './confirm-input.component.html',
  styleUrls: ['./confirm-input.component.css']
})
export class ConfirmInputComponent {
	@Input() defaultValue!: string;
	@Input() placeholder!: string;
	@Input() formName!: string;
	@Input() formInput!: FormControl;
	@Input() type: string = 'text';
}
