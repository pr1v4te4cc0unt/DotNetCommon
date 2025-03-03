using Common.Database.MySql;
using Common.Database.Oracle;
using Common.Database.Postgres;
using Common.Database.Sql;
using Common.EnumUtil;

namespace Common.Database
{
    public enum DbContextDaoType { SqlServer, Oracle, MySql, Postgres }

    public static class DbContextDaoFactory
    {
        #region public
        public static AbstractDbContextDao GetDbContextDao(DbContextDaoType dbContextDaoType)
        {
            AbstractDbContextDao dbContextDao = null;

            switch (dbContextDaoType)
            {
                case DbContextDaoType.SqlServer:
                    dbContextDao = new SqlDbContextDao();
                    break;
                case DbContextDaoType.Oracle:
                    dbContextDao = new OracleDbContextDao();
                    break;
                case DbContextDaoType.MySql:
                    dbContextDao = new MySqlDbContextDao();
                    break;
                case DbContextDaoType.Postgres:
                    dbContextDao = new PostgresDbContextDao();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return dbContextDao;
        }

        public static AbstractDbContextDao GetDbContextDao(DbContextDaoType dbContextDaoType, string connectionString)
        {
            AbstractDbContextDao dbContextDao = null;

            switch (dbContextDaoType)
            {
                case DbContextDaoType.SqlServer:
                    dbContextDao = new SqlDbContextDao(connectionString);
                    break;
                case DbContextDaoType.Oracle:
                    dbContextDao = new OracleDbContextDao(connectionString);
                    break;
                case DbContextDaoType.MySql:
                    dbContextDao = new MySqlDbContextDao(connectionString);
                    break;
                case DbContextDaoType.Postgres:
                    dbContextDao = new PostgresDbContextDao(connectionString);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return dbContextDao;
        }

        public static AbstractDbContextDao GetDbContextDao(DbContextDaoType dbContextDaoType, IConnectionStringProvider connectionStringProvider)
        {
            AbstractDbContextDao dbContextDao = null;

            switch (dbContextDaoType)
            {
                case DbContextDaoType.SqlServer:
                    dbContextDao = new SqlDbContextDao(connectionStringProvider);
                    break;
                case DbContextDaoType.Oracle:
                    dbContextDao = new OracleDbContextDao(connectionStringProvider);
                    break;
                case DbContextDaoType.MySql:
                    dbContextDao = new MySqlDbContextDao(connectionStringProvider);
                    break;
                case DbContextDaoType.Postgres:
                    dbContextDao = new PostgresDbContextDao(connectionStringProvider);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return dbContextDao;
        }

        public static AbstractDbContextDao GetDbContextDao(string dbContextDaoTypeString)
        {
            DbContextDaoType dbContextDaoType = EnumHelper.TryParse<DbContextDaoType>(dbContextDaoTypeString);

            return GetDbContextDao(dbContextDaoType);
        }

        public static AbstractDbContextDao GetDbContextDao(string dbContextDaoTypeString, string connectionString)
        {
            DbContextDaoType dbContextDaoType = EnumHelper.TryParse<DbContextDaoType>(dbContextDaoTypeString);

            return GetDbContextDao(dbContextDaoType, connectionString);
        }

        public static AbstractDbContextDao GetDbContextDao(string dbContextDaoTypeString, IConnectionStringProvider connectionStringProvider)
        {
            DbContextDaoType dbContextDaoType = EnumHelper.TryParse<DbContextDaoType>(dbContextDaoTypeString);

            return GetDbContextDao(dbContextDaoType, connectionStringProvider);
        }
        #endregion
    }
}
