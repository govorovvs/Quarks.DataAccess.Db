using System.Data.Common;

namespace Quarks.DataAccess.Db.ConnectionManagement
{
	public interface IDbConnectionManager
	{
		DbConnection CreateConnection();

		int GetHashCode();
	}
}