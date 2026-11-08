using System.Data;
using System.Data.Common;

namespace Common.Database
{
    public interface IDBContextDaoAsync
    {
        Task<T> ExecuteScalarAsync<T>(CancellationToken cancellationToken, string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400);

        Task<IList<T>> ExecuteReaderAsync<T>(CancellationToken cancellationToken, string sql, Func<IDataReader, IList<T>> func, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400);

        Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken, string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400);
    }
}
