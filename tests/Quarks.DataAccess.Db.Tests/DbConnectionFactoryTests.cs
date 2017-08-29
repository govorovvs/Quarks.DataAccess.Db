using System.Data.Common;
using System.Data.SqlClient;
using Moq;
using NUnit.Framework;

namespace Quarks.DataAccess.Db.Tests
{
    [TestFixture]
    public class DbConnectionFactoryTests
    {
        private DbProviderFactory _providerFactory;
        private string _connectionString;
        private DbConnectionFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _connectionString =
                "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
            _providerFactory = SqlClientFactory.Instance;

            _factory = new DbConnectionFactory(_providerFactory, _connectionString);
        }

        [Test]
        public void Creates_Connection()
        {
            var connection = _factory.Create();

            Assert.That(connection, Is.Not.Null);
            Assert.That(connection, Is.InstanceOf<SqlConnection>());
            Assert.That(connection.ConnectionString, Is.EqualTo(_connectionString));
        }

        [Test]
        public void Returns_Key()
        {
            var key = _factory.GetKey();

            string expected = $@"{_providerFactory.GetHashCode()}_{_connectionString.GetHashCode()}";
            Assert.That(key, Is.EqualTo(expected));
        }
    }
}