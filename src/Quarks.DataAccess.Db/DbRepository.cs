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

		protected internal DbTransaction Transaction
		{
		    get { return DbTransaction.Transaction; }
		}

	    protected internal DbConnection Connection
	    {
	        get { return Transaction.Connection; }
	    }

	    [Obsolete("Use DbConnection extension methods")]
	    protected Task<int> ExecuteAsync(QueryObject queryObject, CancellationToken cancellationToken, int? commandTimeout = null)
	    {
	        CommandDefinition commandDefinition = 
                new CommandDefinition(queryObject.Text, queryObject.Parameters, Transaction, commandTimeout, queryObject.CommandType, cancellationToken:cancellationToken);

	        return Connection.ExecuteAsync(commandDefinition);
	    }

        [Obsolete("Use DbConnection extension methods")]
        protected Task<IEnumerable<dynamic>> QueryAsync(QueryObject queryObject, CancellationToken cancellationToken, int? commandTimeout = null)
        {
            CommandDefinition commandDefinition =
                new CommandDefinition(queryObject.Text, queryObject.Parameters, Transaction, commandTimeout, queryObject.CommandType, cancellationToken: cancellationToken);

            return Connection.QueryAsync(commandDefinition);
        }

        [Obsolete("Use DbConnection extension methods")]
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