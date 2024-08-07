﻿using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Pidgin.Model;

namespace Pidgin.Util;

/// <summary>
/// Class for handling WebSocket connections and messages
/// </summary>
public class WebSocketChannel
{
    /// <summary>
    /// Unique identifier for this channel
    /// </summary>
    public int ChannelId { get; set; }

    /// <summary>
    /// Event handler for when the channel is emptied after all clients disconnect
    /// </summary>
    public delegate void ChannelEmptyHandler();
    public ChannelEmptyHandler _disposal;


    /// <summary>
    /// Clients connected to this channel
    /// </summary>
    private Dictionary<int, WebSocket> _clients = new();

    /// <summary>
    /// Number of clients connected to this channel
    /// </summary>
    public int ClientCount => _clients.Values.Count;

    /// <summary>
    /// Constructor for new WebSocketChannel
    /// </summary>
    /// <param name="channelId">The unique identifier of channel</param>
    public WebSocketChannel(int channelId, ChannelEmptyHandler disposal)
    {
        ChannelId = channelId;
        _disposal = disposal;
    }

    /// <summary>
    /// Broadcast a message to all clients in the channel
    /// </summary>
    /// <param name="message">Message to broadcast. Has known max length from manager's const value</param>
    /// <returns></returns>
    public async Task Broadcast(ChannelMessage cm)
    {		
        foreach (KeyValuePair<int, WebSocket> pair in _clients)
        {
            await pair.Value.SendAsync(
				new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cm))),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }
    }

    /// <summary>
    /// Add a client to the channel
    /// </summary>
    /// <param name="uid">The unique identifier of the user joining the channel</param>
    /// <param name="ws">Websocket connection to join the channel</param>
    /// <returns></returns>
    public async Task Join(int uid, WebSocket ws)
    {
        if (_clients.ContainsKey(uid)) return;
        else _clients[uid] = ws;
    }

    /// <summary>
    /// Removes a client from the channel and closes the connection
    /// </summary>
    /// <param name="uid">User to remove</param>
    /// <returns></returns>
    public async Task Disconnect(int uid)
    {
        if (_clients.ContainsKey(uid))
        {
            try { await _clients[uid].CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None); }
			catch (Exception e) { Console.WriteLine(e.Message); Console.WriteLine(e.StackTrace); }
            _clients.Remove(uid);
            if (ClientCount == 0) _disposal.Invoke();
        }
    }
}
