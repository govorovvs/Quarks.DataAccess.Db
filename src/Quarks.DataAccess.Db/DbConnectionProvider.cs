using System.Data.Common;

namespace Quarks.DataAccess.Db
{
    public class DbConnectionProvider : IDbConnectionProvider
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DbConnectionProvider(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private IDbTransaction DbTransaction
        {
            get {  return DependentDbTransaction.GetCurrent(_connectionFactory);}
        }

        public DbConnection Connection
        {
            get { return DbTransaction.Connection; }
        }

        public DbTransaction Transaction
        {
            get { return DbTransaction.Transaction; }
        }
    }
}