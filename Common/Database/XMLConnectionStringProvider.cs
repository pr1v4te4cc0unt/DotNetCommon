namespace Common.Database
{
    public class XMLConnectionStringProvider : ConnectionStringProvider
    {
        #region ctor
        public XMLConnectionStringProvider() : base() { }

        public XMLConnectionStringProvider(string path) : base(path) { }
        #endregion

        #region public
        public string GetConnectionString() 
        {
            return "";
        }
        #endregion
    }
}
