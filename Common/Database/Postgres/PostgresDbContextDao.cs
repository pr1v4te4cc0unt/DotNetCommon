using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace Common.Database.Postgres
{
    public class PostgresDbContextDao : IDbContextDaoSync, IDbContextDaoCommand, IDbContextDaoParameter
    {
        #region prop
        private readonly string ConnectionString;
        private readonly ILogger Logger;
        #endregion

        #region ctor
        public PostgresDbContextDao(ILoggerFactory loggerFactory, string connectionString)
        {
            Logger = loggerFactory.CreateLogger<PostgresDbContextDao>();
            ConnectionString = connectionString;
        }

        public PostgresDbContextDao(ILoggerFactory loggerFactory, IConnectionStringProvider connectionStringProvider)
        {
            Logger = loggerFactory.CreateLogger<PostgresDbContextDao>();
            ConnectionString = connectionStringProvider.GetConnectionString();
        }
        #endregion

        #region public
        public T ExecuteScalar<T>(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T? result = default;

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    npgsqlConnection.Open();
                    using (NpgsqlCommand npgsqlCommand = (NpgsqlCommand)CreateCommand(npgsqlConnection, sql, commandType, commandTimeout))
                    {
                        npgsqlCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<NpgsqlParameter>()));
                        result = (T?)npgsqlCommand.ExecuteScalar();
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

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    npgsqlConnection.Open();
                    using (NpgsqlCommand npgsqlCommand = (NpgsqlCommand)CreateCommand(npgsqlConnection, sql, commandType, commandTimeout))
                    {
                        npgsqlCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<NpgsqlParameter>()));
                        using (NpgsqlDataReader npgsqlDataReader = npgsqlCommand.ExecuteReader())
                        {
                            result = func(npgsqlDataReader);
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

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    npgsqlConnection.Open();
                    using (NpgsqlCommand npgsqlCommand = (NpgsqlCommand)CreateCommand(npgsqlConnection, sql, commandType, commandTimeout))
                    {
                        npgsqlCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<NpgsqlParameter>()));
                        result = npgsqlCommand.ExecuteNonQuery();
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
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sql, (NpgsqlConnection)dbConnection);
            npgsqlCommand.CommandType = commandType;
            npgsqlCommand.CommandTimeout = commandTimeout;
            return npgsqlCommand;
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string npgsqlParameterName = FormatParameterName(name);
            NpgsqlParameter npgsqlParameter = new NpgsqlParameter(npgsqlParameterName, value)
            {
                NpgsqlDbType = PostgresDbTypeResolver.ResolveDbType(dbType),
                Direction = parameterDirection
            };
            return npgsqlParameter;
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
