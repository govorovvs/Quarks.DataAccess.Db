using System;
using System.Data.Common;

namespace Quarks.DataAccess.Db
{
    public abstract class DbRepository
	{
	    private readonly IDbConnectionProvider _connectionProvider;

	    protected DbRepository(IDbConnectionProvider connectionProvider)
	    {
            if (connectionProvider == null) throw new ArgumentNullException(nameof(connectionProvider));

            _connectionProvider = connectionProvider;
	    }

		protected internal DbTransaction Transaction
		{
		    get { return _connectionProvider.Transaction; }
		}

	    protected internal DbConnection Connection
	    {
	        get { return _connectionProvider.Connection; }
	    }
	}
}