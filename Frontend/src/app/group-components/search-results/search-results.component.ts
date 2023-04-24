import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api-service.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  title: string | null = '';
  constructor(private api: ApiService, private route:ActivatedRoute, private router: Router){
    this.router.routeReuseStrategy.shouldReuseRoute = function() {
      return false;
    }
  }

  ngOnInit(): void {
    let term = this.route.snapshot.paramMap.get('terms');
    this.title = term;
  }

}
