using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Npgsql;
using Npgsql.Replication;

namespace pidgin.models;

public class User
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    public int id { get; private set;}

    /// <summary>
    /// The unique identifier for the organization the user belongs to.
    /// </summary>
    public int organizationId { get; set;}

    /// <summary>
    /// Unique email address for the user.
    /// </summary>
    public string email { get; private set;}

    /// <summary>
    /// The user's first name.
    /// </summary>
    public string firstName { get; set;}

    /// <summary>
    /// The user's last name.
    /// </summary>
    public string lastName { get; set;}

    /// <summary>
    /// The title of the user
    /// </summary>
    public string title { get; set;}

    /// <summary>
    /// Whether the user is active or not. Used for soft deletion.
    /// </summary>
    public bool active { get; set;}

    /// <summary>
    /// The URL of the user's profile picture.
    /// </summary>
    public string profilePhotoUrl { get; set;}

    /// <summary>
    /// The date the user was created.
    /// </summary>
    public DateTime createdOn { get; set;}

    /// <summary>
    /// The date the user was last updated.
    /// </summary>
    public DateTime updatedOn { get; set;}

	public User(int id, int organizationId, string email, string firstName, string lastName, string title, bool active, string profilePhotoUrl, DateTime createdOn, DateTime updatedOn)
	{
        this.id = id;
        this.organizationId = organizationId;
        this.email = email;
        this.firstName = firstName;
        this.lastName = lastName;
        this.title = title;
        this.active = active;
        this.profilePhotoUrl = profilePhotoUrl;
        this.createdOn = createdOn;
        this.updatedOn = updatedOn;
    }
}