using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Npgsql;

namespace Pidgin.Model;

public class Organization
{
    /// <summary>
    /// The unique identifier for the organization.
    /// </summary>
    public int organizationId { get; set; }

	/// <summary>
	/// The name of the organization.
	/// </summary>
	public string name { get; set; }

	/// <summary>
	/// Whether or not the organization is active.
	/// </summary>
	public bool active { get; set; }

    /// <summary>
    /// The date the message was created.
    /// </summary>
    public DateTime createdOn { get; set; }

    /// <summary>
    /// The date the message was last updated.
    /// </summary>
    public DateTime updatedOn { get; set; }

	/// <summary>
	/// Constructor for an organization
	/// </summary>
	public Organization(int organizationId, string name, bool active, DateTime createdOn, DateTime updatedOn)
	{
		this.organizationId = organizationId;
		this.name = name;
		this.active = active;
		this.createdOn = createdOn;
		this.updatedOn = updatedOn;
	}
}