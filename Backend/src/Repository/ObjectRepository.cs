using Npgsql;

namespace Pidgin.Repository;

public abstract class ObjectRepository
{
	protected NpgsqlDataSource _dataSource;

	public ObjectRepository(NpgsqlDataSource ds) => _dataSource = ds;
}