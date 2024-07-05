using Microsoft.AspNetCore.Authorization;
using Npgsql;
using pidgin.services;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Pidgin.Services;

/// <summary>
/// Class for managing WebSocket connections
/// </summary>
public class WebSocketManager
{
    /// <summary>
    /// Data structure for containing WebSocket channels
    /// </summary>
    private Dictionary<int, WebSocketChannel> _channels = new();

    /// <summary>
    /// Maximum size a message can be before it is truncated
    /// </summary>
    private const int MAX_MESSAGE_SIZE = 4096 * 4;


    private readonly IMessageService _messageService;

    public WebSocketManager(IMessageService messageService)
    {
        _messageService = messageService;
    }

    /// <summary>
    /// Main method for handling messages from clients.
    /// Socket connections are divided into channels, each with a unique identifier.
    /// </summary>
    /// <param name="channelId">The channel unique identifier</param>
    /// <param name="uid">The user unique identifier</param>
    /// <param name="ws">The connection to set up message handling on</param>
    /// <returns></returns>
    public async Task HandleMessages(int channelId, int uid, WebSocket ws)
    {
        // Determine if channel exists, if not create it
        if (!_channels.ContainsKey(channelId))
            _channels[channelId] = new WebSocketChannel(channelId, async () => _channels.Remove(channelId));

        // Add client to channel
        await _channels[channelId].Join(uid, ws);

        // Receive buffer for messages
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MAX_MESSAGE_SIZE]);

        // Result of receiving message
        WebSocketReceiveResult result = null;

        using (var ms = new MemoryStream())
        {
            do
            {
                result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                ms.Write(buffer.Array, buffer.Offset, result.Count);
            }
            while (!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);

            if (result.MessageType == WebSocketMessageType.Text)
            {

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    string message = await reader.ReadToEndAsync();
                    SendableMessage messageToSend = await _messageService.CreateChannelMessageReturningSendable(new pidgin.models.Message(
                        -1,
                        uid,
                        channelId,
                        null,
                        message,
                        false
                    ));
                    await _channels[channelId].Broadcast(messageToSend);
                }
            }
        }
        // Remove client from channel
        //await _channels[channelId].Disconnect(uid);
    }
}