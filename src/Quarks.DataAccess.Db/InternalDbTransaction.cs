﻿using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Quarks.DataAccess.Db.ConnectionManagement;
using Quarks.Transactions;

namespace Quarks.DataAccess.Db
{
	internal sealed class InternalDbTransaction : IDependentTransaction
	{
		private static readonly object StaticLock = new object();
		private readonly object _lock = new object();
		private bool _disposed;
		private volatile DbTransaction _transaction;
		private volatile DbConnection _connection;

		public InternalDbTransaction(IDbConnectionManager connectionManager)
		{
			ConnectionManager = connectionManager;
		}

		public DbTransaction Transaction
		{
		    get { return GetOrCreateTransaction(); }
		}

	    public IDbConnectionManager ConnectionManager { get; }

	    internal DbConnection CreateOpenedConnection()
	    {
            DbConnection connection = ConnectionManager.CreateConnection();
            connection.Open();
	        return connection;
	    }

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

		private DbTransaction GetOrCreateTransaction()
		{
			ThrowIfDisposed();

			if (_transaction == null)
			{
				lock (_lock)
				{
					if (_transaction == null)
					{
					    _connection = CreateOpenedConnection();
                        _transaction = _connection.BeginTransaction();
					}
				}
			}

			return _transaction;
		}

		public static InternalDbTransaction GetCurrent(IDbConnectionManager connectionManager)
		{
			var transaction = Transactions.Transaction.Current;
			if (transaction == null)
			{
				return new InternalDbTransaction(connectionManager);
			}

			string key = 
                $"{typeof(InternalDbTransaction).Name}_{connectionManager.GetHashCode()}";

			IDependentTransaction current;
			if (!transaction.DependentTransactions.TryGetValue(key, out current))
			{
				lock (StaticLock)
				{
					if (!transaction.DependentTransactions.TryGetValue(key, out current))
					{
						current = new InternalDbTransaction(connectionManager);
						transaction.Enlist(key, current);
					}
				}
			}

			return (InternalDbTransaction)current;
		}
	}
}
