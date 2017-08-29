using System.Data.Common;

namespace Quarks.DataAccess.Db
{
    public interface IDbConnectionFactory
    {
        DbConnection Create();

        string GetKey();
    }
}