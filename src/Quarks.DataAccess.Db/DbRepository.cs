using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Quarks.DataAccess.Db.ConnectionManagement;

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

		protected internal DbTransaction Transaction => DbTransaction.Transaction;

		protected internal DbConnection Connection => Transaction.Connection;

	    protected Task<int> ExecuteAsync(QueryObject queryObject, CancellationToken cancellationToken, int? commandTimeout = null)
	    {
	        CommandDefinition commandDefinition = 
                new CommandDefinition(queryObject.Text, queryObject.Parameters, Transaction, commandTimeout, queryObject.CommandType, cancellationToken:cancellationToken);

	        return Connection.ExecuteAsync(commandDefinition);
	    }

        protected Task<IEnumerable<dynamic>> QueryAsync(QueryObject queryObject, CancellationToken cancellationToken, int? commandTimeout = null)
        {
            CommandDefinition commandDefinition =
                new CommandDefinition(queryObject.Text, queryObject.Parameters, Transaction, commandTimeout, queryObject.CommandType, cancellationToken: cancellationToken);

            return Connection.QueryAsync(commandDefinition);
        }

        protected Task<IEnumerable<T>> QueryAsync<T>(QueryObject queryObject, CancellationToken cancellationToken, int? commandTimeout = null)
	    {
            CommandDefinition commandDefinition =
                new CommandDefinition(queryObject.Text, queryObject.Parameters, Transaction, commandTimeout, queryObject.CommandType, cancellationToken: cancellationToken);

            return Connection.QueryAsync<T>(commandDefinition);
        }

        private InternalDbTransaction DbTransaction
		{
			get { return InternalDbTransaction.GetCurrent(ConnectionManager); }
		}
	}
}