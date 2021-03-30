using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SoftwareEngineering.Connection;

namespace SoftwareEngineering
{
    public partial class formLogin : Form
    {
        
        public string user_id;
        public formLogin()
        {
            sqliteDatabase.createDB();
            InitializeComponent();
        }

        private void loginButton_click(object sender, EventArgs e)
        {
            this.user_id = user_id;

            if(!string.IsNullOrEmpty(usernameTextBox.Text) && 
                !string.IsNullOrEmpty(passwordTextBox.Text))
            {
                string mySQL = string.Empty;

                mySQL = @"SELECT * FROM Login_Table
                          WHERE Username = '" + usernameTextBox.Text + "' and Password = '"+ passwordTextBox.Text +"';";

                
                DataTable userData = sqliteDatabase.execSqlite(mySQL);
                

                foreach (DataRow row in userData.Rows)
                {
                    user_id = row["user_id"].ToString();
                }

                if (userData.Rows.Count > 0)
                {
                    usernameTextBox.Clear();
                    passwordTextBox.Clear();
                    showPasswordCheckBox.Checked = false;

                    this.Hide();
                
                    formMain mainForm = new formMain(this);
                    mainForm.ShowDialog();
                    mainForm = null;

                    this.Show();
                    this.usernameTextBox.Select();
                    
                }
                else
                {
                    MessageBox.Show("The username or password is incorrect. Try again.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    usernameTextBox.Focus();
                    usernameTextBox.SelectAll();
                }
            }
            else
            {
                MessageBox.Show("Please enter username and password.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                usernameTextBox.Select();
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void formLogin_Load(object sender, EventArgs e)
        {
            usernameTextBox.Select();

        }

 
        private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(showPasswordCheckBox.Checked == true)
            {
                passwordTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                passwordTextBox.UseSystemPasswordChar = true;
            }
        }

        private void oPenRegisterFormLinklabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formRegister register = new formRegister(this);
            this.Hide();
            register.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            formPassword forgot = new formPassword();

            this.Hide();
            forgot.Show();
        }
    }
}
