using System.Data;
using Npgsql;

namespace Pidgin;

public class DashboardService : IDashboardService
{
	private NpgsqlDataSource database;
	public DashboardService(NpgsqlDataSource database)
	{
		this.database = database;
	}

	public async Task<object> GetOrganizationData(DateTime start, DateTime end)
	{
		string sql = @"
			SELECT 
				O.name as Name, 
				COUNT(DISTINCT CASE WHEN U.active THEN U.user_id END) 
					AS ActiveUserCount, 
				Count(M.message_id) 
					AS MessageCount
			FROM organizations O
				INNER JOIN users U 
					ON O.organization_id = U.organization_id
				LEFT JOIN messages M 
					ON M.sender_id = U.user_id
			WHERE M.created_on 
				BETWEEN @StartDate AND @EndDate
			GROUP BY O.name
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("StartDate", start);
			command.Parameters.AddWithValue("EndDate", end);

			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
			{
				if (!reader.HasRows) throw new Exception("No data found");
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
		Console.WriteLine(start);
		Console.WriteLine(end);
        string sql = @"
			SELECT 
				TO_CHAR(M.created_on, 'Month') AS month, 
				TO_CHAR(M.created_on, 'YYYY') AS year,
				Count(*) AS messagesSent,
				RANK() OVER (ORDER BY COUNT(*) DESC) AS rank
			FROM
				messages M
			WHERE 
				M.created_on >= @FirstDate AND M.created_on <= @LastDate
			GROUP BY 
				TO_CHAR(M.created_on, 'Month'),
				TO_CHAR(M.created_on, 'YYYY')
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("FirstDate", start);
			command.Parameters.AddWithValue("LastDate", end);

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
				SUM(CASE WHEN U.active THEN 1 ELSE 0 END)
					AS NumberOfActiveUsers,
				SUM(CASE WHEN NOT U.active THEN 1 ELSE 0 END) 
					AS NumberOfInactiveUsers,
				(SELECT COUNT(*) FROM organizations O WHERE O.active = TRUE) 
					AS NumberOfActiveOrgs,
				(SELECT COUNT(*) FROM organizations O WHERE O.active = TRUE) 
					AS NumberOfInactiveOrgs
			FROM 
				users U;
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
		Console.WriteLine(organizationId);
		Console.WriteLine(start);
		Console.WriteLine(end);
        string sql = @"
			SELECT 
				G.group_id, 
				G.name, 
				COUNT(*) AS MessagesSent,
				(
					SELECT 
						U.first_name || ' ' || U.last_name
					FROM 
						channels C
						INNER JOIN messages M ON C.channel_id = M.channel_id
						INNER JOIN users U ON M.sender_id = U.user_id
					WHERE 
						C.group_id = G.group_id
					GROUP BY 
						U.first_name, U.last_name
					ORDER BY 
						COUNT(*) DESC
					LIMIT 1
				) AS HighestSender
			FROM 
				Groups G
				INNER JOIN channels C ON G.group_id = C.group_id
				INNER JOIN messages M ON C.channel_id = M.channel_id
			WHERE 
				G.organization_id = @OrganizationId 
				AND M.created_on BETWEEN @StartDate AND @EndDate 
				AND G.active = TRUE
			GROUP BY 
				G.group_id, G.name, G.active
			ORDER BY 
				MessagesSent DESC, G.group_id ASC;
		";

		await using (NpgsqlCommand command = database.CreateCommand(sql))
		{
			command.Parameters.AddWithValue("OrganizationId", organizationId);
			command.Parameters.AddWithValue("StartDate", start);
			command.Parameters.AddWithValue("EndDate", end);

			await using (NpgsqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.Default))
			{
				if (!reader.HasRows) throw new Exception("No data found");
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

				return result;
			}
		}
	}
}