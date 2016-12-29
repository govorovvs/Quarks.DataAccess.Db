using System.Data;
using System.Data.Common;

namespace Quarks.DataAccess.Db.Tests
{
	public class FakeDbConnection : DbConnection
	{
		private readonly DbTransaction _transaction;

		public FakeDbConnection(DbTransaction transaction)
		{
			_transaction = transaction;
		}

		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
		{
			return _transaction;
		}

		public override void Close()
		{
		}

		public override void ChangeDatabase(string databaseName)
		{
		}

		public override void Open()
		{
		}

		public override string ConnectionString { get; set; }
		public override string Database { get; }
		public override ConnectionState State { get; }
		public override string DataSource { get; }
		public override string ServerVersion { get; }
		public bool IsDisposed { get; private set; }

		protected override DbCommand CreateDbCommand()
		{
			throw new System.NotImplementedException();
		}

		protected override void Dispose(bool disposing)
		{
			IsDisposed = true;
		}
	}
}