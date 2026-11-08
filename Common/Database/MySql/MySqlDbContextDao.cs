using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Npgsql;
using System.Data;
using System.Data.Common;

namespace Common.Database.MySql
{
    public class MySqlDbContextDao : IDbContextDaoSync, IDbContextDaoCommand, IDbContextDaoParameter
    {
        #region prop
        private readonly string ConnectionString;
        private readonly ILogger Logger;
        #endregion

        #region ctor
        public MySqlDbContextDao(ILoggerFactory loggerFactory, string connectionString)
        {
            Logger = loggerFactory.CreateLogger<MySqlDbContextDao>();
            ConnectionString = connectionString;
        }

        public MySqlDbContextDao(ILoggerFactory loggerFactory, IConnectionStringProvider connectionStringProvider)
        {
            Logger = loggerFactory.CreateLogger<MySqlDbContextDao>();
            ConnectionString = connectionStringProvider.GetConnectionString();
        }
        #endregion

        #region public
        public T ExecuteScalar<T>(string sql, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T? result = default;

            using (MySqlConnection mysqlConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    using (MySqlCommand mySqlCommand = (MySqlCommand)CreateCommand(mysqlConnection, sql, commandType, commandTimeout))
                    {
                        mySqlCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<MySqlParameter>()));
                        result = (T?)mySqlCommand.ExecuteScalar();
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

        public IList<T>? ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, IList<DbParameter>? dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            IList<T>? result = default;

            using (MySqlConnection mysqlConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    using (MySqlCommand mySqlCommand = (MySqlCommand)CreateCommand(mysqlConnection, sql, commandType, commandTimeout))
                    {
                        mySqlCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<MySqlParameter>()));
                        using (MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader())
                        {
                            result = func(mySqlDataReader);
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

            using (MySqlConnection mysqlConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    using (MySqlCommand mySqlCommand = (MySqlCommand)CreateCommand(mysqlConnection, sql, commandType, commandTimeout))
                    {
                        mySqlCommand.Parameters.AddRange((Array)(dbParameters ?? Array.Empty<MySqlParameter>()));
                        result = mySqlCommand.ExecuteNonQuery();
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
            MySqlCommand mySqlCommand = new MySqlCommand(sql, (MySqlConnection)dbConnection);
            mySqlCommand.CommandType = commandType;
            mySqlCommand.CommandTimeout = commandTimeout;
            return mySqlCommand;
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string mysqlParameterName = FormatParameterName(name);
            MySqlParameter mysSqlParameter = new MySqlParameter(mysqlParameterName, value)
            {
                MySqlDbType = MySqlDbTypeResolver.ResolveDbType(dbType),
                Direction = parameterDirection
            };
            return mysSqlParameter;
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
