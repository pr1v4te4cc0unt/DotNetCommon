using Common.Database;
using Common.Model;
using System.Data;
using System.Data.Common;

namespace Common.Dao
{
    public class UserDao : Dao
    {
        #region ctor
        public UserDao(DbContextDaoType dbContextDaoType, string connectionString) : base(dbContextDaoType, connectionString) { }

        public UserDao(DbContextDaoType dbContextDaoType, IConnectionStringProvider connectionStringProvider) : base(dbContextDaoType, connectionStringProvider) { }

        public UserDao(string dbContextDaoTypeString, string connectionString) : base(dbContextDaoTypeString, connectionString) { }

        public UserDao(string dbContextDaoTypeString, IConnectionStringProvider connectionStringProvider) : base(dbContextDaoTypeString, connectionStringProvider) { }
        #endregion

        #region public
        #region users
        public User GetUserByCredentials(string userName, string password)
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.USERS " +
                                    $@"WHERE USER_NAME = {FormatParameterName("USER_NAME")} " +
                                    $@"AND PASSWORD = {FormatParameterName("PASSWORD")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_NAME", userName),
                CreateParameter("PASSWORD", password)
            };
            return ExecuteReader(sql, BuildUsers, dbParameters)[0];
        }

        public Task<User> GetUserByCredentialsAsync(string userName, string password)
        {
            Task<User> userTask = Task.Run(() => GetUserByCredentials(userName, password));
            return userTask;
        }

        public User GetUserByCredential(Credential credential)
        {
            return GetUserByCredentials(credential.Username, credential.Password);
        }

        public User GetUserByUserCredential(UserCredential userCredential)
        {
            return GetUserByCredentials(userCredential.Username, userCredential.Password);
        }

        public User GetUserByUserID(int userId)
        {
            string sql = "SELECT " +
                            "* " +
                                "FROM BETATEST.DBO.USERS " +
                                    $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_ID", userId, DbType.Int32)
            };
            return ExecuteReader(sql, BuildUsers, dbParameters)[0];
        }

        public int GetUserIDByCredentials(string userName, string password)
        {
            string sql = "SELECT " +
                            "USER_ID " +
                                "FROM BETATEST.DBO.USERS " +
                                    $@"WHERE USER_NAME = {FormatParameterName("USER_NAME")} " +
                                    $@"AND PASSWORD = {FormatParameterName("PASSWORD")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_NAME", userName),
                CreateParameter("PASSWORD", password)
            };
            return ExecuteScalar<int>(sql, dbParameters);
        }

        public int GetUserIDByCredential(Credential credential)
        {
            return GetUserIDByCredentials(credential.Username, credential.Password);
        }

        public int GetUserIDByUserCredential(UserCredential userCredential)
        {
            return GetUserIDByCredentials(userCredential.Username, userCredential.Password);
        }

        public int InsertUser(User user)
        {
            string sql = "INSERT INTO BETATEST.DBO.USERS " +
                "(" +
                    "USER_ID, " +
                    "USER_NAME, " +
                    "PASSWORD, " +
                    "FIRST_NAME, " +
                    "MIDDLE_NAME, " +
                    "LAST_NAME " +
                ") " +
             "VALUES " +
                "(" +
                    $@"{FormatParameterName("USER_ID")}, " +
                    $@"{FormatParameterName("USER_NAME")}, " +
                    $@"{FormatParameterName("PASSWORD")}, " +
                    $@"{FormatParameterName("FIRST_NAME")}, " +
                    $@"{FormatParameterName("MIDDLE_NAME")}, " +
                    $@"{FormatParameterName("LAST_NAME")} " +
                ");";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_ID", user.UserID),
                CreateParameter("USER_NAME", user.Username),
                CreateParameter("PASSWORD", user.Password),
                CreateParameter("FIRST_NAME", user.FirstName),
                CreateParameter("MIDDLE_NAME", user.MiddleName),
                CreateParameter("LAST_NAME", user.LastName)
            };
            return ExecuteNonQuery(sql, dbParameters);
        }

        public int UpdateUser(int userId, string firstName = "", string middleName = "", string lastName = "")
        {
            User user = GetUserByUserID(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            firstName = ResolveStringIsNullOrWhiteSpace(firstName, user.FirstName);
            middleName = ResolveStringIsNullOrWhiteSpace(middleName, user.MiddleName);
            lastName = ResolveStringIsNullOrWhiteSpace(lastName, user.LastName);

            string sql = "UPDATE BETATEST.DBO.USERS " +
                            $@"SET FIRST_NAME = {FormatParameterName("FIRST_NAME")}, " +
                            $@"SET MIDDLE_NAME = {FormatParameterName("MIDDLE_NAME")}, " +
                            $@"SET LAST_NAME = {FormatParameterName("LAST_NAME")} " +
                                $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("FIRST_NAME", firstName),
                CreateParameter("MIDDLE_NAME", middleName),
                CreateParameter("LAST_NAME", lastName),
                CreateParameter("USER_ID", userId, DbType.Int32),
            };
            return ExecuteNonQuery(sql, dbParameters);
        }

        public int UpdateUser(User user)
        {
            return UpdateUser(user.UserID, user.FirstName, user.MiddleName, user.LastName);
        }

        public int DeleteUser(int userId)
        {
            string sql = "DELETE FROM BETATEST.DBO.USERS " +
                            $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("ID", userId, DbType.Int32),
            };
            return ExecuteNonQuery(sql, dbParameters);
        }

        public int DeleteUser(User user)
        {
            return DeleteUser(user.UserID);
        }
        #endregion
        //#region usercredentials
        //public IList<UserCredential> GetUserCredentialByUserID(int userId)
        //{
        //    string sql = "SELECT " + 
        //                    "* " + 
        //                        "FROM BETATEST.DBO.USERS_CREDENTIALS " + 
        //                            $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
        //    IList<DbParameter> dbParameters = new List<DbParameter>()
        //    {
        //        CreateParameter("USER_ID", userId, DbType.Int32)
        //    };
        //    return ExecuteReader(sql, BuildUserCredentials, dbParameters);
        //}

        //public int InsertUserCredential(int userId, string userName, string password)
        //{
        //    string sql = "INSERT INTO BETATEST.DBO.USERS_CREDENTIALS " +
        //                     "(" +
        //                        "USER_ID, " +
        //                        "USER_NAME, " +
        //                        "PASSWORD" +
        //                     ") " +
        //                 "VALUES " +
        //                    "(" +
        //                        $@"{FormatParameterName("USER_ID")}, " +
        //                        $@"{FormatParameterName("USER_NAME")}, " +
        //                        $@"{FormatParameterName("PASSWORD")} " +
        //                    ");";
        //    IList<DbParameter> dbParameters = new List<DbParameter>()
        //    {
        //        CreateParameter("USER_ID", userId, DbType.Int32),
        //        CreateParameter("USER_NAME", userName),
        //        CreateParameter("PASSWORD", password)
        //    };
        //    return ExecuteNonQuery(sql, dbParameters);
        //}

        //public int UpdateUserCredentialByUserID(int userId, string userName, string password)
        //{
        //    UserCredential userCredential = GetUserCredentialByUserID(userId)[0];
        //    if (userCredential == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //    userName = ResolveStringIsNullOrWhiteSpace(userName, userCredential.UserName);
        //    password = ResolveStringIsNullOrWhiteSpace(password, userCredential.Password);

        //    string sql = "UPDATE BETATEST.DBO.USERS_CREDENTIALS " +
        //                    $@"SET USER_NAME = {FormatParameterName("USER_NAME")} " +
        //                    $@"SET PASSWORD = {FormatParameterName("PASSWORD")} " +
        //                        $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
        //    IList<DbParameter> dbParameters = new List<DbParameter>()
        //    {
        //        CreateParameter("USER_NAME", userName),
        //        CreateParameter("PASSWORD", password)
        //    };
        //    return ExecuteNonQuery(sql, dbParameters);
        //}

        //public int DeleteUserCredentialByUserID(int userId)
        //{
        //    string sql = "DELETE FROM BETATEST.DBO.USERS_CREDENTIALS " + 
        //                    $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
        //    IList<DbParameter> dbParameters = new List<DbParameter>()
        //    {
        //        CreateParameter("USER_ID", userId, DbType.Int32),
        //    };
        //    return ExecuteNonQuery(sql, dbParameters);
        //}

        //public int ValidateUserCredential(string userName, string password)
        //{
        //    string sql = "SELECT " +
        //                    "COUNT(*) " +
        //                        "FROM BETATEST.DBO.USERS_CREDENTIALS " +
        //                            $@"WHERE USER_NAME = {FormatParameterName("USER_NAME")}" +
        //                            $@"AND PASSWORD = {FormatParameterName("PASSWORD")};";
        //    IList<DbParameter> dbParameters = new List<DbParameter>()
        //    {
        //        CreateParameter("USER_NAME", userName),
        //        CreateParameter("PASSWORD", password)
        //    };
        //    return ExecuteScalar<int>(sql, dbParameters);
        //}
        //#endregion

        #region useraddresses
        public int InsertUserAddress(int userId, string primaryAddress, string secondaryAddress, string city, string stateProvince, string postalCode, string country)
        {
            string sql = "INSERT INTO BETATEST.DBO.USERS_ADRESSES " +
                            "(" +
                                "USER_ID, " +
                                "PRIMARY_ADDRESS, " +
                                "SECONDARY_ADDRESS, " +
                                "CITY" +
                                "STATE_PROVINCE" +
                                "POSTAL_CODE" +
                                "COUNTRY" +
                            ") " +
                         "VALUES " +
                            "(" +
                                $@"{FormatParameterName("USER_ID")}, " +
                                $@"{FormatParameterName("PRIMARY_ADDRESS")}, " +
                                $@"{FormatParameterName("SECONDARY_ADDRESS")}, " +
                                $@"{FormatParameterName("CITY")} " +
                                $@"{FormatParameterName("STATE_PROVINCE")} " +
                                $@"{FormatParameterName("POSTAL_CODE")} " +
                                $@"{FormatParameterName("COUNTRY")} " +
                            ");";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_ID", userId),
                CreateParameter("PRIMARY_ADDRESS", primaryAddress),
                CreateParameter("SECONDARY_ADDRESS", secondaryAddress),
                CreateParameter("CITY", city),
                CreateParameter("STATE_PROVINCE", stateProvince),
                CreateParameter("POSTAL_CODE", postalCode),
                CreateParameter("COUNTRY", country),
            };
            return ExecuteNonQuery(sql, dbParameters);
        }

        public IList<UserAddress> GetUserAddressByUserID(int userId)
        {
            string sql = $@"SELECT * FROM BETATEST.DBO.USERS_ADDRESSES WHERE USER_ID = {FormatParameterName("USER_ID")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_ID", userId, DbType.Int32),
            };
            return ExecuteReader(sql, BuildUserAddresses, dbParameters);
        }

        public int UpdateUserAddressByUserID(int userId, string primaryAddress = "", string secondaryAddress = "", string city = "", string stateProvince = "", string postalCode = "", string country = "")
        {
            UserAddress userAddress = GetUserAddressByUserID(userId)[0];
            if (userAddress == null)
            {
                throw new Exception("UserAddress not found.");
            }

            primaryAddress = ResolveStringIsNullOrWhiteSpace(primaryAddress, userAddress.PrimaryAddress);
            secondaryAddress = ResolveStringIsNullOrWhiteSpace(secondaryAddress, userAddress.SecondaryAddress);
            city = ResolveStringIsNullOrWhiteSpace(city, userAddress.City);
            stateProvince = ResolveStringIsNullOrWhiteSpace(stateProvince, userAddress.StateProvince);
            postalCode = ResolveStringIsNullOrWhiteSpace(postalCode, userAddress.PostalCode);
            country = ResolveStringIsNullOrWhiteSpace(country, userAddress.Country);

            string sql = "UPDATE BETATEST.DBO.USERS_ADDRESSES " +
                            $@"SET PRIMARY_ADDRESS = {FormatParameterName("PRIMARY_ADDRESS")}, " +
                            $@"SET SECONDARY_ADDRESS = {FormatParameterName("SECONDARY_ADDRESS")}, " +
                            $@"SET CITY = {FormatParameterName("CITY")}, " +
                            $@"SET STATE_PROVINCE = {FormatParameterName("STATE_PROVINCE")}, " +
                            $@"SET POSTAL_CODE = {FormatParameterName("POSTAL_CODE")}, " +
                            $@"SET COUNTRY = {FormatParameterName("COUNTRY")} " +
                                $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("PRIMARY_ADDRESS", primaryAddress),
                CreateParameter("SECONDARY_ADDRESS", secondaryAddress),
                CreateParameter("CITY", city),
                CreateParameter("STATE_PROVINCE", stateProvince),
                CreateParameter("POSTAL_CODE", postalCode),
                CreateParameter("COUNTRY", country),
                CreateParameter("USER_ID", userId, DbType.Int32),
            };
            return ExecuteNonQuery(sql, dbParameters);
        }

        public int DeleteUserAddressByUserID(int userId)
        {
            string sql = "DELETE FROM BETATEST.DBO.USERS_ADDRESSES " +
                            $@"WHERE USER_ID = {FormatParameterName("USER_ID")};";
            IList<DbParameter> dbParameters = new List<DbParameter>()
            {
                CreateParameter("USER_ID", userId),
            };
            return ExecuteNonQuery(sql, dbParameters);
        }
        #endregion
        #endregion

        #region private
        private IList<User> BuildUsers(IDataReader dataReader)
        {
            IList<User> users = new List<User>();
            while (dataReader.Read())
            {
                users.Add(
                    new User()
                    {
                        ID = (int)dataReader[0],
                        UserID = (int)dataReader[1],
                        Username = (string)dataReader[2],
                        Password = (string)dataReader[3],
                        FirstName = (string)dataReader[4],
                        MiddleName = (string)dataReader[5],
                        LastName = (string)dataReader[6]
                    }
                );
            }
            return users;
        }

        private IList<UserAddress> BuildUserAddresses(IDataReader dataReader)
        {
            IList<UserAddress> userAddresses = new List<UserAddress>();
            while (dataReader.Read())
            {
                userAddresses.Add(
                    new UserAddress()
                    {
                        ID = (int)dataReader[0],
                        UserID = (int)dataReader[1],
                        PrimaryAddress = (string)dataReader[2],
                        SecondaryAddress = (string)dataReader[3],
                        City = (string)dataReader[4],
                        StateProvince = (string)dataReader[5],
                        PostalCode = (string)dataReader[6],
                        Country = (string)dataReader[7]
                    }
                );
            }
            return userAddresses;
        }

        //private IList<UserCredential> BuildUserCredentials(IDataReader dataReader)
        //{
        //    IList<UserCredential> userCredentials = new List<UserCredential>();
        //    while (dataReader.Read())
        //    {
        //        userCredentials.Add(
        //            new UserCredential()
        //            {
        //                ID = (int)dataReader[0],
        //                UserID = (int)dataReader[1],
        //                UserName = (string)dataReader[2],
        //                Password = (string)dataReader[3]
        //            }
        //        );
        //    }
        //    return userCredentials;
        //}
        #endregion
    }
}
