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

        public static int TableWidth = 145;
        public static int TableHeight = 145;
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
        public List<Table> GetListTable()
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

        // Nếu bàn trống return về TRUE, ngược lại có người return về FALSE
        public bool GetTableStatusByID(int id)
        {
            int result;
            result = (int)DataProvider.Instance.ExecuteScalar("SELECT COUNT(*) FROM FoodTable WHERE id = " + id + " AND tStatus = N'Trống'");
            return result > 0;
        }

        public bool InsertTable(string tableName, string tableStatus)
        {
            string query = String.Format("INSERT INTO FoodTable (tName, tStatus) VALUES (N'{0}' , N'{1}')", tableName, tableStatus);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTable(string tableName, string tableStatus, int id)
        {
            string query = String.Format("UPDATE FoodTable SET tName = N'{0}' , tStatus = N'{1}' WHERE id = {2}", tableName, tableStatus, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int tableID)
        {
            BillDAO.Instance.DeleteBillByTableID(tableID);
            string query = "DELETE FoodTable WHERE id = " + tableID;
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        // NÊU TÊN BÀN ĐÃ TỒN TẠI THÌ TRẢ VỀ TRUE, NGƯỢC LẠI TRẢ VỀ FALSE
        public bool CheckExistTable(string tableName, string tableStatus)
        {
            int result;
            string query = String.Format("SELECT COUNT(*) FROM FoodTable WHERE tName =  N'{0}' AND (tStatus = N'{1}')", tableName, tableStatus);
            result = (int)DataProvider.Instance.ExecuteScalar(query);

            return result > 0;
        }
    }
}
