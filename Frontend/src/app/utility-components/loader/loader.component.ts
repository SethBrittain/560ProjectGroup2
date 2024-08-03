import { Component } from "@angular/core";

@Component({
	selector: 'loader',
	templateUrl: './loader.component.html',
	styleUrls: ['./loader.component.css']
})
export class LoaderComponent {
	loaderImgSrc: string = '/static/loader.svg';
}