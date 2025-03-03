using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

namespace Common.Database.Oracle
{
    public class OracleDbContextDao : AbstractDbContextDao
    {
        #region prop
        private ILogger Logger;
        #endregion

        #region ctor
        public OracleDbContextDao() : base()
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<OracleDbContextDao>();
        }

        public OracleDbContextDao(string connectionString) : base(connectionString)
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<OracleDbContextDao>();
        }

        public OracleDbContextDao(IConnectionStringProvider connectionStringProvider) : base(connectionStringProvider)
        {
            ILoggerFactory factory = new LoggerFactory();
            Logger = factory.CreateLogger<OracleDbContextDao>();
        }
        #endregion

        #region public
        public override T ExecuteScalar<T>(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            T result = default;

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                try
                {
                    oracleConnection.Open();
                    OracleCommand oracleCommand = (OracleCommand)CreateCommand(oracleConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        oracleCommand.Parameters.AddRange(dbParameters);
                    }
                    result = (T)oracleCommand.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    oracleConnection.Close();
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

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                try
                {
                    oracleConnection.Open();
                    OracleCommand oracleCommand = (OracleCommand)CreateCommand(oracleConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        oracleCommand.Parameters.AddRange(dbParameters);
                    }
                    result = func(oracleCommand.ExecuteReader());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    oracleConnection.Close();
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

            using (OracleConnection oracleConnection = new OracleConnection(ConnectionString))
            {
                try
                {
                    oracleConnection.Open();
                    OracleCommand oracleCommand = (OracleCommand)CreateCommand(oracleConnection, sql, commandType, commandTimeout);
                    if (dbParameters != null)
                    {
                        oracleCommand.Parameters.AddRange(dbParameters);
                    }
                    result = oracleCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError($@"{ex.Message}: {ex.StackTrace}");
                }
                finally
                {
                    oracleConnection.Close();
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
            OracleCommand oracleCommand = new OracleCommand(sql, (OracleConnection)dbConnection);
            oracleCommand.CommandType = commandType;
            oracleCommand.CommandTimeout = commandTimeout;
            return oracleCommand;
        }

        public override DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            string oracleParameterName = FormatParameterName(name);
            OracleParameter oracleParameter = new OracleParameter(oracleParameterName, value);
            oracleParameter.OracleDbType = OracleDbTypeResolver.ResolveDbType(dbType);
            oracleParameter.Direction = parameterDirection;
            return oracleParameter;
        }

        public override string FormatParameterName(string name)
        {
            return $@":{name}";
        }
        #endregion
    }
}
