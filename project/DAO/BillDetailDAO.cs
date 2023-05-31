using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class BillDetailDAO
    {
        private static BillDetailDAO instance;
        
        public static BillDetailDAO Instance
        {
            get { if (instance == null) instance = new BillDetailDAO(); return instance; }
            private set { instance = value; }
        }

        private BillDetailDAO() { }
        public List<BillDetail> GetListBillDetail (int id)
        {
            List<BillDetail> listBillDetail = new List<BillDetail>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM BillDetail WHERE idBill = " + id);
            foreach (DataRow item in data.Rows)
            {
                BillDetail detail = new BillDetail(item);
                listBillDetail.Add(detail);
            }

            return listBillDetail;
        }

        public void InsertBillDetail(int idBill, int idFood, int quantity)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillDetail @idBill , @idFood , @quantity ", new object[] { idBill, idFood, quantity });
        }

        // Khi xóa Food thì phải xóa những BillDetail có chứa Food
        public void DeleteBillDetailByFoodID(int idFood)
        {
            DataProvider.Instance.ExecuteNonQuery("DELETE BillDetail WHERE idFood = " + idFood);
        }
    }
}
