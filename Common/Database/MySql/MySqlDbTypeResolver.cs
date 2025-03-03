using System.Data;
using MySql.Data.MySqlClient;

namespace Common.Database.MySql
{
    public static class MySqlDbTypeResolver
    {
        #region public
        public static MySqlDbType ResolveDbType(DbType dbType)
        {
            MySqlDbType mySqlDbType = MySqlDbType.String;

            switch (dbType)
            {
                case DbType.Binary:
                    mySqlDbType = MySqlDbType.Blob;
                    break;
                case DbType.Date:
                    mySqlDbType = MySqlDbType.Date;
                    break;
                case DbType.DateTime:
                case DbType.DateTime2:
                    mySqlDbType = MySqlDbType.DateTime;
                    break;
                case DbType.Decimal:
                    mySqlDbType = MySqlDbType.Decimal;
                    break;
                case DbType.Double:
                    mySqlDbType = MySqlDbType.Double;
                    break;
                case DbType.Int16:
                case DbType.UInt16:
                    mySqlDbType = MySqlDbType.Int16;
                    break;
                case DbType.Int32:
                case DbType.UInt32:
                    mySqlDbType = MySqlDbType.Int32;
                    break;
                case DbType.Int64:
                case DbType.UInt64:
                    mySqlDbType = MySqlDbType.Int64;
                    break;
                default:
                    mySqlDbType = MySqlDbType.String;
                    break;
            }
            return mySqlDbType;
        }
        #endregion
    }
}
