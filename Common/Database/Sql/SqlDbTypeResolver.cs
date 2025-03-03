using System.Data;

namespace Common.Database.Sql
{
    public static class SqlDbTypeResolver
    {
        #region public
        public static SqlDbType ResolveDbType(DbType dbType) 
        {
            SqlDbType sqlDbType = SqlDbType.NVarChar;
            
            switch (dbType) 
            {
                case DbType.Binary:
                    sqlDbType = SqlDbType.Binary;
                    break;
                case DbType.Boolean:
                    sqlDbType = SqlDbType.Bit;
                    break;
                case DbType.Date:
                    sqlDbType = SqlDbType.Date;
                    break;
                case DbType.DateTime:
                    sqlDbType = SqlDbType.DateTime;
                    break;
                case DbType.DateTime2:
                    sqlDbType = SqlDbType.DateTime2;
                    break;
                case DbType.Decimal:
                    sqlDbType = SqlDbType.Decimal; 
                    break;
                case DbType.Double:
                    sqlDbType = SqlDbType.Float;
                    break;
                case DbType.Int16:
                case DbType.UInt16:
                    sqlDbType = SqlDbType.SmallInt;
                    break;
                case DbType.Int32:
                case DbType.UInt32:
                    sqlDbType = SqlDbType.Int;
                    break;
                case DbType.Int64:
                case DbType.UInt64:
                    sqlDbType = SqlDbType.BigInt;
                    break;
                default:
                    sqlDbType = SqlDbType.NVarChar;
                    break;
            }
            return sqlDbType;
        }
        #endregion
    }
}
