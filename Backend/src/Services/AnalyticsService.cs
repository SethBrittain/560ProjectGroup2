using System.Data;
using Npgsql;
using Pidgin.Exceptions;
using Pidgin.Services;

namespace Pidgin;

public class AnalyticsService : IAnalyticsService
{
	private NpgsqlDataSource database;
	public AnalyticsService(NpgsqlDataSource database)
	{
		this.database = database;
	}

	public async Task<object> GetOrganizationData(int uid, DateTime start, DateTime end)
	{
		string sql = @"
			SELECT
				o.name as organization_name,
				(SELECT COUNT(CASE WHEN active=TRUE THEN 1 ELSE NULL END) FROM users WHERE organization_id=(
					(SELECT organization_id FROM users WHERE user_id=@uid)
				)) as active_users,
				COUNT(DISTINCT cm.channel_message_id) +
				COUNT(DISTINCT dm.direct_message_id) as message_count
				
			FROM
				users u
			LEFT JOIN channel_messages cm
				ON cm.sender_id=u.user_id AND cm.created_on BETWEEN @startDate AND @endDate
			LEFT JOIN direct_messages dm
				ON dm.sender_id=u.user_id AND dm.created_on BETWEEN @startDate AND @endDate
			LEFT JOIN organizations o
				ON o.organization_id=u.organization_id
			WHERE u.organization_id=(SELECT organization_id FROM users WHERE user_id=@uid)
			GROUP BY o.name;
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("startDate", start);
			command.Parameters.AddWithValue("endDate", end);
			command.Parameters.AddWithValue("uid", uid);

			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
			{
				if (!reader.HasRows)
					throw new DataNotFoundException("No data found");
				await reader.ReadAsync();
				return new
				{
					name = reader.GetString(0),
					activeUserCount = reader.GetInt32(1),
					messageCount = reader.GetInt32(2)
				};
			}
		}
	}

	public async Task<List<object>> GetMonthlyTraffic(DateTime start, DateTime end)
	{
        string sql = @"
			SELECT
				un.month AS month,
				un.year AS year,
				SUM(un.message_count) AS messages_sent,
				RANK() OVER (ORDER BY SUM(un.message_count) DESC) AS rank
			FROM
			((SELECT
				COUNT(dm.*) AS message_count,
				TO_CHAR(dm.created_on, 'Month') AS month,
				TO_CHAR(dm.created_on, 'YYYY') AS year
			FROM direct_messages dm
			WHERE dm.created_on BETWEEN '0001-01-01' AND '2222-02-02'
			GROUP BY 
				TO_CHAR(dm.created_on, 'Month'),
				TO_CHAR(dm.created_on, 'YYYY'))
			UNION
			(SELECT
				COUNT(cm.*) AS message_count,
				TO_CHAR(cm.created_on, 'Month') AS month,
				TO_CHAR(cm.created_on, 'YYYY') AS year
			FROM channel_messages cm
			WHERE cm.created_on BETWEEN '0001-01-01' AND '2222-02-02'
			GROUP BY 
				TO_CHAR(cm.created_on, 'Month'),
				TO_CHAR(cm.created_on, 'YYYY')) 
			) un
			GROUP BY un.month, un.year
			ORDER BY rank ASC;
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("startDate", start);
			command.Parameters.AddWithValue("endDate", end);

			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.Default))
			{
				if (!reader.HasRows) throw new Exception("No data found");

				List<object> result = new List<object>();
				while (await reader.ReadAsync())
				{
                    result.Add(new
					{
						Month = reader.GetString(0),
						Year = reader.GetString(1),
						MessagesSent = reader.GetInt32(2),
						Rank = reader.GetInt32(3)
					});
				}

				return result;
			}
		}
	}

	public async Task<object> GetAppGrowth()
	{
		string sql = @"
			SELECT
				COUNT(CASE WHEN u.active=TRUE THEN 1 ELSE NULL END) as active_users,
				COUNT(CASE WHEN u.active=FALSE THEN 1 ELSE NULL END) as inactive_users,
				(SELECT COUNT(o.*) FROM organizations o WHERE o.active=TRUE) as active_organization_count,
				(SELECT COUNT(o.*) FROM organizations o WHERE o.active=FALSE) as inactive_organization_count
			FROM users u;
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
			{
				if (!reader.HasRows) throw new Exception("No data found");
				await reader.ReadAsync();
				return new {
					NumberOfActiveUsers = reader.GetInt32(0),
					NumberOfInactiveUsers = reader.GetInt32(1),
					NumberOfActiveOrgs = reader.GetInt32(2),
					NumberOfInactiveOrgs = reader.GetInt32(3)
				};
			}
		}
	}

	public async Task<List<object>> GetGroupActivity(int organizationId, DateTime start, DateTime end)
	{
        string sql = @"
			SELECT 
				c.group_id, 
				c.name, 
				COUNT(cm.channel_message_id) as message_count,
				u.first_name || ' ' || u.last_name as highest_sender 
			FROM 
				channel_messages cm 
			LEFT JOIN 
				channels c ON c.channel_id=cm.channel_id
			LEFT JOIN 
				users u ON cm.sender_id=u.user_id
			LEFT JOIN 
				groups g ON g.group_id=c.group_id
			WHERE u.organization_id=@oid
				AND cm.created_on BETWEEN @startDate AND @endDate
			GROUP BY 
				c.group_id, c.name, cm.sender_id, u.first_name, u.last_name
			ORDER BY 
				message_count DESC
			LIMIT 5;
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("oid", organizationId);
			command.Parameters.AddWithValue("startDate", start);
			command.Parameters.AddWithValue("endDate", end);

			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.Default))
			{
				List<object> result = new List<object>();

                while (await reader.ReadAsync())
				{
					result.Add(new {
						GroupId = reader.GetInt32(0),
						Name = reader.GetString(1),
						MessagesSent = reader.GetInt32(2),
						HighestSender = reader.GetString(3)
					});
				}
				
				// if (result.Count < 1) throw new Exception("No data found");

				return result;
			}
		}
	}
}