using System.Data.Common;

namespace Quarks.DataAccess.Db
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DbProviderFactory _providerFactory;
        private readonly string _connectionString;

        public DbConnectionFactory(DbProviderFactory providerFactory, string connectionString)
        {
            _providerFactory = providerFactory;
            _connectionString = connectionString;
        }

        public DbConnection Create()
        {
            var connection = _providerFactory.CreateConnection();
            connection.ConnectionString = _connectionString;
            return connection;
        }

        public string GetKey()
        {
            return $"{_providerFactory.GetHashCode()}_{_connectionString.GetHashCode()}";
        }
    }
}