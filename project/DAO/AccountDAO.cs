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

        public DataTable GetListAccount()
        {
            return  DataProvider.Instance.ExecuteQuery("SELECT aUsername, displayname, aType FROM Account");
        }

        public bool InsertAccount(string username, string displayname, int type)
        {
            string query = "INSERT INTO Account (aUsername , displayname , aType) VALUES (N'" + username + "' , N'" + displayname + "' , " + type + " )";
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int id, string username, string displayname, int type)
        {
            string query = String.Format("UPDATE Account SET aUsername = N'{0}' , displayname = N'{1}', type = {2} WHERE id = {3}", username, displayname, type, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFood(int idAccount)
        {
            string query = "DELETE Food WHERE id = " + idAccount;
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

    }
}
