using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BU50_API
{
    public partial class FRM_Config : Form
    {
        public SqlConnection con;
        public SqlConnection cons;
        private string Servername = Properties.Settings.Default.ServerName;
        private string ServerNameExternal = Properties.Settings.Default.ServerNameExternal;
        public string DataBase = Properties.Settings.Default.DataBase;
        public string DataBase2 = Properties.Settings.Default.DB;
        private string id = Properties.Settings.Default.UserId;
        private string pass = Properties.Settings.Default.Password;
        private string password2 = Properties.Settings.Default.password2;
        public static string Constr = string.Empty;

        public FRM_Config()
        {
            InitializeComponent();
            txtapi.Text = Properties.Settings.Default.ApiKey;
        }

        private void save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CompCode = txtcomcode.Text;

            Properties.Settings.Default.password2 = txtpass2.Text;
            Properties.Settings.Default.ServerNameExternal = txtServerNameExternal.Text;
            Properties.Settings.Default.DB = txtdb.Text;
            Properties.Settings.Default.ApiKey = txtapi.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Saved Done");
        }


        private void btnsave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ServerName = txt_Server.Text;
            Properties.Settings.Default.DataBase = txt_DataBase.Text;
            Properties.Settings.Default.Password = txt_password.Text;
            Properties.Settings.Default.UserId = txt_user.Text;
            Properties.Settings.Default.Save();
            MessageBox.Show("Saved Don ...");

            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(@"Server=" + Servername + "; Database=" + DataBase +
                                    "; Integrated Security=false; User ID=" + id + " ; Password=" + pass + "");

            string connetionString = null;
            SqlConnection cnn;
            connetionString = con.ConnectionString;
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Connection Open ! ");

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }

        private void check_Click(object sender, EventArgs e)
        {
            cons = new SqlConnection(@"Server=" + ServerNameExternal + "; Database=" + DataBase2 +
                                     "; Integrated Security=false; User ID=" + id + " ; Password=" + password2 + "");

            string connetionString = null;
            SqlConnection cnn;
            connetionString = cons.ConnectionString;
            cnn = new SqlConnection(connetionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Connection Open ! ");

                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }
    }
}