// #region Angular Depedencies
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';
// #endregion

// #region Components
import { AppComponent } from './app.component';
import { LogInComponent } from './pages/log-in/log-in.component';
import { MainWindowComponent } from './pages/main-window/main-window.component';
import { NavMenuComponent } from './group-components/nav-menu/nav-menu.component';
import { SearchResultsComponent } from './group-components/search-results/search-results.component';
import { HeaderComponent } from './group-components/header/header.component';
import { ChatComponent } from './group-components/chat/chat.component';
import { ResultComponent } from './base-components/result/result.component';
import { MessageComponent } from './base-components/message/message.component';
import { ChatHeaderComponent } from './base-components/chat-header/chat-header.component';
import { MessageInputComponent } from './base-components/message-input/message-input.component';
import { ChannelListComponent } from './base-components/channel-list/channel-list.component';
import { DmListComponent } from './base-components/dm-list/dm-list.component';
import { ChannelListItemComponent } from './base-components/channel-list-item/channel-list-item.component';
import { DmListItemComponent } from './base-components/dm-list-item/dm-list-item.component';
import { SearchResultsHeaderComponent } from './base-components/search-results-header/search-results-header.component';
import { ProfileComponent } from './base-components/profile/profile.component';
import { SearchComponent } from './base-components/search/search.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { DashboardHeaderComponent } from './group-components/dashboard-header/dashboard-header.component';
import { TableRowComponent } from './base-components/table-row/table-row.component';
import { LoaderComponent } from './base-components/loader/loader.component';
// #endregion

// #region Misc
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ApiService } from './services/api-service.service';
import { ChatService } from './services/chat-service.service';
//#endregion

@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    MainWindowComponent,
    NavMenuComponent,
    HeaderComponent,
    SearchResultsComponent,
    ChatComponent,
    ResultComponent,
    MessageComponent,
    ChatHeaderComponent,
    MessageInputComponent,
    ChannelListComponent,
    DmListComponent,
    ChannelListItemComponent,
    DmListItemComponent,
    SearchResultsHeaderComponent,
    ProfileComponent,
    SearchComponent,
    DashboardComponent,
    DashboardHeaderComponent,
    TableRowComponent,
    LoaderComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatIconModule
  ],
  providers: [
    ApiService,
    {
      provide: ChatService
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
