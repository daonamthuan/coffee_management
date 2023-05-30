namespace project
{
    partial class fAccountProfile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.usn = new System.Windows.Forms.Label();
            this.txbUsername = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txbDisplayName = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txbPassword = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txbNewPassword = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txbReEnterPassword = new System.Windows.Forms.TextBox();
            this.btnUpdateProfile = new System.Windows.Forms.Button();
            this.btnExitUpdateProfile = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.usn);
            this.panel2.Controls.Add(this.txbUsername);
            this.panel2.Location = new System.Drawing.Point(12, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(450, 55);
            this.panel2.TabIndex = 1;
            // 
            // usn
            // 
            this.usn.AutoSize = true;
            this.usn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.usn.Location = new System.Drawing.Point(3, 21);
            this.usn.Name = "usn";
            this.usn.Size = new System.Drawing.Size(112, 24);
            this.usn.TabIndex = 1;
            this.usn.Text = "Username:";
            // 
            // txbUsername
            // 
            this.txbUsername.Location = new System.Drawing.Point(126, 18);
            this.txbUsername.Name = "txbUsername";
            this.txbUsername.ReadOnly = true;
            this.txbUsername.Size = new System.Drawing.Size(311, 27);
            this.txbUsername.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txbDisplayName);
            this.panel1.Location = new System.Drawing.Point(12, 88);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(450, 55);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(3, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fullname:";
            // 
            // txbDisplayName
            // 
            this.txbDisplayName.Location = new System.Drawing.Point(126, 18);
            this.txbDisplayName.Name = "txbDisplayName";
            this.txbDisplayName.Size = new System.Drawing.Size(311, 27);
            this.txbDisplayName.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txbPassword);
            this.panel3.Location = new System.Drawing.Point(12, 149);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(440, 56);
            this.panel3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // txbPassword
            // 
            this.txbPassword.Location = new System.Drawing.Point(126, 18);
            this.txbPassword.Name = "txbPassword";
            this.txbPassword.Size = new System.Drawing.Size(311, 27);
            this.txbPassword.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.AutoSize = true;
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.txbNewPassword);
            this.panel4.Location = new System.Drawing.Point(12, 207);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(440, 56);
            this.panel4.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(3, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 24);
            this.label3.TabIndex = 1;
            this.label3.Text = "New Pword:";
            // 
            // txbNewPassword
            // 
            this.txbNewPassword.Location = new System.Drawing.Point(141, 18);
            this.txbNewPassword.Name = "txbNewPassword";
            this.txbNewPassword.Size = new System.Drawing.Size(296, 27);
            this.txbNewPassword.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.AutoSize = true;
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.txbReEnterPassword);
            this.panel5.Location = new System.Drawing.Point(12, 269);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(443, 56);
            this.panel5.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(3, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 24);
            this.label4.TabIndex = 1;
            this.label4.Text = "Re-enter Pw:";
            // 
            // txbReEnterPassword
            // 
            this.txbReEnterPassword.Location = new System.Drawing.Point(141, 18);
            this.txbReEnterPassword.Name = "txbReEnterPassword";
            this.txbReEnterPassword.Size = new System.Drawing.Size(296, 27);
            this.txbReEnterPassword.TabIndex = 0;
            // 
            // btnUpdateProfile
            // 
            this.btnUpdateProfile.Location = new System.Drawing.Point(71, 342);
            this.btnUpdateProfile.Name = "btnUpdateProfile";
            this.btnUpdateProfile.Size = new System.Drawing.Size(139, 51);
            this.btnUpdateProfile.TabIndex = 6;
            this.btnUpdateProfile.Text = "Update";
            this.btnUpdateProfile.UseVisualStyleBackColor = true;
            this.btnUpdateProfile.Click += new System.EventHandler(this.btnUpdateProfile_Click);
            // 
            // btnExitUpdateProfile
            // 
            this.btnExitUpdateProfile.Location = new System.Drawing.Point(238, 342);
            this.btnExitUpdateProfile.Name = "btnExitUpdateProfile";
            this.btnExitUpdateProfile.Size = new System.Drawing.Size(139, 51);
            this.btnExitUpdateProfile.TabIndex = 7;
            this.btnExitUpdateProfile.Text = "Exit";
            this.btnExitUpdateProfile.UseVisualStyleBackColor = true;
            this.btnExitUpdateProfile.Click += new System.EventHandler(this.btnExitUpdateProfile_Click);
            // 
            // fAccountProfile
            // 
            this.AcceptButton = this.btnUpdateProfile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExitUpdateProfile;
            this.ClientSize = new System.Drawing.Size(459, 418);
            this.Controls.Add(this.btnExitUpdateProfile);
            this.Controls.Add(this.btnUpdateProfile);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "fAccountProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin cá nhân";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel2;
        private Label usn;
        private TextBox txbUsername;
        private Panel panel1;
        private Label label1;
        private TextBox txbDisplayName;
        private Panel panel3;
        private Label label2;
        private TextBox txbPassword;
        private Panel panel4;
        private Label label3;
        private TextBox txbNewPassword;
        private Panel panel5;
        private Label label4;
        private TextBox txbReEnterPassword;
        private Button btnUpdateProfile;
        private Button btnExitUpdateProfile;
    }
}