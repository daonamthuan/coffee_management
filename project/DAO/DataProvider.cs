using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace project.DAO
{
    public class DataProvider
    {
        // static: thành viên của lớp thuộc về lớp chứ không thuộc về 1 thể hiện cụ thể của lớp (chỉ được tạo 1 lần duy nhất)
        private static DataProvider instance;

        public static DataProvider Instance 
        { 
            get {if (instance == null) {instance = new DataProvider();}; return instance; }
            private set { DataProvider.instance = value; } 
        }

        private DataProvider() { }

        private string connectionSTR = @"Data Source=NAUHT;Initial Catalog=COFFEESHOPMANAGEMENT;Integrated Security=True";
        public DataTable ExecuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionSTR)) // sau khi ket thuc khoi lenh thi doi tuong trong using cung duoc giai phong
            {
                connection.Open();

                // cmd : thuc thi cau query tren connection da chon
                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listParam = query.Split(' ');
                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i++]);
                        }
                    }
                }

                // trung gian de lay du lieu tu cau truy van
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                // fill vao bang data da~ tao.
                adapter.Fill(data);

                connection.Close();
            }
            return data;  
        }

        // Insert, Update, Delete, trả về số dòng thành công
        public int ExecuteNonQuery(string query, object[] parameter = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR)) // sau khi ket thuc khoi lenh thi doi tuong trong using cung duoc giai phong
            {
                connection.Open();

                // cmd : thuc thi cau query tren connection da chon
                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listParam = query.Split(' ');
                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i++]);
                        }
                    }
                }
                data = cmd.ExecuteNonQuery();

                connection.Close();
            }
            return data;
        }

        // Count(*)
        public object ExecuteScalar(string query, object[] parameter = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR)) // sau khi ket thuc khoi lenh thi doi tuong trong using cung duoc giai phong
            {
                connection.Open();

                // cmd : thuc thi cau query tren connection da chon
                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listParam = query.Split(' ');
                    int i = 0;
                    foreach (string item in listParam)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameter[i++]);
                        }
                    }
                }
                data = cmd.ExecuteScalar();

                connection.Close();
            }
            return data;
        }
    }
}
    