using Common.EnumUtil;

namespace Common.Database
{
    public enum ConnectionStringProviderType { XML, JSON };

    public static class ConnectionStringProviderFactory
    {
        #region public
        public static IConnectionStringProvider GetConnectionStringProvider(ConnectionStringProviderType connectionStringProviderType) 
        { 
            IConnectionStringProvider connectionStringProvider = null;

            switch (connectionStringProviderType)
            {
                case ConnectionStringProviderType.XML:
                    connectionStringProvider = new XMLConnectionStringProvider();
                    break;
                case ConnectionStringProviderType.JSON:
                    connectionStringProvider = new JsonConnectionStringProvider();
                    break;
                default:
                    throw new NotImplementedException();
            }

            return connectionStringProvider;
        }

        public static IConnectionStringProvider GetConnectionStringProvider(ConnectionStringProviderType connectionStringProviderType, string path)
        {
            IConnectionStringProvider connectionStringProvider = null;

            switch (connectionStringProviderType)
            {
                case ConnectionStringProviderType.XML:
                    connectionStringProvider = new XMLConnectionStringProvider(path);
                    break;
                case ConnectionStringProviderType.JSON:
                    connectionStringProvider = new JsonConnectionStringProvider(path);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return connectionStringProvider;
        }
         
        public static IConnectionStringProvider GetConnectionStringProvider(string connectionStringProviderTypeString)
        {
            ConnectionStringProviderType connectionStringProviderType = EnumHelper.TryParse<ConnectionStringProviderType>(connectionStringProviderTypeString);

            return GetConnectionStringProvider(connectionStringProviderType);
        }

        public static IConnectionStringProvider GetConnectionStringProvider(string connectionStringProviderTypeString, string path)
        {
            ConnectionStringProviderType connectionStringProviderType = EnumHelper.TryParse<ConnectionStringProviderType>(connectionStringProviderTypeString);

            return GetConnectionStringProvider(connectionStringProviderType, path);
        }
        #endregion
    }
}
