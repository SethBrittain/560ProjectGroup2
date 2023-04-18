import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LogInComponent } from './pages/log-in/log-in.component';
import { MainWindowComponent } from './pages/main-window/main-window.component';
import { LogInFormComponent } from './group-components/log-in-form/log-in-form.component';
import { NavMenuComponent } from './group-components/nav-menu/nav-menu.component';
import { HeaderComponent } from './group-components/header/header.component';
import { ContentComponent } from './group-components/content/content.component';
import { SearchResultsComponent } from './group-components/search-results/search-results.component';
import { NewMessageComponent } from './group-components/new-message/new-message.component';
import { ChatComponent } from './group-components/chat/chat.component';
import { ResultComponent } from './base-components/result/result.component';
import { UserComponent } from './base-components/user/user.component';
import { MessageComponent } from './base-components/message/message.component';
import { ChatHeaderComponent } from './base-components/chat-header/chat-header.component';
import { MessageInputComponent } from './base-components/message-input/message-input.component';
import { ChannelListComponent } from './base-components/channel-list/channel-list.component';
import { DmListComponent } from './base-components/dm-list/dm-list.component';
import { ChannelListItemComponent } from './base-components/channel-list-item/channel-list-item.component';

@NgModule({
  declarations: [
    AppComponent,
    LogInComponent,
    MainWindowComponent,
    LogInFormComponent,
    NavMenuComponent,
    HeaderComponent,
    ContentComponent,
    SearchResultsComponent,
    NewMessageComponent,
    ChatComponent,
    ResultComponent,
    UserComponent,
    MessageComponent,
    ChatHeaderComponent,
    MessageInputComponent,
    ChannelListComponent,
    DmListComponent,
    ChannelListItemComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatIconModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
