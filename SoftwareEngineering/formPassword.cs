using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using SoftwareEngineering.Connection;

namespace SoftwareEngineering
{
    public partial class formPassword : Form
    {
        public formPassword()
        {
            InitializeComponent();
        }

        private void formPassword_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(emailTextBox.Text))
            {
                SmtpClient sc = new SmtpClient();
                string email = emailTextBox.Text;

                sc.Port = 587;
                sc.Host = "smtp.gmail.com";
                sc.EnableSsl = true;

                sc.Credentials = new NetworkCredential("bookshelf.app.swe@gmail.com", "bookshelf2021");

                string mySQL = @"SELECT Username, Password FROM Login_Table
                          WHERE Email_Adress = '" + email + "';";


                DataTable userData = sqliteDatabase.execSqlite(mySQL);

                string username = "";
                string password = "";

                foreach (DataRow row in userData.Rows)
                {
                    username = row["Username"].ToString();
                    password = row["Password"].ToString();
                }

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("bookshelf.app.swe@gmail.com", "Bookshelf App");
                    mail.To.Add(email);
                    mail.Subject = "Username and Password Information"; mail.IsBodyHtml = true;
                    mail.Body = "Username: '" + username + "' Password: '" + password + "'";
                    sc.Send(mail);

                    MessageBox.Show("We have send an email.");

                    this.Close();
                }
                else
                {
                    MessageBox.Show("This email is not registered");
                    return;
                }
            }
        }

        private void formPassword_FormClosed(object sender, FormClosedEventArgs e)
        {
            formLogin login = new formLogin();
            login.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
