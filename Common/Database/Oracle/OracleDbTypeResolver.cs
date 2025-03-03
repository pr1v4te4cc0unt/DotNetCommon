using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Common.Database.Oracle
{
    public static class OracleDbTypeResolver
    {
        #region public
        public static OracleDbType ResolveDbType(DbType dbType)
        {
            OracleDbType oracleDbType = OracleDbType.NVarchar2;

            switch (dbType)
            {
                case DbType.Binary:
                    oracleDbType = OracleDbType.Blob;
                    break;
                case DbType.Boolean:
                    oracleDbType = OracleDbType.Char;
                    break;
                case DbType.Date:
                    oracleDbType = OracleDbType.Date;
                    break;
                case DbType.DateTime:
                case DbType.DateTime2:
                    oracleDbType = OracleDbType.TimeStamp;
                    break;
                case DbType.Decimal:
                    oracleDbType = OracleDbType.Decimal;
                    break;
                case DbType.Double:
                    oracleDbType = OracleDbType.Double;
                    break;
                case DbType.Int16:
                case DbType.UInt16:
                    oracleDbType = OracleDbType.Int16;
                    break;
                case DbType.Int32:
                case DbType.UInt32:
                    oracleDbType = OracleDbType.Int32;
                    break;
                case DbType.Int64:
                case DbType.UInt64:
                    oracleDbType = OracleDbType.Int64;
                    break;
                default:
                    oracleDbType = OracleDbType.NVarchar2;
                    break;
            }
            return oracleDbType;
        }
        #endregion
    }
}
