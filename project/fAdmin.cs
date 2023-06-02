using project.DAO;
using project.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class fAdmin : Form
    {

        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource categoryList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }

        #region method
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvTable.DataSource = tableList;
            dtgvCategory.DataSource = categoryList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            LoadListFood();
            LoadListAccount();
            LoadListTable();
            LoadListCategory();

            LoadCategoryIntoCombobox(cbbFoodCategory);

            AddFoodBinding();
            AddAccountBinding();
            AddTableBinding();
            AddCategoryBinding();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(checkIn, checkOut, 1);
            LoadStatistic();
        }

        void LoadStatistic()
        {
            CultureInfo culture = new CultureInfo("vi-VN");
            // Chuyển luồng ĐANG CHẠY culture thành en-US, không ảnh hưởng thread
            Thread.CurrentThread.CurrentCulture = culture;
            txbTotalNumberBill.Text = BillDAO.Instance.GetNumberBillByDate(dtpkFromDate.Value, dtpkToDate.Value).ToString();
            txbTotalPrice.Text = BillDAO.Instance.GetTotalPriceByDate(dtpkFromDate.Value, dtpkToDate.Value).ToString("c");
            txbTotalPrice.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }

        void AddFoodBinding()
        {
            // Thuoc tinh "Text" thay doi theo "Name", nguồn là DataSource
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFood.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }


        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.GetListTable();
        }
        
        List<Food> SearchFoodByName (string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        void AddAccountBinding ()
        {
            // Combobox LOAD sau
            // Thuoc tinh "Text" thay doi theo "Name", nguồn là DataSource
            txbAccountUsername.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Username", true, DataSourceUpdateMode.Never));
            txbAccountFullname.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Displayname", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        // LoadListAccount : cho BindingSource accountList = DataTable trả về
        void LoadListAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(username, displayName, type))
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Thêm tài khoản mới thành công!";
                lbNotification.ForeColor = Color.Green;
            }
            else
            {
                lbNotification.Text = "Lỗi : Tên đăng nhập đã tồn tại trong hệ thống";
                lbNotification.Visible = true;
                lbNotification.ForeColor = Color.Red;
            }
            LoadListAccount();
        }

        void EditAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(username, displayName, type))
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Chỉnh sửa thông tin tài khoản thành công!";
                lbNotification.ForeColor = Color.Green;
            }
            else
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Chỉnh sửa thông tin tài khoản thành công!";
                lbNotification.ForeColor = Color.Red;
            }
            LoadListAccount();
        }

        void DeleteAccount(string username)
        {
            if (AccountDAO.Instance.DeleteAccount(username))
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Xóa tài khoản thành công!";
                lbNotification.ForeColor = Color.Green;
            }
            else
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Xóa tài khoản không thành công!";
                lbNotification.ForeColor = Color.Red;
            }
            LoadListAccount();
        }

        void ResetPassword(string username)
        {
            if (AccountDAO.Instance.ResetPassword(username))
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Đặt lại mật khẩu thành công, mật khẩu mới là \"12345678Aa@\"";
                lbNotification.ForeColor = Color.Green;
            }
            else
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Đặt lại mật khẩu tài khoản KHÔNG thành công!";
                lbNotification.ForeColor = Color.Red;
            }
        }

        bool CheckValidUsernameAndPassword(string username, string password)
        {
            if (username.Trim().Contains(" "))
            {
                MessageBox.Show("Tên đăng nhập không được chứa khoảng trắng", "Thông báo");
                return false;
            }
            if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*\W).{8,}$"))
            {
                MessageBox.Show("Mật khẩu phải ít nhất 8 ký tự và phải bao gồm cả chữ, số, ký tự đặc biệt!", "Thông báo");
                return false;
            }

            return true;
        }

        void AddCategory(string categoryName)
        {
            if (CategoryDAO.Instance.InsertCategory(categoryName))
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Thành công : Thêm danh mục món thành công!";
                lbNotifyCategory.ForeColor = Color.Green;
            }
            else
            {
                lbNotifyCategory.Text = "Lỗi : Thêm danh mục món không thành công";
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.ForeColor = Color.Red;
            }
            LoadListCategory();
        }

        void UpdateCategory(string categoryName, int id)
        {
            if (CategoryDAO.Instance.UpdateCategory(categoryName, id))
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Thành công : Chỉnh sửa mục món thành công!";
                lbNotifyCategory.ForeColor = Color.Green;
            }
            LoadListCategory();
        }
   
        void DeleteCategory(int id)
        {
            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Thành công : Xóa danh mục món thành công!";
                lbNotifyCategory.ForeColor = Color.Green;
            }
            LoadListCategory();
        }

        void AddTable(string tableName, string tableStatus)
        {
            if (TableDAO.Instance.InsertTable(tableName, tableStatus ))
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Thành công : Thêm bàn thành công!";
                lbNotifyTable.ForeColor = Color.Green;
            }
            LoadListTable();
        }

        void UpdateTable(string tableName, string tableStatus, int tableID)
        {
            if (TableDAO.Instance.UpdateTable(tableName, tableStatus, tableID))
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Thành công : Chỉnh sửa bàn thành công!";
                lbNotifyTable.ForeColor = Color.Green;
            }
            LoadListTable();
        }

        void DeleteTable(int tableID)
        {
            if (TableDAO.Instance.DeleteTable(tableID))
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Thành công : Xóa bàn thành công!";
                lbNotifyTable.ForeColor = Color.Green;
            }
            LoadListTable();
        }

        #endregion



        #region event
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private void fAdmin_Resize(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;
            this.Size = new Size(width, height);
        }

        // Khi txbFoodID thay đổi thì cbbFoodCategory.SelectedIem cũng thay đổi theo cái ID
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            { 
                int idFood = -1;
                Category category = null;
                if (Int32.TryParse(txbFoodID.Text, out idFood))
                {
                    category = CategoryDAO.Instance.GetCategoryByID(idFood);
                    //cbbFoodCategory.SelectedIndex = category.ID;
                }
                cbbFoodCategory.SelectedItem = category;
                int index = -1;
                int i = 0;
                foreach (Category item in cbbFoodCategory.Items)
                {
                    if (category != null && item.ID == category.ID)
                    {
                        index = i; // tìm thấy ở vòng lặp thứ i tức là có index = i trong cbb
                        break;
                    }
                    i++;
                }
                cbbFoodCategory.SelectedIndex = index;
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFood.Value;

            if (FoodDAO.Instance.CheckExistFood(name))
            {
                lbNotifyFood.Text = "Lỗi : Thêm không thành công do món đã tồn tại trong hệ thống";
                lbNotifyFood.ForeColor = Color.Red;
                lbNotifyFood.Visible = true;
                return;
            }    

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                lbNotifyFood.Text = "Thành công: Thêm món vào Menu thành công";
                lbNotifyFood.ForeColor = Color.Green;
                lbNotifyFood.Visible = true;
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else 
            {
                lbNotifyFood.Text = "Lỗi : Thêm món vào Menu không thành công";
                lbNotifyFood.ForeColor = Color.Red;
                lbNotifyFood.Visible = true;
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txbFoodID.Text);
            string name = txbFoodName.Text;
            int categoryID = (cbbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFood.Value;

            if (FoodDAO.Instance.UpdateFood(idFood, name, categoryID, price))
            {
                lbNotifyFood.Text = "Thành công: Chỉnh sửa món thành công!";
                lbNotifyFood.ForeColor = Color.Green;
                lbNotifyFood.Visible = true;
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                lbNotifyFood.Text = "Lỗi : Chỉnh sửa món KHÔNG thành công!";
                lbNotifyFood.ForeColor = Color.Red;
                lbNotifyFood.Visible = true;
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                lbNotifyFood.Text = "Thành công: Xóa món khỏi Menu thành công!";
                lbNotifyFood.ForeColor = Color.Green;
                lbNotifyFood.Visible = true;
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                lbNotifyFood.Text = "Lỗi : Xóa món khỏi Menu KHÔNG thành công!";
                lbNotifyFood.ForeColor = Color.Red;
                lbNotifyFood.Visible = true;
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            string name = txbSearchFood.Text;
            foodList.DataSource = SearchFoodByName(name);
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        #endregion

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;
            string displayname = txbAccountFullname.Text;
            int type = Convert.ToInt32(nmType.Value);
            if (username == "" || displayname == "")
            {
                MessageBox.Show("Bạn phải điền đầy đủ thông tin", "Thông báo");
                return;
            }
            if (!AccountDAO.Instance.CheckUsernameExist(txbAccountUsername.Text))
            {
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Thêm tài khoản mới thành công!";
                lbNotification.ForeColor = Color.Green;
                AddAccount(username, displayname, type);
            }
            else
            {
                lbNotification.Text = "Lỗi : Tên đăng nhập đã tồn tại trong hệ thống";
                lbNotification.Visible = true;
                lbNotification.ForeColor = Color.Red;
                return;
            }
            //if (username.Trim().Contains(" "))
            //{
            //    MessageBox.Show("Tên đăng nhập không được chứa khoảng trắng", "Thông báo");
            //    return;
            //}

           
        }


        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;
            string displayname = txbAccountFullname.Text;
            int type = Convert.ToInt32(nmType.Value);

            if (username == "" || displayname == "")
            {
                MessageBox.Show("Bạn phải điền đầy đủ thông tin", "Thông báo");
                return;
            }
            if (!AccountDAO.Instance.CheckUsernameExist(username))
            {
                lbNotification.ForeColor = Color.Red;
                lbNotification.Text = "Lỗi : Không được chỉnh sửa \"Tên đăng nhập\"!";
                lbNotification.Visible = true;
            }
            else
            {
                EditAccount(username, displayname, type);
                lbNotification.Visible = true;
                lbNotification.Text = "Thành công : Chỉnh sửa thông tin tài khoản thành công!";
                lbNotification.ForeColor = Color.Green;
            }    
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string username = txbAccountUsername.Text;

            if (loginAccount.Username != username)
            {
                DeleteAccount(username);
            }
            else
            {
                lbNotification.Text = "Lỗi : Tài khoản hiện đang đăng nhập trong hệ thống";
                lbNotification.Visible = true;
                lbNotification.ForeColor = Color.Red;
            }
            
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumberBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 14;

            if (sumRecord % 14 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbNumPage_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            if (page > 1)
                page--;
            txbPageBill.Text = page.ToString();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumberBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }

        private void dtgvBill_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0) // Kiểm tra dòng chẵn
            {
                // Đặt màu nền cho dòng chẵn
                dtgvBill.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                // Đặt màu nền cho dòng lẻ
                dtgvBill.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dtgvFood_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0) // Kiểm tra dòng chẵn
            {
                // Đặt màu nền cho dòng chẵn
                dtgvFood.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                // Đặt màu nền cho dòng lẻ
                dtgvFood.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dtgvCategory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0) // Kiểm tra dòng chẵn
            {
                // Đặt màu nền cho dòng chẵn
                dtgvCategory.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                // Đặt màu nền cho dòng lẻ
                dtgvCategory.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dtgvTable_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0) // Kiểm tra dòng chẵn
            {
                // Đặt màu nền cho dòng chẵn
                dtgvTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                // Đặt màu nền cho dòng lẻ
                dtgvTable.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void dtgvAccount_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex % 2 == 0) // Kiểm tra dòng chẵn
            {
                // Đặt màu nền cho dòng chẵn
                dtgvAccount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            }
            else
            {
                // Đặt màu nền cho dòng lẻ
                dtgvAccount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận đặt lại mật khẩu cho " + txbAccountUsername.Text , "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                ResetPassword(txbAccountUsername.Text);
            }
            
        }

        private void txbTableID_TextChanged(object sender, EventArgs e)
        {
            cbbTableStatus.DataSource = new[] { "Có người", "Trống" };
            int idTable = Convert.ToInt32(txbTableID.Text);

            if (TableDAO.Instance.GetTableStatusByID(idTable))
                cbbTableStatus.SelectedItem = "Trống";
            else
                cbbTableStatus.SelectedItem = "Có người";

            
        }

        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txbCategoryName.Text;

            if (categoryName == "")
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Lỗi : Không được để trống danh mục.";
                lbNotifyCategory.ForeColor = Color.Red;
                return;
            }
            if (CategoryDAO.Instance.CheckExistCategory(categoryName))
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Lỗi : Tên danh mục đã tồn tại trong hệ thống.";
                lbNotifyCategory.ForeColor = Color.Red;
                return;
            }    
            AddCategory(categoryName);
        }


        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txbCategoryName.Text;
            int categoryID = Convert.ToInt32(txbCategoryID.Text);

            if (categoryName == "")
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Lỗi : Không được để trống danh mục.";
                lbNotifyCategory.ForeColor = Color.Red;
                return;
            }
            if (CategoryDAO.Instance.CheckExistCategory(categoryName))
            {
                lbNotifyCategory.Visible = true;
                lbNotifyCategory.Text = "Lỗi : Tên danh mục đã tồn tại trong hệ thống.";
                lbNotifyCategory.ForeColor = Color.Red;
                return;
            }
            UpdateCategory(categoryName, categoryID);
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int categoryID = Convert.ToInt32(txbCategoryID.Text);
            string categoryName = txbCategoryName.Text;
            DeleteCategory(categoryID);                
        }

        private void btnViewTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string tableName = txbTableName.Text;
            string tableStatus = cbbTableStatus.SelectedValue.ToString();

            if (tableName == "" || tableStatus == "")
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Lỗi : Không được để trống tên bàn hoặc trạng thái bàn.";
                lbNotifyTable.ForeColor = Color.Red;
                return;
            }
            if (TableDAO.Instance.CheckExistTable(tableName, "Có người' OR tStatus = N'Trống"))
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Lỗi : Tên bàn đã tồn tại trong hệ thống.";
                lbNotifyTable.ForeColor = Color.Red;
                return;
            }

            AddTable(tableName, tableStatus);
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string tableName = txbTableName.Text;
            string tableStatus = cbbTableStatus.SelectedValue.ToString();
            int tableID = Convert.ToInt32(txbTableID.Text);

            if (tableName == "" || tableStatus == "")
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Lỗi : Không được để trống tên bàn hoặc trạng thái bàn.";
                lbNotifyTable.ForeColor = Color.Red;
                return;
            }
            if (TableDAO.Instance.CheckExistTable(tableName, tableStatus))
            {
                lbNotifyTable.Visible = true;
                lbNotifyTable.Text = "Lỗi : Tên bàn đã tồn tại trong hệ thống.";
                lbNotifyTable.ForeColor = Color.Red;
                return;
            }
            UpdateTable(tableName, tableStatus, tableID);
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int tableID = Convert.ToInt32(txbTableID.Text);
            DeleteTable(tableID);
        }
    }
}
