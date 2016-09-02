using System;
using System.Data.Common;

namespace Quarks.DataAccess.Db.ConnectionManagement
{
	public class DbConnectionManager : IDbConnectionManager
	{
		public DbConnectionManager(DbProviderFactory dbProviderFactory, string connectionString)
		{
			if (dbProviderFactory == null) throw new ArgumentNullException(nameof(dbProviderFactory));
			if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

			ProviderFactory = dbProviderFactory;
			ConnectionString = connectionString;
		}

		public DbProviderFactory ProviderFactory { get; }

		public string ConnectionString { get; }

		public DbConnection CreateConnection()
		{
			DbConnection connection = ProviderFactory.CreateConnection();
			connection.ConnectionString = ConnectionString;
			return connection;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (ProviderFactory.GetHashCode()*397) ^ ConnectionString.GetHashCode();
			}
		}
	}
}