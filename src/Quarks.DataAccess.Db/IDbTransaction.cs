using System.Data.Common;

namespace Quarks.DataAccess.Db
{
    public interface IDbTransaction
    {
        DbConnection Connection { get; }

        DbTransaction Transaction { get; }
    }
}