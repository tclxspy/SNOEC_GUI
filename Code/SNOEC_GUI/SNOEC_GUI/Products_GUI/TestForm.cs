using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;

namespace SNOEC_GUI
{
    public partial class TestForm : Form
    {
        private QSFP28_SNOEC dut;
        private static int status = 0;

        public TestForm(QSFP28_SNOEC dt)
        {
            InitializeComponent();
            this.dut = dt;
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

        private delegate void UpdateControl(string label);

        private void toolStripBtnRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.radioButtonPowerCyclesTest.Checked)
                {
                    return;
                }

                this.Icon = Properties.Resources.OnLineBusy;
                this.toolStripBtnRun.Enabled = false;
                decimal cycles = this.numericUpDownCycles.Value;
                status = 0;

                string sn = dut.ReadPn();
                int count = 0;
                int delay = (int)this.numericUpDownDelay.Value;
                Dictionary<string, int> dic = new Dictionary<string, int>();
                dic.Add(sn, count);

                Task task = Task.Factory.StartNew(() =>
                {        
                    for (int i = 1; i < cycles; i++)
                    {
                        Thread.Sleep(delay);
                        if (status == -1)
                        {                            
                            return;
                        }

                        sn = dut.ReadPn();
                        if (!dic.Keys.Contains(sn))
                        {
                            dic.Add(sn, ++count);
                        }

                        string label = count + " / " + (i + 1);
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new UpdateControl(delegate
                            {
                                this.labelResult.Text = label;
                            }), label);
                        }
                        else
                        {
                            this.labelResult.Text = label;
                        }
                    }
                });

                Task cwt = task.ContinueWith(t =>
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(delegate
                        {
                            string message = "Serial number-->";
                            foreach (string key in dic.Keys)
                            {
                                message += key + " ";
                            }
                            this.Icon = Properties.Resources.Online;
                            this.toolStripBtnRun.Enabled = true;
                            MessageBox.Show(message, "how many serial number", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }));
                    }
                    else
                    {
                        string message = "Serial number-->";
                        foreach (string key in dic.Keys)
                        {
                            message += key + " ";
                        }
                        this.Icon = Properties.Resources.Online;
                        this.toolStripBtnRun.Enabled = true;
                        MessageBox.Show(message, "how many serial number", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                });
            }
            catch(Exception ex)
            {
                this.Icon = Properties.Resources.Online;
                this.toolStripBtnRun.Enabled = true;
                MessageBox.Show("No link. please click read buton fistly on tha main GUI", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        

        private void toolStripBtnStop_Click(object sender, EventArgs e)
        {
            status = -1;
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            status = -1;
        }
    }
}
