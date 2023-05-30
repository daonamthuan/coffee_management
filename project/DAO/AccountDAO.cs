using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) AccountDAO.instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName ,string passWord)
        {
            string query = "USP_Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord }); ;

            return result.Rows.Count > 0;
        }

        public bool UpdateAccout(string username, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("EXEC USP_UpdateAccount @username , @displayName , @password , @newpassword ", new object[] { username, displayName, pass, newPass });
            return result > 0;
        }

        public Account GetAccountByUserName(string username)
        {
            DataTable data =  DataProvider.Instance.ExecuteQuery("SELECT * FROM Account WHERE aUsername = '" + username + "'");
            foreach (DataRow item in data.Rows)
            {
                return new Account(item);

            }
            return null;
        }

    }
}
