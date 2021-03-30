using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Windows.Forms;
using SoftwareEngineering.Connection;

namespace SoftwareEngineering
{
    public partial class bookForm : Form
    {
        string user_id = null;
        int page_number = 0;
        string book_id = null;
        string title = null;
        System.Windows.Forms.HtmlDocument htmlDoc = null;
        
        public bookForm(string user_id, string book_id, int page_number, string title)
        {
            this.user_id = user_id;
            this.book_id = book_id;
            this.page_number = page_number;
            this.title = title;
            this.htmlDoc = htmlDoc;

            InitializeComponent(); 
        }

        public void bookForm_Load(object sender, EventArgs e)
        {
            string url = createURL();
            this.Text = title;
            webBrowser1.Navigate(new Uri(url));
        }

        private string createURL() 
        {
            string num_id = book_id.Replace("/ebooks/", "");
            string url = book_id.Replace("/ebooks/", "https://www.gutenberg.org/files/");
            url = url + "/" + num_id + "-h" + "/" + num_id + "-h.htm";

            return url;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if(createURL() == webBrowser1.Url.AbsoluteUri)
            {
                setScrollBar(page_number);
                webBrowser1.DocumentCompleted -= webBrowser1_DocumentCompleted;
            }
            
            
        }

        public void setScrollBar(int page)
        {
            System.Windows.Forms.HtmlDocument htmlDoc = webBrowser1.Document;

            if (page > 0)
            {
                htmlDoc.GetElementsByTagName("HTML")[0].ScrollTop = Convert.ToInt32(page);
            }
        }

        public int takePageNumber() 
        {
            string select = "select page_num from BookTable where title='" + title + "' and user_id = '" + user_id + "' ;";

            DataTable userData = sqliteDatabase.execSqlite(select);
            
            int pg_num = 0;

            foreach (DataRow row in userData.Rows)            
                pg_num = Convert.ToInt32(row["page_num"]);

            return pg_num;

        }

        public void setPageNumber() 
        {
            if (webBrowser1.Url.AbsoluteUri == createURL())
            {
                System.Windows.Forms.HtmlDocument htmlDoc = webBrowser1.Document;

                int scrollTop = htmlDoc.GetElementsByTagName("HTML")[0].ScrollTop;

                string mySQL = string.Empty;
                mySQL += "UPDATE BookTable SET page_num = ('" + scrollTop + "') ";
                mySQL += "WHERE user_id = ('" + user_id + "') and book_id = ('" + book_id + "') ;";

                sqliteDatabase.execSqlite(mySQL);
            }
        }

        private void bookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            setPageNumber();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (webBrowser1.Url.AbsoluteUri != createURL())
                button1.Visible = true;
            else if(webBrowser1.Url.AbsoluteUri == createURL())
            {
                button1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }
    }
}
