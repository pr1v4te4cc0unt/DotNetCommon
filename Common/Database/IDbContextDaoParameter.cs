using System.Data;
using System.Data.Common;

namespace Common.Database
{
    public interface IDbContextDaoParameter
    {
        DbParameter CreateParameter(string name, object value, DbType dbType = DbType.AnsiString, ParameterDirection parameterDirection = ParameterDirection.Input);
    }
}
