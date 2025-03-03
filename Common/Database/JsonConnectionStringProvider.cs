namespace Common.Database
{
    public class JsonConnectionStringProvider : ConnectionStringProvider
    {
        #region ctor
        public JsonConnectionStringProvider() : base() { }

        public JsonConnectionStringProvider(string path) : base(path) { }
        #endregion

        #region public
        public string GetConnectionString() 
        {
            return "";
        }
        #endregion
    }
}
