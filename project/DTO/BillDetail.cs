using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DTO
{
    internal class BillDetail
    {
        public BillDetail (int id, int billID, int foodID, int quantity)
        {
            this.ID = id;
            this.BillID = billID;
            this.FoodID = foodID;
            this.Quantity = quantity;   
        }

        // Constructor chuyen du lieu tu dong` sang
        public BillDetail(DataRow row)
        {
            this.ID = (int)row["id"];
            this.BillID = (int)row["idBill"];
            this.FoodID = (int)row["idFood"];
            this.Quantity = (int)row["quantity"];
        }
        private int iD;
        private int billID;
        private int foodID;
        private int quantity;

        public int ID { get => iD; set => iD = value; }
        public int BillID { get => billID; set => billID = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
