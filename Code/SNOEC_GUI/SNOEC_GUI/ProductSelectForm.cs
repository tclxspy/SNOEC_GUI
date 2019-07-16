using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SNOEC_GUI
{
    public partial class ProductSelectForm : Form
    {
        public ProductSelectForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.WParam.ToInt32())
            {
                //click restore button
                //case 0xF120:
                //    m.WParam = (IntPtr)0xF030;
                //    break;

                //click title panel
                case 0xF122:
                    m.WParam = IntPtr.Zero;
                    break;
            }

            base.WndProc(ref m);
        }

        private void btnSelect_QSFP28_Click(object sender, EventArgs e)
        {
            Form form = new MainForm();
            this.Visible = false;
            form.Show();
        }

        private void btnSelect_QSFP_DD_Click(object sender, EventArgs e)
        {
            Form form = new CMIS_Form();
            this.Visible = false;
            form.Show();
        }

        private void btnSelect_QSFP28_PSM4_Click(object sender, EventArgs e)
        {
            Form form = new QSFP28_PSM4_Form();
            this.Visible = false;
            form.Show();
        }

        private void btnSelect_Edemux_Click(object sender, EventArgs e)
        {
            Form form = new Edemux_Form();
            this.Visible = false;
            form.Show();
        }

        private void btnSelect_100G_DR1_Click(object sender, EventArgs e)
        {
            Form form = new _100G_DR1_From();
            this.Visible = false;
            form.Show();
        }
    }
}
