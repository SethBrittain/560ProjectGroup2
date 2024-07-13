using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Npgsql;

namespace Pidgin.Modules.Channels;

public class Channel
{
    /// <summary>
    /// The unique identifier for the channel.
    /// </summary>
    public int channelId { get; private set; }

    /// <summary>
    /// The unique identifier for the group the channel belongs to.
    /// </summary>
    public int groupId { get; private set; }

    /// <summary>
    /// The name of the channel.
    /// </summary>
    public string name { get; private set; }

    /// <summary>
    /// The datetime that the channel was created on.
    /// </summary>
    public DateTime createdOn { get; private set; }

    /// <summary>
    /// The datetime that the channel was last updated on.
    /// </summary>
    public DateTime updatedOn { get; private set; }


    public Channel(int channelId, int groupId, string name, DateTime createdOn, DateTime updatedOn)
    {
        this.channelId = channelId;
        this.groupId = groupId;
        this.name = name;
        this.createdOn = createdOn;
        this.updatedOn = updatedOn;
    }
}