using Microsoft.AspNetCore.Authorization;
using Npgsql;
using pidgin.services;
using Pidgin.Util;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Numerics;
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
    /// 
    /// </summary>
    private Dictionary<WebSocketDirectKey, WebSocketDirectGroup> _directMessages = new();

    /// <summary>
    /// Maximum size a message can be before it is truncated
    /// </summary>
    private const int MAX_MESSAGE_SIZE = 4096 * 4;


    private readonly IMessageService _messageService;

    public WebSocketManager(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task HandleConnection(int uid, string type, int chatId, WebSocket ws)
    {
        if (type == "channel")
            await HandleChannelConnection(uid, chatId, ws);
        else
            await HandleDirectConnection(uid, chatId, ws);

        // Receive buffer for messages
        ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MAX_MESSAGE_SIZE]);

        // Result of receiving message
        WebSocketReceiveResult result = null;

        using (var ms = new MemoryStream())
        {
            result = await ws.ReceiveAsync(buffer, CancellationToken.None);
            ms.Write(buffer.Array, buffer.Offset, result.Count);

            ms.Seek(0, SeekOrigin.Begin);

            if (result.MessageType == WebSocketMessageType.Text)
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


    private async Task HandleChannelConnection(int uid, int chatId, WebSocket ws)
    {
        if (!_channels.ContainsKey(chatId))
            _channels[chatId] = new WebSocketChannel(chatId, async () => _channels.Remove(chatId));

        await _channels[chatId].Join(uid, ws);
    }

    private async Task HandleDirectConnection(int uid, int chatId, WebSocket ws)
    {
        if (!_directMessages.ContainsKey(new WebSocketDirectKey(uid, chatId)))
            _directMessages[new WebSocketDirectKey(uid, chatId)] = new WebSocketDirectGroup(async () => _directMessages.Remove(new WebSocketDirectKey(uid, chatId)));

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
            result = await ws.ReceiveAsync(buffer, CancellationToken.None);
            ms.Write(buffer.Array, buffer.Offset, result.Count);

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