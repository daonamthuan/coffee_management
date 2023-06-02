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
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;
        
        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(LoginAccount); }
        }
        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;
        }

        void ChangeAccount (Account acc)
        {
            txbUsername.Text = LoginAccount.Username;
            txbDisplayName.Text = LoginAccount.DisplayName;
            lbUsername.Text = "@" + LoginAccount.Username;
        }

        void UpdateAccount()
        {
            string displayName = txbDisplayName.Text;
            string username = txbUsername.Text;
            string password = txbPassword.Text;
            string newPassword = txbNewPassword.Text;
            string reenterPassword = txbReEnterPassword.Text;
            lbUsername.Text = txbUsername.Text;
            if (!newPassword.Equals(reenterPassword))
            {
                MessageBox.Show("Vui lòng nhập lại đúng mật khẩu mới !");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccout(username, displayName, password, reenterPassword))
                {
                    MessageBox.Show("Cập nhập thành công");
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đúng mật khẩu");
                }
            }
        }

        private void btnExitUpdateProfile_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }

        private void fAccountProfile_Resize(object sender, EventArgs e)
        {
            int width = this.Size.Width;
            int height = this.Size.Height;
            this.Size = new Size(width, height);
        }
    }
}
