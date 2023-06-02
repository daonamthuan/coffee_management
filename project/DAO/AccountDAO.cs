using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
            /* C# hỗ trợ thư viện mã hóa mật khẩu MD5
             * Từ chuỗi => chuyển thành mảng byte => (dùng thư viện MD5) => chuyển thành mảng byte khác đã được băm
             */
            //byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord); // Lấy mảng kiểu byte từ chuỗi
            //byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp); // mật khẩu đã được mã hóa

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
            return  DataProvider.Instance.ExecuteQuery("SELECT aUsername AS N'Username', displayname AS N'Displayname' , aType AS N'Type' FROM Account");
        }

        public bool CheckUsernameExist(string username)
        {
            return (int)DataProvider.Instance.ExecuteScalar("SELECT COUNT (*) FROM Account WHERE aUsername = N'" + username + "'") > 0;
        }

        public bool InsertAccount(string username, string displayname, int type)
        {
            string query = "INSERT INTO Account (aUsername , aPassword , displayname , aType) VALUES (N'" + username + "' , N'12345678Aa@' , N'" + displayname + "' , " + type + " )";
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string username, string displayname, int type)
        {
            string query = String.Format("UPDATE Account SET displayname = N'{1}', aType = {2} WHERE aUsername = N'{0}'", username, displayname, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string username)
        {
            string query = "DELETE Account WHERE aUsername = N'" + username + "'";
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string username)
        {
            string query = String.Format("UPDATE Account SET aPassword = '12345678Aa@' WHERE aUsername = N'{0}'", username);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
