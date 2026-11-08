using Common.Database.MySql;
using Common.Database.Oracle;
using Common.Database.Postgres;
using Common.Database.Sql;
using Common.EnumUtil;
using Microsoft.Extensions.Logging;

namespace Common.Database
{
    public enum DbContextDaoType { SqlServer, Oracle, MySql, Postgres }

    public static class DbContextDaoFactory
    {
        #region public
        public static IDbContextDaoSync GetDbContextDao(DbContextDaoType dbContextDaoType, ILoggerFactory loggerFactory, string connectionString)
        {
            IDbContextDaoSync dbContextDao = null;

            switch (dbContextDaoType)
            {
                case DbContextDaoType.SqlServer:
                    dbContextDao = new SqlDbContextDao(loggerFactory, connectionString);
                    break;
                case DbContextDaoType.Oracle:
                    dbContextDao = new OracleDbContextDao(loggerFactory, connectionString);
                    break;
                case DbContextDaoType.MySql:
                    dbContextDao = new MySqlDbContextDao(loggerFactory, connectionString);
                    break;
                case DbContextDaoType.Postgres:
                    dbContextDao = new PostgresDbContextDao(loggerFactory, connectionString);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return dbContextDao;
        }

        public static IDbContextDaoSync GetDbContextDao(DbContextDaoType dbContextDaoType, ILoggerFactory loggerFactory, IConnectionStringProvider connectionStringProvider)
        {
            IDbContextDaoSync dbContextDao = null;

            switch (dbContextDaoType)
            {
                case DbContextDaoType.SqlServer:
                    dbContextDao = new SqlDbContextDao(loggerFactory, connectionStringProvider);
                    break;
                case DbContextDaoType.Oracle:
                    dbContextDao = new OracleDbContextDao(loggerFactory, connectionStringProvider);
                    break;
                case DbContextDaoType.MySql:
                    dbContextDao = new MySqlDbContextDao(loggerFactory, connectionStringProvider);
                    break;
                case DbContextDaoType.Postgres:
                    dbContextDao = new PostgresDbContextDao(loggerFactory, connectionStringProvider);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return dbContextDao;
        }

        public static IDbContextDaoSync GetDbContextDao(string dbContextDaoTypeString, ILoggerFactory loggerFactory , string connectionString)
        {
            DbContextDaoType dbContextDaoType = EnumHelper.TryParse<DbContextDaoType>(dbContextDaoTypeString);

            return GetDbContextDao(dbContextDaoType, loggerFactory, connectionString);
        }

        public static IDbContextDaoSync GetDbContextDao(string dbContextDaoTypeString, ILoggerFactory loggerFactory, IConnectionStringProvider connectionStringProvider)
        {
            DbContextDaoType dbContextDaoType = EnumHelper.TryParse<DbContextDaoType>(dbContextDaoTypeString);

            return GetDbContextDao(dbContextDaoType, loggerFactory, connectionStringProvider);
        }
        #endregion
    }
}
