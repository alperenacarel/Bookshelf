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
    public partial class formRegister : Form
    {
        string[] easy_passwords = { "1234567", "12345678", "123456789","password", "qwertyu","abc1234", "abcd1234", "" };
        formLogin form_log = null;
        public formRegister(formLogin form_log)
        {
            this.form_log = form_log;
            InitializeComponent();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
            form_log.Visible = true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearControls();
            firstNameTextBox.Select();
        }

        private void clearControls()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text = string.Empty;
            }
        }

        private void formRegister_Load(object sender, EventArgs e)
        {
  
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons btn = MessageBoxButtons.OK;
            MessageBoxIcon ico = MessageBoxIcon.Information;
            string caption = "Save Data";

            if (string.IsNullOrEmpty(firstNameTextBox.Text))
            {   
                MessageBox.Show("Please enter First Name.", caption,btn,ico);
                firstNameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(lastNameTextBox.Text))
            {
                MessageBox.Show("Please enter Last Name.", caption, btn, ico);
                lastNameTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(usernameTextBox.Text))
            {
                MessageBox.Show("Please enter Username.", caption, btn, ico);
                usernameTextBox.Select();
                return;
            }
            
            var res = emailTextBox.Text.Substring(emailTextBox.Text.Length - 4);

            if (emailTextBox.Text.Contains("@") == false || res != ".com")
            {
                MessageBox.Show("Please enter a valid e-mail address", caption, btn, ico);
                emailTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show("Please enter Password.", caption, btn, ico);
                passwordTextBox.Select();
                return;
            }

            int pos = Array.IndexOf(easy_passwords, passwordTextBox.Text);

            if (pos > -1)
            {
                MessageBox.Show("It is an easy password.Please enter more secure password.", caption, btn, ico);
                passwordTextBox.Select();
                return;
            }

            if (passwordTextBox.Text.Length <= 6) 
            {
                MessageBox.Show("Password length must be more than 6.", caption, btn, ico);
                passwordTextBox.Select();
                return;
            }

            if (string.IsNullOrEmpty(confirmPasswordTextBox.Text))
            {
                MessageBox.Show("Please Confirm Your Password.", caption, btn, ico);
                confirmPasswordTextBox.Select();
                return;
            }

            if (passwordTextBox.Text != confirmPasswordTextBox.Text)
            {
                MessageBox.Show("Your password and confirmation password do not match.", caption, btn, ico);
                confirmPasswordTextBox.SelectAll();
                return;
            }           

            string sql = @"SELECT Username FROM Login_Table WHERE Username = '"+usernameTextBox.Text+"';";
            DataTable checkDuplicates = sqliteDatabase.execSqlite(sql);

            if(checkDuplicates.Rows.Count > 0)
            {
                MessageBox.Show("The username already taken. Plaease try another username.",
                    "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                usernameTextBox.SelectAll();
                return;
            }

            DialogResult result;
            result = MessageBox.Show("Do you want to save the record?", 
                "Save Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                string sql2 = @"INSERT INTO Login_Table(First_Name, Last_Name, Email_Adress, Username, Password)
                                VALUES('" + firstNameTextBox.Text + "', '" + lastNameTextBox.Text + "', '" + emailTextBox.Text + "' , " +
                                " '" + usernameTextBox.Text + "' , '" + passwordTextBox.Text +"');";

                sqliteDatabase.execSqlite(sql2);

                MessageBox.Show("You have registered successfully.",
                "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

                form_log.Show();                
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                passwordTextBox.UseSystemPasswordChar = false;
                confirmPasswordTextBox.UseSystemPasswordChar = false;
            }
            else
            {
                passwordTextBox.UseSystemPasswordChar = true;
                confirmPasswordTextBox.UseSystemPasswordChar = true;
            }
        }
    }
}
