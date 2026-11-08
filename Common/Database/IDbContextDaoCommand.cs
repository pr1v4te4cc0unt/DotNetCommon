using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public interface IDbContextDaoCommand
    {
        DbCommand CreateCommand(DbConnection dbConnection, string sql, CommandType commandType = CommandType.Text, int commandTimeout = 400);
    }
}
