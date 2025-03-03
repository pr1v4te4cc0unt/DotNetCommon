namespace Common.Database
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        #region prop
        public string Path { get; set; }

        public string Key { get; } = "ConnectionString";
        #endregion

        #region ctor
        public ConnectionStringProvider() { }

        public ConnectionStringProvider(string path)
        {
            this.Path = path;
        }
        #endregion

        #region public
        public string GetConnectionString() 
        { 
            throw new NotImplementedException();
        }

        public ConnectionStringProvider SetPath(string path)
        {
            Path = path;
            return this;
        }
        #endregion
    }
}
