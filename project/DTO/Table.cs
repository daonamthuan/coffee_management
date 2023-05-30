using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DTO
{
    internal class Table
    {
        public Table(int id, string name, string status)
        {
            this.ID = id;
            this.Name = name;
            this.Status = status;
        }
        private int iD;
        private string name;
        private string status;

        public Table (DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = (string)row["tName"];
            this.Status = (string)row["tStatus"];
        }
        public int ID 
        {
            get { return iD; }
            private set { iD = value; } 
        }

        public string Name { get => name; private set => name = value; }
        public string Status { get => status; set => status = value; }
    }
}
