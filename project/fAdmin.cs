using project.DAO;
using project.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class fAdmin : Form
    {

        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
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
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadListFood();
            LoadListAccount();
            LoadCategoryIntoCombobox(cbbFoodCategory);
            AddFoodBinding();
            AddAccountBinding();
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDate(checkIn, checkOut);
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

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
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
            txbAccountUsername.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "aUsername", true, DataSourceUpdateMode.Never));
            txbAccountFullname.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "displayName", true, DataSourceUpdateMode.Never));
            txbAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "aType", true, DataSourceUpdateMode.Never));
        }

        void LoadListAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddAccount(string username, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(username, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại", "Thông báo");
            }
            LoadListAccount();
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

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công", "Thông báo");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else 
            {
                MessageBox.Show("Lỗi thêm món không thành công", "Thông báo");
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
                MessageBox.Show("Chỉnh sửa món thành công", "Thông báo");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Lỗi chỉnh sửa món không thành công", "Thông báo");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Xóa món thành công", "Thông báo");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Lỗi xóa món không thành công", "Thông báo");
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
            int type = Convert.ToInt32(txbAccountType.Text);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {

        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {

        }
    }
}
