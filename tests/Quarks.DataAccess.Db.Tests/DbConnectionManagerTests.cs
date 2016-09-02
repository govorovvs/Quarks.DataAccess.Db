using System.Data.Common;
using System.Data.SqlClient;
using NUnit.Framework;
using Quarks.DataAccess.Db.ConnectionManagement;

namespace Quarks.DataAccess.Db.Tests
{
	[TestFixture]
    public class DbConnectionManagerTests
	{
		[Test]
        public void Can_be_Constructed_With_ProviderFactory_And_ConnectionString()
        {
	        const string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
	        DbProviderFactory providerFactory = SqlClientFactory.Instance;

			DbConnectionManager connectionManager = 
				new DbConnectionManager(providerFactory, connectionString);

			Assert.That(connectionManager.ProviderFactory, Is.EqualTo(providerFactory));
			Assert.That(connectionManager.ConnectionString, Is.EqualTo(connectionString));
		}

		[Test]
		public void CreateConnection_Creates_One()
		{
			const string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
			SqlClientFactory providerFactory = SqlClientFactory.Instance;

			DbConnectionManager connectionManager =
				new DbConnectionManager(providerFactory, connectionString);

			DbConnection connection = connectionManager.CreateConnection();

			Assert.That(connection, Is.InstanceOf<SqlConnection>());
			Assert.That(connection.ConnectionString, Is.EqualTo(connectionString));
		}

		[Test]
		public void GetHashCode_Equality()
		{
			const string connectionString1 = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;1";
			const string connectionString2 = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;2";
			SqlClientFactory providerFactory = SqlClientFactory.Instance;

			DbConnectionManager connectionManager1 =
				new DbConnectionManager(providerFactory, connectionString1);
			DbConnectionManager connectionManager2 =
				new DbConnectionManager(providerFactory, connectionString1);
			DbConnectionManager connectionManager3 =
				new DbConnectionManager(providerFactory, connectionString2);

			Assert.That(connectionManager1.GetHashCode(), Is.EqualTo(connectionManager2.GetHashCode()));
			Assert.That(connectionManager1.GetHashCode(), Is.Not.EqualTo(connectionManager3.GetHashCode()));
		}
	}
}
