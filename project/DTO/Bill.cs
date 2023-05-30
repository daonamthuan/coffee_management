using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DTO
{
    internal class Bill
    {
        public Bill(int id, DateTime? dateCheckin, DateTime? dateCheckout, int status)
        {
            this.ID = id;
            this.DateCheckIn = dateCheckin;
            this.DateCheckOut = dateCheckout;
            this.Status = status;

        }

        private int iD;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut; // ? nghia la gia tri duoc phep null
        private int status;

        public int ID { get => iD; set => iD = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }

        public Bill (DataRow row)
        {
            this.ID = (int)row["id"];
            this.DateCheckIn = (DateTime?)row["dateCheckin"];

            var dateCheckOutTemp = row["dateCheckout"];
            if (dateCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dateCheckOutTemp;

            //this.DateCheckOut = (DateTime?)row["dateCheckin"];
            this.Status =(int)row["billStatus"];
        }
    }
}
