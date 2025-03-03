using Common.Database;
using Common.Model;
using System.Data.Common;
using System.Data;

namespace Common.Dao
{
    public class ProductDao : Dao
    {
        #region ctor
        public ProductDao(DbContextDaoType dbContextDaoType, string connectionString) : base(dbContextDaoType, connectionString) { }

        public ProductDao(DbContextDaoType dbContextDaoType, IConnectionStringProvider connectionStringProvider) : base(dbContextDaoType, connectionStringProvider) { }

        public ProductDao(string dbContextDaoTypeString, string connectionString) : base(dbContextDaoTypeString, connectionString) { }

        public ProductDao(string dbContextDaoTypeString, IConnectionStringProvider connectionStringProvider) : base(dbContextDaoTypeString, connectionStringProvider) { }
        #endregion

        #region public
        #region products
        public IList<Product> GetProducts()
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.PRODUCTS ";
            return ExecuteReader(sql, BuildProducts, null);
        }

        public IList<Product> GetProductsByCategory(string category)
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.PRODUCTS " +
                                    $@"WHERE CATEGORY = {FormatParameterName("CATEGORY")}";
            return ExecuteReader(sql, BuildProducts, null);
        }

        public Product GetProductById(int Id)
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.PRODUCTS " +
                                    $@"WHERE ID = {FormatParameterName("ID")}";
            return ExecuteReader(sql, BuildProducts, null)[0];
        }

        public Product GetProductByProductId(int productId)
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.PRODUCTS " +
                                    $@"WHERE PRODUCT_ID = {FormatParameterName("PRODUCT_ID")}";
            return ExecuteReader(sql, BuildProducts, null)[0];
        }

        public Product GetProductByName(string name)
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.PRODUCTS " +
                                    $@"WHERE NAME = {FormatParameterName("NAME")}";
            return ExecuteReader(sql, BuildProducts, null)[0];
        }

        public int InsertProduct(Product product)
        {
            string sql = "INSERT INTO BETATEST.DBO.PRODUCTS " +
                            "(" +
                                "PRODUCT_ID, " +
                                "NAME, " +
                                "SHORT_DESCR, " +
                                "LONG_DESCR, " +
                                "IMAGE " +
                                "PRICE" +
                                "DISCOUNT" +
                                "CATEGORY" +
                            ") " +
                         "VALUES " +
                            "(" +
                                $@"{FormatParameterName("PRODUCT_ID")}, " +
                                $@"{FormatParameterName("NAME")}, " +
                                $@"{FormatParameterName("SHORT_DESCR")}, " +
                                $@"{FormatParameterName("LONG_DESCR")}, " +
                                $@"{FormatParameterName("IMAGE")}, " +
                                $@"{FormatParameterName("PRICE")} " +
                                $@"{FormatParameterName("DISCOUNT")} " +
                                $@"{FormatParameterName("CATEGORY")} " +
                            ");";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("PRODUCT_ID", product.ProductID),
                CreateParameter("NAME", product.Name),
                CreateParameter("SHORT_DESCR", product.ShortDescription),
                CreateParameter("LONG_DESCR", product.LongDescription),
                CreateParameter("IMAGE", product.Image),
                CreateParameter("PRICE", product.Price),
                CreateParameter("DISCOUNT", product.Discount),
                CreateParameter("CATEGORY", product.Category)
            };
            return ExecuteNonQuery(sql, dbParameters);
        }
        #endregion
        #endregion

        #region private
        private IList<Product> BuildProducts(IDataReader dataReader)
        {
            IList<Product> products = new List<Product>();
            while (dataReader.Read())
            {
                products.Add(
                    new Product()
                    {
                        ID = (int)dataReader[0],
                        ProductID = (int)dataReader[1],
                        Name = (string)dataReader[2],
                        ShortDescription = (string)dataReader[3],
                        LongDescription = (string)dataReader[4],
                        Image = (string)dataReader[5],
                        Price = (decimal)dataReader[6],
                        Tax = (decimal)dataReader[7],
                        Discount = (decimal)dataReader[8],
                        Category = (string)dataReader[9],
                    }
                );
            }
            return products;
        }
        #endregion
    }
}
