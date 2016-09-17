# Quarks.DataAccess.Db

[![Version](https://img.shields.io/nuget/v/Quarks.DataAccess.Db.svg)](https://www.nuget.org/packages/Quarks.DataAccess.Db)

## Example

Here is an example that describes how to use Dapper with Quarks.Transactions.

```csharp
public class User : IEntity, IAggregate
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class UserManagementDbConnectionManager(string connectionString) : IDbConnectionManager
{
    private readonly string _connectionString = connectionString;

    public DbConnection CreateConnection() => new SqlConnection(_connectionString);

    public int GetHashCode() => _connectionString.GetHashCode();
}

public class DbUserRepository : DbRepository, IUserRepository
{
    public DbUserRepository(UserManagementDbConnectionManager connectionManager) : base(connectionManager)
    {
    }

    public async Task<User> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        QueryObject query = 
            new QueryObject("SELECT Id, Name FROM Users WHERE Id = @Id", new { Id = id });
        var result = await QueryAsync<User>(query, cancellationToken);
        return result.SingleOrDefault();
    }

    public Task ModifyAsync(User user, CancellationToken cancellationToken)
    {
        QueryObject query =
            new QueryObject("UPDATE Users SET Name = @Name WHERE Id = @Id", new { Id = user.Id, Name = user.Name });
        return ExecuteAsync(query, cancellationToken);
    }
}

public class RenameUserCommandHandler : ICommandHandler<RenameUserCommand>
{
	private readonly IUserRepository _userRepository;

	public async Task HandleAsync(RenameUserCommand command, CancellationToken ct)
	{
		using(ITransaction transaction = Transaction.BeginTransaction())
		{
			User user = await _userRepository.FindByIdAsync(command.Id, ct);
			user.Name = command.Name;
			await _userRepository.ModifyAsync(user, ct);
			await transaction.CommitAsync(ct);
		}
	}
}
```

## How it works

*DbRepository* internally uses *DbTransaction* and gets it from the current *Quarks.Transaction*.

```csharp
public abstract class DbRepository(IDbConnectionManager connectionManager)
{
    private readonly IDbConnectionManager _connectionManager = connectionManager;

    protected DbConnection Connection => InternalTransaction.Connection;

    protected DbTransaction Transaction => InternalTransaction.Transaction;

    private InternalDbTransaction InternalTransaction => InternalDbTransaction.GetCurrent(_connectionManager);
}

internal class InternalDbTransaction(DbConnection connection, DbTransaction transaction) : IDependentTransaction
{
    public DbConnection Connection { get; } = connection;

    public DbTransaction Transaction { get; } = transaction;

	public static InternalDbTransaction GetCurrent(IDbConnectionManager connectionManager)
	{
		int key = connectionManager.GetHashCode().ToString();
		IDependentTransaction current;
		if (!Transaction.Current.DependentTransactions.TryGetValue(key, out current))
		{
            DbConnection connection = connectionManager.CreateConnection();
            connection.Open();
            DbTransaction transaction = connection.BeginTransaction();

			current = new InternalDbTransaction(connection, transaction);
			Transaction.Current.Enlist(key, current);
		}

		return (InternalDbTransaction)current;
	}
}
```