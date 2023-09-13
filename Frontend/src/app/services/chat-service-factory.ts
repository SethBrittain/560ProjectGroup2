import { ChatService, ChatConfig } from './chat-service.service';

export function chatServiceFactory() {
  const rxStomp = new ChatService();
  rxStomp.configure(ChatConfig);
  rxStomp.activate();
  return rxStomp;
}
