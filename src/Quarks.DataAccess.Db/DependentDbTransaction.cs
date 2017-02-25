using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Quarks.Transactions;

namespace Quarks.DataAccess.Db
{
    internal class DependentDbTransaction : IDbTransaction, IDependentTransaction
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private bool _disposed;
        private DbConnection _connection;
        private DbTransaction _transaction;

        public DependentDbTransaction(IDbConnectionFactory connectionFactory)
        {
            if (connectionFactory == null) throw new ArgumentNullException(nameof(connectionFactory));

            _connectionFactory = connectionFactory;
        }

        public DbConnection Connection
        {
            get { return GetOrCreateOpenedConnection(); }
        }

        public DbTransaction Transaction
        {
            get { return GetOrBeginTransaction(); }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _transaction?.Dispose();
            _transaction = null;

            _connection?.Dispose();
            _connection = null;

            _disposed = true;
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            _transaction?.Commit();

            return Task.FromResult(0);
        }

        private DbConnection GetOrCreateOpenedConnection()
        {
            ThrowIfDisposed();

            if (_connection != null)
                return _connection;

            _connection = _connectionFactory.Create();
            _connection.Open();
            return _connection;
        }

        private DbTransaction GetOrBeginTransaction()
        {
            ThrowIfDisposed();

            if (_transaction != null)
                return _transaction;

            _transaction = Connection.BeginTransaction();
            return _transaction;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public static IDbTransaction GetCurrent(IDbConnectionFactory connectionFactory)
        {
            Transaction transaction = Transactions.Transaction.Current;
            string key = connectionFactory.GetKey();

            IDependentTransaction current =
                transaction == null
                    ? new DependentDbTransaction(connectionFactory)
                    : transaction.GetOrEnlist(key, () => new DependentDbTransaction(connectionFactory));
            return (DependentDbTransaction) current;
        }
    }
}
