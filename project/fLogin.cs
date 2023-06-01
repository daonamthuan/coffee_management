using project.DAO;
using project.DTO;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace project
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassword.Text;

            // Check valid username and password.
            if (username.Trim().Contains(" ") || !Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*\W).{8,}$"))
            {
                if (username.Trim().Contains(" ")) lbValideUsername.Visible = true;
                if (!Regex.IsMatch(password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*\W).{8,}$")) lbValidPassword.Visible = true;
                return;
            }

            if (Login(username, password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(username);
                fTableManager f = new fTableManager(loginAccount);

                lbValideUsername.Visible = false;
                lbValidPassword.Visible = false;
                // dialog la thao tac chi duoc xu li tren top most, no cung la top most
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Sai tên thông tin đăng nhập", "Đăng nhập thất bại");
            }
            
        }
        bool Login(string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?", "Notification", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}