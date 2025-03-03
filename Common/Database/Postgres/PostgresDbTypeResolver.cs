using System.Data;
using NpgsqlTypes;

namespace Common.Database.Postgres
{
    public static class PostgresDbTypeResolver
    {
        #region public
        public static NpgsqlDbType ResolveDbType(DbType dbType)
        {
            NpgsqlDbType npgSqlDbType = NpgsqlDbType.Text;

            switch (dbType)
            {
                case DbType.Boolean:
                    npgSqlDbType = NpgsqlDbType.Char;
                    break;
                case DbType.Date:
                    npgSqlDbType = NpgsqlDbType.Date;
                    break;
                case DbType.DateTime:
                case DbType.DateTime2:
                    npgSqlDbType = NpgsqlDbType.Timestamp;
                    break;
                case DbType.Decimal:
                    npgSqlDbType = NpgsqlDbType.Money;
                    break;
                case DbType.Double:
                    npgSqlDbType = NpgsqlDbType.Double;
                    break;
                case DbType.Int16:
                case DbType.UInt16:
                    npgSqlDbType = NpgsqlDbType.Smallint;
                    break;
                case DbType.Int32:
                case DbType.UInt32:
                    npgSqlDbType = NpgsqlDbType.Integer;
                    break;
                case DbType.Int64:
                case DbType.UInt64:
                    npgSqlDbType = NpgsqlDbType.Bigint;
                    break;
                default:
                    npgSqlDbType = NpgsqlDbType.Text;
                    break;
            }
            return npgSqlDbType;
        }
        #endregion
    }
}
