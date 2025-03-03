using System.Data;
using System.Data.Common;

namespace Common.Database
{
    public class DbContextDaoHandler
    {
        #region prop
        protected readonly AbstractDbContextDao AbstractDbContextDao;
        #endregion

        #region ctor
        public DbContextDaoHandler(DbContextDaoType dbContextDaoType, string connectionString)
        {
            AbstractDbContextDao = DbContextDaoFactory.GetDbContextDao(dbContextDaoType, connectionString);
        }

        public DbContextDaoHandler(DbContextDaoType dbContextDaoType, IConnectionStringProvider connectionStringProvider)
        {
            AbstractDbContextDao = DbContextDaoFactory.GetDbContextDao(dbContextDaoType, connectionStringProvider);
        }

        public DbContextDaoHandler(string dbContextDaoTypeString, string connectionString)
        {
            AbstractDbContextDao = DbContextDaoFactory.GetDbContextDao(dbContextDaoTypeString, connectionString);
        }

        public DbContextDaoHandler(string dbContextDaoTypeString, IConnectionStringProvider connectionStringProvider)
        {
            AbstractDbContextDao = DbContextDaoFactory.GetDbContextDao(dbContextDaoTypeString, connectionStringProvider);
        }
        #endregion

        #region protected
        protected T ExecuteScalar<T>(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            return AbstractDbContextDao.ExecuteScalar<T>(sql, dbParameters, commandType, commandTimeout);
        }

        protected T ExecuteScalar<T>(string sql, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            return AbstractDbContextDao.ExecuteScalar<T>(sql, dbParameters, commandType, commandTimeout);
        }

        protected IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            return AbstractDbContextDao.ExecuteReader(sql, func, dbParameters, commandType, commandTimeout);
        }

        protected IList<T> ExecuteReader<T>(string sql, Func<IDataReader, IList<T>> func, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            return AbstractDbContextDao.ExecuteReader(sql, func, dbParameters, commandType, commandTimeout);
        }

        protected int ExecuteNonQuery(string sql, DbParameter[] dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            return AbstractDbContextDao.ExecuteNonQuery(sql, dbParameters, commandType, commandTimeout);
        }

        protected int ExecuteNonQuery(string sql, IList<DbParameter> dbParameters = null, CommandType commandType = CommandType.Text, int commandTimeout = 400)
        {
            return AbstractDbContextDao.ExecuteNonQuery(sql, dbParameters, commandType, commandTimeout);
        }

        protected DbCommand CreateCommand(DbConnection dbConnection, string sql, CommandType commandType, int commandTimeout)
        {
            return AbstractDbContextDao.CreateCommand(dbConnection, sql, commandType, commandTimeout);
        }

        protected DbParameter CreateParameter(string name, object value, DbType dbType = DbType.String, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            return AbstractDbContextDao.CreateParameter(name, value, dbType, parameterDirection);
        }

        protected string FormatParameterName(string name)
        {
            return AbstractDbContextDao.FormatParameterName(name);
        }
        #endregion
    }
}
