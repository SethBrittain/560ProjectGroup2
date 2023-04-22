import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogInComponent } from './pages/log-in/log-in.component';
import { MainWindowComponent } from './pages/main-window/main-window.component';
import { ChatComponent } from './group-components/chat/chat.component';
// Import modules before adding them to routing like: import { moduleName } from 'module/path/string';

// Add routes here in the form of { path: 'pagepath', component: PageComponent } 
// more documentation can be found here: https://angular.io/tutorial/tour-of-heroes/toh-pt5
const routes: Routes = [
  { path: '', component: LogInComponent },
  { path: 'app', component: MainWindowComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { 
}
