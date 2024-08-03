using System.Net.WebSockets;

namespace Pidgin.Services;

public interface IWebSocketService
{
    public Task HandleWebsocketConnection(WebSocket websock);
}
