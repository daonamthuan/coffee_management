using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class MenuDAO
    {
        private static MenuDAO instance;

        internal static MenuDAO Instance 
        { 
            get { if (instance == null) instance = new MenuDAO(); return instance; }
            set { instance = value; }
        }

        private MenuDAO() { }
        
        public List<Menu> GetListMenuByTableID(int id)
        {
            List<Menu> listMenu = new List<Menu>();
            string query = "SELECT f.fName, bd.quantity, f.price, f.price*bd.quantity as totalPrice FROM BillDetail as bd, Bill as b, Food as f WHERE b.id = bd.idBill AND f.id = bd.idFood AND b.idTable = " + id + " AND b.billStatus = 0";
            
            // tra ve 1 bang
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            
            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }
    }
}
