using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite; 
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SoftwareEngineering.Connection
{
    class sqliteDatabase
    {

        static SQLiteConnection con;
        static SQLiteCommand cmd;

        public static void createDB() 
        {
            if (!File.Exists("BookshelfDB.sqlite")) 
            {
                SQLiteConnection.CreateFile("BookshelfDB.sqlite");

                string sql = @"CREATE TABLE Login_Table(
                                user_id         INTEGER     PRIMARY KEY AUTOINCREMENT,
                                First_Name      TEXT        NOT NULL,
                                Last_Name       TEXT        NOT NULL,
                                Email_Adress    TEXT        NOT NULL,
                                Username        TEXT        NOT NULL,
                                Password        TEXT        NOT NULL);";

                string sql2 = @"CREATE TABLE BookTable(
                                book_id         TEXT        NOT NULL,
                                user_id         INT         NOT NULL,
                                title           TEXT        NOT NULL,
                                page_num        INT         NOT NULL);";


                con = new SQLiteConnection("Data Source=BookshelfDB.sqlite;Version=3");
                con.Open();
                cmd = new SQLiteCommand(sql, con);
                cmd.ExecuteNonQuery();
                cmd = new SQLiteCommand(sql2, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            

        }

        public static DataTable execSqlite(string sqlite) 
        {
            SQLiteConnection connection = new SQLiteConnection();
            SQLiteDataAdapter adapter = default(SQLiteDataAdapter);
            DataTable dt = new DataTable();

            try
            {
                connection.ConnectionString = "Data Source=BookshelfDB.sqlite;Version=3";
                connection.Open();

                adapter = new SQLiteDataAdapter(sqlite, connection);
                adapter.Fill(dt);

                connection.Close();
                connection = null;

                return dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("An error occured: " + ex.Message,
                "SQL Server Connection Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

                dt = null;
            }

            return dt;
        }


    }
}
