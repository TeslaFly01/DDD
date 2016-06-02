using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDD.BusinessServiceEngine.Common
{
    public class FormHelper
    {
        /// <summary> 
        /// 控件随窗体自动缩放 
        /// </summary> 
        /// <param name="frm"></param> 
        public static void AutoScale(Form frm)
        {
            frm.Tag = frm.Width.ToString() + "," + frm.Height.ToString();
            frm.SizeChanged += new EventHandler(frm_SizeChanged);
            frm.MaximumSizeChanged += new EventHandler(frm_SizeChanged);
        }

        static void frm_SizeChanged(object sender, EventArgs e)
        {
            string[] tmp = ((Form)sender).Tag.ToString().Split(',');
            float width = (float)((Form)sender).Width / (float)Convert.ToInt16(tmp[0]);
            float heigth = (float)((Form)sender).Height / (float)Convert.ToInt16(tmp[1]);

            ((Form)sender).Tag = ((Form)sender).Width.ToString() + "," + ((Form)sender).Height;

            foreach (Control control in ((Form)sender).Controls)
            {
                // if (control.GetType().Name.ToLower() != "button")
                control.Scale(new SizeF(width, heigth));

            }
        }
    }
}
