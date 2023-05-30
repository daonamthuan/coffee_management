using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        // Thành công: Lấy được BillID
        // Thất bại: -1
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Bill WHERE idTable = " + id + " AND billStatus = 0");

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }

        public void InserrBill(int id)
        {
            // id is idTable
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBill @idTable", new object[] {id});
        }

        public int GetMaxIDBill()
        {
            return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM Bill");
            
        }

        public void CheckOut(int id, float totalPrice)
        {
            string query = "UPDATE Bill SET dateCheckout = GETDATE() , billStatus = 1 , totalPrice = " + totalPrice + " WHERE id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public DataTable GetListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }

    }
}
