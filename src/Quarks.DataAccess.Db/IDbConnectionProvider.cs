using System.Data.Common;

namespace Quarks.DataAccess.Db
{
    public interface IDbConnectionProvider
    {
        DbConnection Connection { get; }

        DbTransaction Transaction { get; }
    }
}