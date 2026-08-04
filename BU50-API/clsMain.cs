using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BU50_API
{
    public class ClsMain
    {
        public bool F_Int(long A, long B, long C)
        {
            using (var frm = new BU50_API.FRM_Updatewebsite())
            {
                frm.ShowDialog();
            }
            return true;
        }
    }
}