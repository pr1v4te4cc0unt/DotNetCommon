using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace Common.Database.Oracle
{
    public class OracleDbContextDao : IDbContextDaoSync, IDbContextDaoCommand, IDbContextDaoParameter
    {
        #region prop
        private readonly string ConnectionString;
        private readonly ILogger Logger;
        #endregion

        #region ctor
        public OracleDbContextDao(ILoggerFactory loggerFactory, string connectionString)
        {
            Logger = loggerFactory.CreateLogger<OracleDbContextDao>();
            ConnectionString = connectionString;
        }

        public OracleDbContextDao(ILoggerFactory loggerFactory, IConnectionStringProvider connectionStringProvider)
        {
            Logger = loggerFactory.CreateLogger<OracleDbContextDao>();
            ConnectionString = connectionStringProvider.GetConnectionString();
        }
        #endregion

        #region public
        public T ExecuteScalar<T>(string sql, DbParameter[]? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T? result = default;

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                try
                {
                    oracleConnection.Open();
                    using(OracleCommand oracleCommand = (OracleCommand)CreateCommand(oracleConnection, sql, commandType, commandTimeout))
                    {
                        oracleCommand.Parameters.AddRange(dbParameters ?? Array.Empty<OracleParameter>());
                        result = (T?)oracleCommand.ExecuteScalar();
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

        public T ExecuteScalar<T>(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            DbParameter[]? dbParameterArray = dbParameters?.ToArray();
            return ExecuteScalar<T>(sql, dbParameterArray, commandType, commandTimeout);
        }

        public IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, DbParameter[]? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            IList<T>? result = default;

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                try
                {
                    oracleConnection.Open();
                    using (OracleCommand oracleCommand = (OracleCommand)CreateCommand(oracleConnection, sql, commandType, commandTimeout))
                    {
                        oracleCommand.Parameters.AddRange(dbParameters ?? Array.Empty<OracleParameter>());
                        using(OracleDataReader oracleDataReader = oracleCommand.ExecuteReader())
                        {
                            result = func(oracleDataReader);
                        }                    }
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
            DbParameter[]? dbParameterArray = dbParameters?.ToArray();
            return ExecuteReader(sql, func, dbParameterArray, commandType, commandTimeout);
        }

        public int ExecuteNonQuery(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            int result = 0;

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                try
                {
                    oracleConnection.Open();
                    using (OracleCommand oracleCommand = (OracleCommand)CreateCommand(oracleConnection, sql, commandType, commandTimeout))
                    {
                        oracleCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<OracleParameter>()));
                        result = oracleCommand.ExecuteNonQuery();
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
            OracleCommand oracleCommand = new OracleCommand(sql, (OracleConnection)dbConnection);
            oracleCommand.CommandType = commandType;
            oracleCommand.CommandTimeout = commandTimeout;
            return oracleCommand;
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string oracleParameterName = FormatParameterName(name);
            OracleParameter oracleParameter = new OracleParameter(oracleParameterName, value)
            {
                OracleDbType = OracleDbTypeResolver.ResolveDbType(dbType),
                Direction = parameterDirection
            };
            return oracleParameter;
        }
        #endregion

        #region private
        private string FormatParameterName(string name)
        {
            return $@":{name}";
        }
        #endregion
    }
}
