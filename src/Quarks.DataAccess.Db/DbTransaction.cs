using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Quarks.DataAccess.Db.ConnectionManagement;
using Quarks.Transactions;
using AdoNetTransaction = System.Data.Common.DbTransaction;

namespace Quarks.DataAccess.Db
{
	internal sealed class DbTransaction : IDependentTransaction
	{
		private static readonly object StaticLock = new object();
		private readonly object _lock = new object();
		private bool _disposed;
		private volatile AdoNetTransaction _transaction;
		private volatile DbConnection _connection;

		public DbTransaction(IDbConnectionManager connectionManager)
		{
			ConnectionManager = connectionManager;
		}

		public AdoNetTransaction Transaction => GetOrCreateTransaction();

		public IDbConnectionManager ConnectionManager { get; }

		public void Dispose()
		{
			if (_disposed)
				return;

			_transaction?.Dispose();
			_connection?.Dispose();

			_disposed = true;
		}

		public Task CommitAsync(CancellationToken cancellationToken)
		{
			ThrowIfDisposed();

			_transaction?.Commit();

			return Task.FromResult(0);
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		private AdoNetTransaction GetOrCreateTransaction()
		{
			ThrowIfDisposed();

			if (_transaction == null)
			{
				lock (_lock)
				{
					if (_transaction == null)
					{
						_connection = ConnectionManager.CreateConnection();
						_connection.Open();
						_transaction = _connection.BeginTransaction();
					}
				}
			}

			return _transaction;
		}

		public static DbTransaction GetCurrent(IDbConnectionManager connectionManager)
		{
			var transaction = Transactions.Transaction.Current;
			if (transaction == null)
			{
				return new DbTransaction(connectionManager);
			}

			string key = connectionManager.GetHashCode().ToString();

			IDependentTransaction current;
			if (!transaction.DependentTransactions.TryGetValue(key, out current))
			{
				lock (StaticLock)
				{
					if (!transaction.DependentTransactions.TryGetValue(key, out current))
					{
						current = new DbTransaction(connectionManager);
						transaction.Enlist(key, current);
					}
				}
			}

			return (DbTransaction)current;
		}
	}
}
