using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using HtmlAgilityPack;
using SoftwareEngineering.Connection;

namespace SoftwareEngineering
{
    public partial class formSearch : Form
    {
        private List<string> book_id = new List<string>();
        formLogin frm1 = null;
        formMain frm_main = null;
        private string book_name;

        public formSearch(formLogin frm, formMain f_main)
        {
            this.frm1 = frm;
            this.frm_main = f_main;
            InitializeComponent();
        }

        private void formSearch_Load(object sender, EventArgs e)
        {

        }

        private string define_adress()
        {
            string adress = "https://www.gutenberg.org/ebooks/search/?query=";
            book_name = search_box.Text;
            book_name = book_name.Replace(" ", "+");
            adress = adress + book_name;
            book_name = string.Empty;

            return adress;
        }

        private void htmlSearch(string adr)
        {
            HtmlWeb client = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = client.Load(adr);
            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//li[@class='booklink']/a[@class='link']");

            if (Nodes != null)
            {
                foreach (var link in Nodes)
                {
                    book_id.Add(link.Attributes["href"].Value);
                }

                if (book_id.Count() > 0)
                {
                    HtmlWeb client2 = new HtmlWeb();

                    foreach (string id in book_id)
                    {
                        HtmlAgilityPack.HtmlDocument doc2 = client.Load("https://www.gutenberg.org" + id);
                        HtmlNodeCollection Nodes2 = doc2.DocumentNode.SelectNodes("//meta[@name='title']");

                        foreach (var link2 in Nodes2)
                        {
                            listBox1.Items.Add(link2.Attributes["content"].Value);
                        }
                    }
                }
            }

            else
            {
                MessageBox.Show("There is no book",
                    "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            book_id.Clear();

            if (string.IsNullOrWhiteSpace(search_box.Text))
            {
                MessageBox.Show("Search Box cannot be empty.",
                    "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
            {
                string adress = define_adress();
                htmlSearch(adress);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            if (listBox1.SelectedIndex > -1) 
            {
                string selected_book = listBox1.SelectedItem.ToString();
                selected_book = selected_book.Replace("'", "''");
                
                int selected_index = listBox1.SelectedIndex;
                
                string user_ID = frm1.user_id;
                string id = book_id.ElementAt(selected_index);

                string duplicate = "SELECT title, user_id FROM BookTable " +
                                 "WHERE title = '" + selected_book + "' and user_id = '"+ user_ID +"' ;";

                DataTable checkDuplicates = sqliteDatabase.execSqlite(duplicate);

                if (checkDuplicates.Rows.Count > 0)
                {
                    DialogResult result;
                    result = MessageBox.Show("The book already added.",
                        "Ops!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else 
                {
                    string mySQL = string.Empty;
                    mySQL += "INSERT INTO BookTable (book_id, user_id, title, page_num)";
                    mySQL += "VALUES ('" + id + "','" + user_ID + "','" + selected_book + "', '" + 0 + "') ;";

                    sqliteDatabase.execSqlite(mySQL);

                    MessageBox.Show("Book Added.",
                    "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frm_main.imageList1.Dispose();
                    frm_main.formMain_Load(this, e);
                }             
            }
        }

        private void search_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                pictureBox1_Click(sender, e);            
            }
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Beige;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Honeydew;
        }
    }
}
