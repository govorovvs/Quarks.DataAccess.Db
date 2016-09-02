using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quarks.DataAccess.Db.ConnectionManagement;
using Quarks.Transactions;
using Assert = NUnit.Framework.Assert;

namespace Quarks.DataAccess.Db.Tests
{
	[TestFixture]
	public class DbTransactionTests
	{
		private CancellationToken _cancellationToken;
		private FakeDbTransaction _transaction;
		private FakeDbConnection _connection;
		private IDbConnectionManager _connectionManager;

		[SetUp]
		public void SetUp()
		{
			_cancellationToken = new CancellationTokenSource().Token;
			_transaction = new FakeDbTransaction();
			_connection = new FakeDbConnection(_transaction);
			_connectionManager = new Mock<IDbConnectionManager>().Object;

			Mock.Get(_connectionManager)
				.Setup(x => x.CreateConnection())
				.Returns(_connection);
		}

	    [Test]
		public void Can_Be_Constructed_With_ConnectionManager()
		{
			DbTransaction transaction = new DbTransaction(_connectionManager);

			Assert.That(transaction.ConnectionManager, Is.EqualTo(_connectionManager));
		}

		[Test]
		public void Is_Instance_Of_IDependentTransaction()
		{
			DbTransaction transaction = CreateTransaction();

			Assert.That(transaction, Is.InstanceOf<IDependentTransaction>());
		}

		[Test]
		public void Transaction_Test()
		{
			DbTransaction transaction = CreateTransaction();

			Assert.That(transaction.Transaction, Is.SameAs(_transaction));
		}

		[Test]
		public void Dispose_Disposes_Transaction()
		{
			DbTransaction transaction = CreateTransaction();

			transaction.Dispose();

			Assert.That(_transaction.IsDisposed, Is.True);
		}

		[Test]
		public void Dispose_Disposes_Connection()
		{
			DbTransaction transaction = CreateTransaction();

			transaction.Dispose();

			Assert.That(_connection.IsDisposed, Is.True);
		}

		[Test]
		public void Can_Be_Disposed_Twice()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();
			transaction.Dispose();
		}

		[Test]
		public async Task Commit_Commits_Transaction()
		{
			var transaction = CreateTransaction();

			await transaction.CommitAsync(_cancellationToken);

			Assert.That(_transaction.IsCommitted, Is.True);
		}

		[Test]
		public void Commit_Throws_An_Exception_If_It_Was_Previously_Disposed()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();

			Assert.ThrowsAsync<ObjectDisposedException>(() => transaction.CommitAsync(_cancellationToken));
		}

		private DbTransaction CreateTransaction()
		{
			var transaction =  new DbTransaction(_connectionManager);

			Assert.That(transaction.Transaction, Is.Not.Null);

			return transaction;
		}
	}
}