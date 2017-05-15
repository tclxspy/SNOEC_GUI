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
    public partial class TestForm : Form
    {
        public TestForm()
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

        private void radioButtonI2C_Com_Test_CheckedChanged(object sender, EventArgs e)
        {
            if(this.radioButtonI2C_Com_Test.Checked)
            {
                this.numericUpDownCycles.Value = 1000000;
                this.txtSavePath.Enabled = false;
                this.btnBrowse.Enabled = false;
            }
            else
            {
                this.numericUpDownCycles.Value = 0;
                this.txtSavePath.Enabled = true;
                this.btnBrowse.Enabled = true;
            }
        }
    }
}
