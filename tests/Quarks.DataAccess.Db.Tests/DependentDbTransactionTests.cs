using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quarks.Transactions;

namespace Quarks.DataAccess.Db.Tests
{
	[TestFixture]
	public class DependentDbTransactionTests
    {
		private CancellationToken _cancellationToken;
		private FakeDbTransaction _dbTransaction;
		private FakeDbConnection _dbConnection;
		private Mock<IDbConnectionFactory> _mockConnectionFactory;
        private DependentDbTransaction _transaction;

        [SetUp]
		public void SetUp()
		{
			_cancellationToken = new CancellationTokenSource().Token;
			_dbTransaction = new FakeDbTransaction();
			_dbConnection = new FakeDbConnection(_dbTransaction);
            _mockConnectionFactory = new Mock<IDbConnectionFactory>();

			_mockConnectionFactory
				.Setup(x => x.Create())
				.Returns(_dbConnection);
            _transaction = new DependentDbTransaction(_mockConnectionFactory.Object);
		}

		[Test]
		public void Is_Instance_Of_IDependentTransaction()
		{
			var transaction = CreateTransaction();

			Assert.That(transaction, Is.InstanceOf<IDependentTransaction>());
		}

		[Test]
		public void Transaction_Test()
		{
			var transaction = CreateTransaction();

			Assert.That(transaction.Transaction, Is.SameAs(_dbTransaction));
		}

		[Test]
		public void Dispose_Disposes_Transaction()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();

			Assert.That(_dbTransaction.IsDisposed, Is.True);
		}

		[Test]
		public void Dispose_Disposes_Connection()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();

			Assert.That(_dbConnection.IsDisposed, Is.True);
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

			Assert.That(_dbTransaction.IsCommitted, Is.True);
		}

		[Test]
		public void Commit_Throws_An_Exception_If_It_Was_Previously_Disposed()
		{
			var transaction = CreateTransaction();

			transaction.Dispose();

			Assert.ThrowsAsync<ObjectDisposedException>(() => transaction.CommitAsync(_cancellationToken));
		}

		private DependentDbTransaction CreateTransaction()
		{
			var transaction = new DependentDbTransaction(_mockConnectionFactory.Object);

			Assert.That(transaction.Transaction, Is.Not.Null);

			return transaction;
		}
	}
}