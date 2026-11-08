using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Database
{
    public interface IDbContextDaoSync
    {
        T ExecuteScalar<T>(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400);

        IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400);

        int ExecuteNonQuery(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400);
    }
}
