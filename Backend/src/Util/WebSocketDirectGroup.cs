using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Pidgin.Util;

public class WebSocketDirectGroup : WebSocketChannel
{
    public WebSocketDirectGroup(ChannelEmptyHandler disposal) : base(-1, disposal) { }


    public new async Task Join(int uid, WebSocket ws)
    {
        if (ClientCount >= 2) throw new Exception("More than 2 clients cannot join a direct chat");
        await base.Join(uid, ws);
    }
}
