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

namespace BU50_API.PL
{
    public partial class FRM_Schema : Form
    {
        private DataTable schema = new DataTable();
        private DataTable Searchschema = new DataTable();
        private DataTable _schemaType = new DataTable();
        private FRM_Updatewebsite fr = new FRM_Updatewebsite();

        public FRM_Schema()
        {
            InitializeComponent();
            getschematype();
            GetSchema();
        }

        private void GetSchema()
        {
            using (var cmdschema = new SqlCommand("", fr.con))
            {
                cmdschema.CommandText = $"SELECT * FROM SchemaByMHDI ORDER BY Id DESC";
                fr.con.Close();
                fr.con.Open();

                schema.Load(cmdschema.ExecuteReader());
                dataGridView1.DataSource = schema;
                fr.con.Close();
            }
        }

        public void getschematype()
        {
            using (var cmdschemaType = new SqlCommand("", fr.con))
            {
                cmdschemaType.CommandText = $"SELECT Id,SchemeName FROM  dbo.SchemeHeader ORDER BY Id DESC";
                fr.con.Close();
                fr.con.Open();

                _schemaType.Load(cmdschemaType.ExecuteReader());
                cmdschematype.DataSource = _schemaType;
                cmdschematype.DisplayMember = "SchemeName";
                cmdschematype.ValueMember = "Id";
                fr.con.Close();
            }
        }

        public void Search(string Code)
        {
            try
            {
                using (var cmdsearch = new SqlCommand("", fr.con))
                {
                    Searchschema.Clear();
                    cmdsearch.CommandText = $"SELECT * FROM SchemaByMHDI WHERE Id ='{Code}'";
                    //fr.con.Close();
                    fr.con.Open();

                    Searchschema.Load(cmdsearch.ExecuteReader());
                    dataGridView1.DataSource = Searchschema;
                    fr.con.Close();
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        private void cmdschematype_SelectedValueChanged(object sender, EventArgs e)
        {
            Search(cmdschematype.SelectedValue.ToString());
        }

        private void btnget_Click(object sender, EventArgs e)
        {
            //Search(cmdschematype.SelectedValue.ToString());
        }
    }
}