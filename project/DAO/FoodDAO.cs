using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return instance; }
            private set { instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetListFoodByCategoryID(int id)
        {
            List<Food> listFood = new List<Food> ();

            string query = "SELECT * FROM Food WHERE idCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }

        public List<Food> GetListFood()
        {
            List<Food> listFood = new List<Food>();
            string query = "SELECT * FROM FOOD";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }

        public bool InsertFood(string name, int idCategory, float price)
        {
            string query = "INSERT INTO Food (fName , idCategory , price) VALUES (N'" + name + "' , " + idCategory + " , " + price + " )";
            int result =  DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateFood(int idFood, string name, int idCategory, float price)
        {
            string query = String.Format("UPDATE Food SET fName = N'{0}' , idCategory = {1}, price = {2} WHERE id = {3}", name, idCategory, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillDetailDAO.Instance.DeleteBillDetailByFoodID(idFood);
            string query = "DELETE Food WHERE id = " + idFood;
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public List<Food> SearchFoodByName (string name)
        {
            List<Food> listFood = new List<Food>();
            string query = String.Format("SELECT * FROM Food WHERE [dbo].[fuConvertToUnsign1](fName) LIKE N'%' + [dbo].[fuConvertToUnsign1](N'{0}') + '%' ", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }

        // Khi xóa category thì xóa tất cả những món trong Category
        public void DeleteFoodByCategoryID(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("DELETE Food WHERE idCategory = " + id);
        }

        public bool CheckExistFood(string foodName) 
        {
            return (int)DataProvider.Instance.ExecuteScalar("SELECT COUNT (*) FROM Food WHERE fName = N'" + foodName + "'") > 0;
        }
    }
}
