using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace Common.Database.Sql
{
    public class SqlDbContextDao : IDbContextDao, IDbContextDaoCommand, IDbContextDaoParameter
    {
        #region prop
        private readonly string ConnectionString;
        private readonly ILogger Logger;
        #endregion

        #region ctor
        public SqlDbContextDao(ILoggerFactory loggerFactory, string connectionString) 
        {
            Logger = loggerFactory.CreateLogger<SqlDbContextDao>();
            ConnectionString = connectionString;
        }

        public SqlDbContextDao(ILoggerFactory loggerFactory, IConnectionStringProvider connectionStringProvider)
        {
            Logger = loggerFactory.CreateLogger<SqlDbContextDao>();
            ConnectionString = connectionStringProvider.GetConnectionString();
        }
        #endregion

        #region public
        public T ExecuteScalar<T>(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T? result = default;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout))
                    {
                        sqlCommand.Parameters.AddRange((SqlParameter[])(dbParameters ?? Array.Empty<SqlParameter>()));
                        result = (T?)sqlCommand.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                    throw;
                }
            }

            return result;
        }

        public IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            IList<T>? result = default;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout))
                    {
                        sqlCommand.Parameters.AddRange((SqlParameter[])(dbParameters ?? Array.Empty<SqlParameter>()));
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            result = func(sqlDataReader);
                        }
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                    throw;
                }
            }

            return result;
        }

        public int ExecuteNonQuery(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            int result = 0;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout))
                    {
                        sqlCommand.Parameters.AddRange((SqlParameter[])(dbParameters ?? Array.Empty<SqlParameter>()));
                        result = sqlCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                    throw;
                }
            }

            return result;
        }

        public async Task<T> ExecuteScalarAsync<T>(CancellationToken cancellationToken, string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T? result = default;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                    using (SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout))
                    {
                        sqlCommand.Parameters.AddRange((SqlParameter[])(dbParameters ?? Array.Empty<SqlParameter>()));
                        result = (T?)(await sqlCommand.ExecuteScalarAsync(cancellationToken));
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                    throw;
                }
            }

            return result;
        }

        public async Task<IList<T>> ExecuteReaderAsync<T>(CancellationToken cancellationToken, string sql, Func<IDataReader, IList<T>> func, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            IList<T>? result = default;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                    using (SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout))
                    {
                        sqlCommand.Parameters.AddRange((SqlParameter[])(dbParameters ?? Array.Empty<SqlParameter>()));
                        using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken))
                        {
                            result = func(sqlDataReader);
                        }
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                    throw;
                }
            }

            return result;
        }

        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken, string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            int result = 0;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                    using (SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout))
                    {
                        sqlCommand.Parameters.AddRange((SqlParameter[])(dbParameters ?? Array.Empty<SqlParameter>()));
                        result = await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
#endif
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                    throw;
                }
            }

            return result;
        }

        public DbCommand CreateCommand(DbConnection dbConnection, string sql, CommandType commandType, int commandTimeout)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, (SqlConnection)dbConnection);
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = commandTimeout;
            return sqlCommand;
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string sqlParameterName = FormatParameterName(name);
            SqlParameter sqlParameter = new SqlParameter(sqlParameterName, value)
            {
                SqlDbType = SqlDbTypeResolver.ResolveDbType(dbType),
                Direction = parameterDirection
            };
            return sqlParameter;
        }
        #endregion

        #region private
        private string FormatParameterName(string name)
        {
            return $@"@{name}";
        }
        #endregion
    }
}
