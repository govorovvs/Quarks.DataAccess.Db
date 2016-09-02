using System;
using System.Data.Common;
using Quarks.DataAccess.Db.ConnectionManagement;
using AdoNetTransaction = System.Data.Common.DbTransaction;

namespace Quarks.DataAccess.Db
{
	public abstract class DbRepository
	{
		protected DbRepository(IDbConnectionManager connectionManager)
		{
			if (connectionManager == null) throw new ArgumentNullException(nameof(connectionManager));

			ConnectionManager = connectionManager;
		}

		public IDbConnectionManager ConnectionManager { get; }

		protected internal AdoNetTransaction Transaction => DbTransaction.Transaction;

		protected internal DbConnection Connection => Transaction.Connection;

		private DbTransaction DbTransaction
		{
			get { return DbTransaction.GetCurrent(ConnectionManager); }
		}
	}
}