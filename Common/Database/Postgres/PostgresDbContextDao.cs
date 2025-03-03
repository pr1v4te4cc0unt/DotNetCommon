using System.Data;
using System.Data.Common;
using Common.Database.Oracle;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace Common.Database.Postgres
{
    public class PostgresDbContextDao : AbstractDbContextDao
    {
        #region prop
        private ILogger Logger;
        #endregion

        #region ctor
        public PostgresDbContextDao() : base()
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<PostgresDbContextDao>();
        }

        public PostgresDbContextDao(string connectionString) : base(connectionString)
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<PostgresDbContextDao>();
        }

        public PostgresDbContextDao(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<PostgresDbContextDao>();
        }
        #endregion

        #region public
        public override T ExecuteScalar<T>(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T result = default;

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    npgsqlConnection.Open();
                    NpgsqlCommand npgsqlCommand = (NpgsqlCommand)CreateCommand(npgsqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        npgsqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = (T)npgsqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    npgsqlConnection.Close();
                }
            }


            return result;
        }

        public override T ExecuteScalar<T>(string sql, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            DbParameter[] dbParameterArray = dbParameters.ToArray();
            return ExecuteScalar<T>(sql, dbParameterArray, commandType, commandTimeout);
        }

        public override IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            IList<T> result = default;

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    npgsqlConnection.Open();
                    NpgsqlCommand npgsqlCommand = (NpgsqlCommand)CreateCommand(npgsqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        npgsqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = func(npgsqlCommand.ExecuteReader());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    npgsqlConnection.Close();
                }
            }

            return result;
        }

        public override IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            DbParameter[] dbParameterArray = dbParameters.ToArray();
            return ExecuteReader(sql, func, dbParameterArray, commandType, commandTimeout);
        }

        public override int ExecuteNonQuery(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            int result = 0;

            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    npgsqlConnection.Open();
                    NpgsqlCommand npgsqlCommand = (NpgsqlCommand)CreateCommand(npgsqlConnection,sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        npgsqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = npgsqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    npgsqlConnection.Close();
                }
            }

            return result;
        }

        public override int ExecuteNonQuery(string sql, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            DbParameter[] dbParameterArray = dbParameters.ToArray();
            return ExecuteNonQuery(sql, dbParameterArray, commandType, commandTimeout);
        }

        public override DbCommand CreateCommand(DbConnection dbConnection, string sql, CommandType commandType, int commandTimeout)
        {
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(sql, (NpgsqlConnection)dbConnection);
            npgsqlCommand.CommandType = commandType;
            npgsqlCommand.CommandTimeout = commandTimeout;
            return npgsqlCommand;
        }

        public override DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string npgsqlParameterName = FormatParameterName(name);
            NpgsqlParameter npgsqlParameter = new NpgsqlParameter(npgsqlParameterName, value);
            npgsqlParameter.NpgsqlDbType = PostgresDbTypeResolver.ResolveDbType(dbType);
            npgsqlParameter.Direction = parameterDirection;
            return npgsqlParameter;
        }

        public override string FormatParameterName(string name)
        {
            return $@"@{name}";
        }
        #endregion
    }
}
