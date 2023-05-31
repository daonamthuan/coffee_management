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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project
{
    public partial class fTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount 
        { 
            get { return loginAccount; }
            set {loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            // Truyền tài khoản đăng nhập
            this.LoginAccount = acc;

            LoadTable();

            LoadCategory();
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            //thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbbCategory.DataSource = listCategory;
            cbbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategoryID(id);
            cbbFoodByCategory.DataSource = listFood;
            cbbFoodByCategory.DisplayMember= "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList =  TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width=TableDAO.TableWidth, Height=TableDAO.TableHeight };
                btn.Text = item.Name + "\n" + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item;
                if (item.Status == "Trống")
                {
                    btn.BackColor = Color.FromArgb(255, 252, 215);
                }
                else 
                {
                    btn.BackColor = Color.FromArgb(189, 25, 42);
                }
                flpTable.Controls.Add(btn);
            }
        }

        // show bill từ ID Table
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu> listMenu = MenuDAO.Instance.GetListMenuByTableID(id);
            float totalBillPrice = 0;
            foreach (Menu item in listMenu)
            {
                // 1 item chinh'
                ListViewItem lsvItem = new ListViewItem(item.FoodName);
                // nhieu item phu.
                lsvItem.SubItems.Add(item.Quantity.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalBillPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem); 
            }
            CultureInfo culture = new CultureInfo("vi-VN");
            // Chuyển luồng ĐANG CHẠY culture thành en-US, không ảnh hưởng thread
            Thread.CurrentThread.CurrentCulture = culture;  
            txbTotalBillPrice.Text = totalBillPrice.ToString("c");
            txbTotalBillPrice.ForeColor = Color.Red;
        }   

        void Btn_Click(object? sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        #endregion

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
        }

        private void F_UpdateFood(object? sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)    
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_DeleteFood(object? sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object? sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // sender gui event la Combobox
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;
            Category selected = cb.SelectedItem as Category;
            int id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            // Lấy Table hiện tại muốn AddFood
            Table table = lsvBill.Tag as Table;
            // Lay idBill hien tai, không có thì trả về -1 (bàn mới)
            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn để thêm món ăn!", "Thông báo");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            // Lay foodID muon them
            int foodID = (cbbFoodByCategory.SelectedItem as Food).ID;
            int quantity = (int)nmFoodQuantity.Value;

            // Trường hợp add Bill mới vào bàn (bàn trống)
            if (idBill == -1 && quantity > 0)
            {
                // chua co bill => them moi
                // Insert Bill và BillDetail và database
                BillDAO.Instance.InserrBill(table.ID);
                BillDetailDAO.Instance.InsertBillDetail(BillDAO.Instance.GetMaxIDBill(), foodID, quantity);
            }
            else // Trường hợp Bill của bàn đã tồn tại
            {
                // Thêm chi tiết vào bảng BillDetail có idBill của hiện tại
                // 2 tr hợp: bill đã có món muốn thêm, bill chưa có món muốn thêm xử lí bên dưới database
                BillDetailDAO.Instance.InsertBillDetail(idBill, foodID, quantity);
            }

            ShowBill(table.ID);
            LoadTable();

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            // Lay Table hiện tại
            Table table = lsvBill.Tag as Table;
            // Lay IDBill, trả về -1 nếu không có bill cho bàn (bàn mới)
            if (table == null)
            {
                MessageBox.Show("Bạn chưa chọn bàn để thanh toán!", "Thông báo");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            // int discount = (int)nmDiscount.Value;
            float finalTotalPrice = (float)Convert.ToDouble(txbTotalBillPrice.Text.Split(',')[0]);
            // Kiem tra bill
            if (idBill != -1)
            {
                if (MessageBox.Show("Thanh toán hóa đơn cho " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, finalTotalPrice);
                    ShowBill(table.ID);
                    LoadTable();
                }
            }
        }

        private void fTableManager_Resize(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;
            this.Size = new Size(width, height);
        }
    }
}
