using System.Text.Json;
using Npgsql;

namespace pidgin.models;

public class User
{
    /// <summary>
    /// The unique identifier for the user.
    /// </summary>
    private int id { get; }

    /// <summary>
    /// The unique identifier for the organization the user belongs to.
    /// </summary>
    private int organizationId { get; }

    /// <summary>
    /// Unique email address for the user.
    /// </summary>
    private string email { get; }

    /// <summary>
    /// The user's first name.
    /// </summary>
    private string firstName { get; }

    /// <summary>
    /// The user's last name.
    /// </summary>
    private string lastName { get; }

    /// <summary>
    /// The title of the user
    /// </summary>
    private string title { get; }

    /// <summary>
    /// Whether the user is active or not. Used for soft deletion.
    /// </summary>
    private bool active { get; }

    /// <summary>
    /// The URL of the user's profile picture.
    /// </summary>
    private string profilePhotoUrl { get; }

    /// <summary>
    /// The date the user was created.
    /// </summary>
    private DateTime createdOn { get; }

    /// <summary>
    /// The date the user was last updated.
    /// </summary>
    private DateTime updatedOn { get; }

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

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}