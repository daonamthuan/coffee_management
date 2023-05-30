using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DTO
{
    public class Account
    {
        private string username;
        private string password;
        private string displayName;
        private int type;

        public Account(string username, string displayName, int type, string password = null)
        {
            this.Username = username;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }

        public Account (DataRow row)
        {
            this.Username = (string)row["aUsername"];
            this.DisplayName = (string)row["displayname"];
            this.Type = (int)row["aType"];
            this.Password = (string)row["aPassword"];
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int Type { get => type; set => type = value; }
    }
}
