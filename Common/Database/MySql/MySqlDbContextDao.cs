using System.Data;
using System.Data.Common;
using Common.Database.Postgres;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Common.Database.MySql
{
    public class MySqlDbContextDao : AbstractDbContextDao
    {
        #region prop
        private ILogger Logger;
        #endregion

        #region ctor
        public MySqlDbContextDao() : base()
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<MySqlDbContextDao>();
        }

        public MySqlDbContextDao(string connectionString) : base(connectionString)
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<MySqlDbContextDao>();
        }

        public MySqlDbContextDao(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<MySqlDbContextDao>();
        }
        #endregion

        #region public
        public override T ExecuteScalar<T>(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T result = default;

            using (MySqlConnection mysqlConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    MySqlCommand mySqlCommand = (MySqlCommand)CreateCommand(mysqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        mySqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = (T)mySqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    mysqlConnection.Close();
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

            using (MySqlConnection mysqlConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    MySqlCommand mySqlCommand = (MySqlCommand)CreateCommand(mysqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        mySqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = func(mySqlCommand.ExecuteReader());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    mysqlConnection.Close();
                }
            }

            return result;
        }

        public override IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            DbParameter[] dbParameterArray = dbParameters.ToArray();
            return ExecuteReader<T>(sql, func, dbParameterArray, commandType, commandTimeout);
        }

        public override int ExecuteNonQuery(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            int result = 0;

            using (MySqlConnection mysqlConnection = new MySqlConnection(ConnectionString))
            {
                try
                {
                    mysqlConnection.Open();
                    MySqlCommand mySqlCommand = (MySqlCommand)CreateCommand(mysqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        mySqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = mySqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    mysqlConnection.Close();
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
            MySqlCommand mySqlCommand = new MySqlCommand(sql, (MySqlConnection)dbConnection);
            mySqlCommand.CommandType = commandType;
            mySqlCommand.CommandTimeout = commandTimeout;
            return mySqlCommand;

            
        }

        public override DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string mysqlParameterName = FormatParameterName(name);
            MySqlParameter mysSqlParameter = new MySqlParameter(mysqlParameterName, value);
            mysSqlParameter.MySqlDbType = MySqlDbTypeResolver.ResolveDbType(dbType);
            mysSqlParameter.Direction = parameterDirection;
            return mysSqlParameter;
        }

        public override string FormatParameterName(string name)
        {
            return $@":{name}";
        }
        #endregion
    }
}
