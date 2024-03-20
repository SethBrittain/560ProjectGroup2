using pidgin.models;
using pidgin.services;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Pidgin.Services;

public class WebsocketService : IWebSocketService
{
    private IMessageService messageService;

    /// <summary>
    /// Key: User id
    /// Value: Channel ID
    /// </summary>
    private Dictionary<int, List<WebSocket>> usersInChannel = new();

    public WebsocketService(IMessageService messageService)
    {
        this.messageService = messageService;
    }

    public async Task HandleWebsocketChannelConnection(WebSocket websock, int channelId)
    {
        // Message buffer
        byte[] buffer = new byte[1024 * 4];

        while (websock.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await websock.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            switch (result.MessageType)
            {
                case WebSocketMessageType.Text:
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Message
                    await websock.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
                    break;
                case WebSocketMessageType.Binary:
                    break;
                case WebSocketMessageType.Close:
                    break;
                default:
                    break;
            }
        }
    }

    public async Task HandleWebsocketDirectConnection(WebSocket websock, int recipientId)
    {
        // Message buffer
        byte[] buffer = new byte[1024 * 4];

        while (websock.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await websock.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            switch (result.MessageType)
            {
                case WebSocketMessageType.Text:
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    JsonSerializer.Serialize<Message>(message);
                    await websock.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
                    break;
                case WebSocketMessageType.Binary:
                    break;
                case WebSocketMessageType.Close:
                    break;
                default:
                    break;
            }
        }
    }
}