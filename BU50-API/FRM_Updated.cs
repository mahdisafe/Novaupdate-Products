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
    public partial class FRM_Updated : Form
    {
        private string Servername = Properties.Settings.Default.ServerName;
        public string DataBase = Properties.Settings.Default.DataBase;
        public string db = Properties.Settings.Default.DB;
        private string id = Properties.Settings.Default.UserId;
        private string pass = Properties.Settings.Default.Password;
        public SqlConnection con;
        public SqlConnection cons;
        private DataTable dtPRO = new DataTable();

        public FRM_Updated()
        {
            InitializeComponent();
            cons = new SqlConnection(@"Server=" + Servername + "; Database=" + db + "; Integrated Security=false; User ID=" + id + " ; Password=" + pass + "");
            con = new SqlConnection(@"Server=" + Servername + "; Database=" + DataBase + "; Integrated Security=false; User ID=" + id + " ; Password=" + pass + "");

            using (var cmdPRODUCTS = new SqlCommand("", cons))
            {
                cons.Open();
                cmdPRODUCTS.CommandText = $"SELECT TOP(6000)UpdatedProducts.* FROM dbo.UpdatedProducts ORDER BY UpdatedProducts.isdate DESC";
                //dtPRO.Rows.Clear();
                dtPRO.Load(cmdPRODUCTS.ExecuteReader());
                grdproduct.DataSource = dtPRO;
                cons.Close();
            }
        }

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            var sear = new SqlConnection(@"Server=" + Servername + "; Database=" + db + "; Integrated Security=false; User ID=" + id + " ; Password=" + pass + "");
            using (var cmdPROD = new SqlCommand($"Select * from Products  WHERE name Like '%{txtsearch.Text}%'", sear))
            {
                sear.Open();

                dt.Load(cmdPROD.ExecuteReader());
                grdproduct.DataSource = dt;
                sear.Close();
            }
        }
    }
}