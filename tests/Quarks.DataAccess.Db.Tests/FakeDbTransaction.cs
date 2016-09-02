using System.Data;
using System.Data.Common;

namespace Quarks.DataAccess.Db.Tests
{
	public class FakeDbTransaction : System.Data.Common.DbTransaction
	{
		public override void Commit()
		{
			IsCommitted = true;
		}

		public override void Rollback()
		{
		}

		protected override void Dispose(bool disposing)
		{
			IsDisposed = true;
		}

		protected override DbConnection DbConnection { get; }
		public override IsolationLevel IsolationLevel { get; }

		public bool IsCommitted { get; private set; }

		public bool IsDisposed { get; private set; }
	}
}