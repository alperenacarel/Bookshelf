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
using System.Net;
using System.IO;
using System.Net.Mail;

namespace SoftwareEngineering
{
    public partial class formMain : Form
    {
        List<string> book_ids = new List<string>();
        formLogin form_log = null;
        List<string> books = new List<string>();
        
        public formMain(formLogin form_log)
        {
            this.form_log = form_log;
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            formSearch search = new formSearch(form_log, this);
            search.ShowDialog();
        }

        public void formMain_Load(object sender, EventArgs e)
        {
            sqlConnection();
            getCover();
        }

        public void sqlConnection()
        {
            books.Clear();
            book_ids.Clear();

            string user_id = form_log.user_id;
            string mySQL = string.Empty;


            mySQL += "SELECT title, book_id FROM BookTable ";
            mySQL += "WHERE user_id = '" + user_id + "' ;";

            DataTable bookData = sqliteDatabase.execSqlite(mySQL);

            foreach (DataRow row in bookData.Rows)
            {
                books.Add(row["title"].ToString());
                book_ids.Add(row["book_id"].ToString());
            }

        }

        public void getCover()
        {
            List<string> cover_address = new List<string>();
            listView1.Clear();
            cover_address.Clear();

            book_ids.ForEach(delegate (String addresses)
            {
                addresses = addresses.Replace("/ebooks/", "https://www.gutenberg.org/cache/epub/");
                addresses = addresses + "/pg" + addresses.Replace("https://www.gutenberg.org/cache/epub/", "") + ".cover.medium.jpg";
                cover_address.Add(addresses);
            });

            DownloadImagesFromWeb(cover_address, imageList1);
            imageList1.ImageSize = new Size(120, 150);
            int counter = 0;
            listView1.LargeImageList = imageList1;

            foreach (string s in books)
            {
                ListViewItem lst = new ListViewItem();
                lst.Text = s;
                lst.ImageIndex = counter++;
                listView1.Items.Add(lst);
            }

        }

        private void DownloadImagesFromWeb(List<string> adress, ImageList il)
        {
            foreach (string img in adress)
            {
                WebRequest request = WebRequest.Create(img);
                WebResponse resp = request.GetResponse();
                Stream respStream = resp.GetResponseStream();
                Bitmap bmp = new Bitmap(respStream);
                respStream.Dispose();

                il.Images.Add(bmp);
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void deleteBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Are you sure?",
            "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                for (int i = listView1.Items.Count - 1; i >= 0; i--)
                {
                    if (listView1.Items[i].Selected)
                    {
                        string test = listView1.Items[i].Text;
                        test = test.Replace("'", "''");
                        listView1.Items[i].Remove();

                        string del = "delete from BookTable where title='" + test + "' ;";

                        sqliteDatabase.execSqlite(del);
                    }
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string us_id = form_log.user_id;
            string test = "";

            foreach (int i in listView1.SelectedIndices)
            {
                
                test = listView1.Items[i].Text;
                test = test.Replace("'", "''");
            }

            string select = "select book_id, page_num from BookTable where title='" + test + "' and user_id = '" + us_id + "' ;";

            DataTable userData = sqliteDatabase.execSqlite(select);

            string bk_id = "";
            int pg_num = 0;

            foreach (DataRow row in userData.Rows)
            {
                bk_id = row["book_id"].ToString();
                pg_num = Convert.ToInt32(row["page_num"]);
            }

            bookForm book = new bookForm(us_id,bk_id, pg_num, test);
            book.ShowDialog();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.pictureBox1, "Search Book");

            pictureBox1.BackColor = Color.PaleGoldenrod;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.Khaki;
        }
    }
}
