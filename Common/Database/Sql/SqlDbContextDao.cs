using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Common.Database.Sql
{
    public class SqlDbContextDao : AbstractDbContextDao
    {
        #region prop
        private ILogger Logger;
        #endregion

        #region ctor
        public SqlDbContextDao() : base() 
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<SqlDbContextDao>();
        }

        public SqlDbContextDao(string connectionString) : base(connectionString) 
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<SqlDbContextDao>();
        }

        public SqlDbContextDao(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider) 
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<SqlDbContextDao>();
        }
        #endregion

        #region public
        public override T ExecuteScalar<T>(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T result = default;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        sqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = (T)sqlCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    sqlConnection.Close();
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

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        sqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = func(sqlCommand.ExecuteReader());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    sqlConnection.Close();
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

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = (SqlCommand)CreateCommand(sqlConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        sqlCommand.Parameters.AddRange(dbParameters);
                    }
                    result = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    sqlConnection.Close();
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
            SqlCommand sqlCommand = new SqlCommand(sql, (SqlConnection)dbConnection);
            sqlCommand.CommandType = commandType;
            sqlCommand.CommandTimeout = commandTimeout;
            return sqlCommand;
        }

        public override DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string sqlParameterName = FormatParameterName(name);
            SqlParameter sqlParameter = new SqlParameter(sqlParameterName, value);
            sqlParameter.SqlDbType = SqlDbTypeResolver.ResolveDbType(dbType);
            sqlParameter.Direction = parameterDirection;
            return sqlParameter;
        }

        public override string FormatParameterName(string name) {
            return $@"@{name}";
        }
        #endregion
    }
}
