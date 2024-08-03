import { Inject, NgModule, inject } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LogInComponent } from './pages/log-in/log-in.component';
import { MainWindowComponent } from './pages/main-window/main-window.component';
import { ChatComponent } from './chat-components/chat/chat.component';
import { SearchResultsComponent } from './group-components/search-results/search-results.component';
import { EmptyStateComponent } from './group-components/empty-state/empty-state.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { ProfilePageComponent } from './pages/profile-page/profile-page.component';
import { SignUpComponent } from './pages/sign-up/sign-up.component';

// Import modules before adding them to routing like: import { moduleName } from 'module/path/string';

// Add routes here in the form of { path: 'pagepath', component: PageComponent } 
// more documentation can be found here: https://angular.io/tutorial/tour-of-heroes/toh-pt5
const routes: Routes = [
	{ path: '', redirectTo: 'app', pathMatch: 'full' },
	{ path: 'login', component: LogInComponent },
	{ path: 'signup', component: SignUpComponent },
	{ path: 'profile', component: ProfilePageComponent },
	{ path: 'dashboard', component: DashboardComponent },
	{
		path: 'app', component: MainWindowComponent,
		children: [
			{ path: '', component: ChatComponent },
			{ path: 'search/:terms', component: SearchResultsComponent }
		]
	},
	//   { path: '**', redirectTo: 'app' },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule {
}
