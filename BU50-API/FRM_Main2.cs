using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FOCUSAPILib;

namespace BU50_API
{
    public partial class FRM_Main2 : Form
    {
        public FRM_Main2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sCompCode = Properties.Settings.Default.CompCode.ToString();
            var seqId = 0;

            var cd = new FOCUSAPILib.CompanyDetails();
            var fm = new FOCUSAPILib.FMaster();
            var cmp = new COMPLib.Init();
            var ft = new FOCUSAPILib.Transaction();
            var fr = new FReport();

            var frs = new FOCUSAPILib.FRateMaster();

            cmp.InitComp(sCompCode);
            cd.Open(0);

            double PQty = 0;
            double AvgVal = 0;
            double Val = 0;

            int warehouseid = 354157;
            int productid = 28940;
            var FRate = new FOCUSAPILib.FRateMaster();

            fr.GetStockOn(Convert.ToInt32(productid), DateTime.Now.Date, 5, ref PQty, ref AvgVal);
            fr.GetStockOn(productid, DateTime.Now.Date, 92, ref PQty, ref AvgVal);
            fr.OpenProductTransactions(DateTime.Now.Date, DateTime.Now.Date, productid);
            //var saleingprice = FRate.GetRate(0);
            FRate.Close();
            // MessageBox.Show(saleingprice.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var frm = new FRM_Updatewebsite();
            frm.Show();
        }
    }
}