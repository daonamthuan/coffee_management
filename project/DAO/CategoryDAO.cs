using project.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.DAO
{
    internal class CategoryDAO
    {
        private static CategoryDAO instance;

        internal static CategoryDAO Instance 
        { 
            get { if (instance == null) instance = new CategoryDAO(); return instance; }
            private set { instance = value; } 
        }

        private CategoryDAO() { }

        // DAO: lop tiep can data, => goi ham tra ve danh sach data
        public List<Category> GetListCategory()
        {
            List<Category> listCategory = new List<Category>();

            string query = "SELECT * FROM FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }

        public Category GetCategoryByID (int id)
        {
            string query = "SELECT c.id , c.fcName FROM FoodCategory AS c, Food AS f WHERE f.idCategory = c.id AND f.id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                return new Category(item);
            }

            return null;
        }

        public bool InsertCategory (string categoryName)
        {
            string query = "INSERT INTO FoodCategory (fcName) VALUES (N'" + categoryName + "')";
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(string categoryName, int id)
        {
            string query = String.Format("UPDATE FoodCategory SET fcName = N'{0}' WHERE id = {1}", categoryName, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int idCategory)
        {
            FoodDAO.Instance.DeleteFoodByCategoryID(idCategory);
            string query = "DELETE FoodCategory WHERE id = " + idCategory;
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        // Trả về true nếu Category tồn tại
        public bool CheckExistCategory(string categoryName)
        {
            int result;
            result = (int)DataProvider.Instance.ExecuteScalar("SELECT COUNT(*) FROM FoodCategory WHERE fcName =  N'" + categoryName + "'");

            return result > 0;
        }
    }
}
