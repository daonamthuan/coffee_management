using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DTO
{
    internal class Category
    {
        public Category(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        public Category(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = (string)row["fcName"];
        }

        private int iD;
        private string name;

        public string Name { get => name; set => name = value; }
        public int ID { get => iD; set => iD = value; }
    }
}
