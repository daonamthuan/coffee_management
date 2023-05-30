using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (TableDAO.instance == null) instance = new TableDAO(); return instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 118;
        public static int TableHeight = 118;
        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            // Lay cai bang tu sql
            DataTable data = DataProvider.Instance.ExecuteQuery("EXEC USP_GetTableList");

            // chuyen 1 dong trong bang thanh 1 doi tuong
            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
    }
}
