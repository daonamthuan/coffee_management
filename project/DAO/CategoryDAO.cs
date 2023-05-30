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
    }
}
