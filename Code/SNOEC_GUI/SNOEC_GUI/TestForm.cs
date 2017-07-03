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
                dut = new QSFP28_SNOEC();
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
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxDeviceIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            QSFP28_SNOEC.DUT_USB_Port = this.comboBoxDeviceIndex.SelectedIndex;
        }

        private void comboBoxSoftHard_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBoxSoftHard.Text)
            {
                case "HARDWARE_SEQUENT":
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.HARDWARE_SEQUENT;
                    break;

                case "SOFTWARE_SEQUENT":
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.SOFTWARE_SEQUENT;
                    break;

                case "HARDWARE_SINGLE":
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.HARDWARE_SINGLE;
                    break;

                case "SOFTWARE_SINGLE":
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.SOFTWARE_SINGLE;
                    break;

                case "OnEasyB_I2C":
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.OnEasyB_I2C;
                    break;

                case "SerialPort":
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.SerialPort;
                    break;

                default:
                    QSFP28_SNOEC.softHard = IOPort.SoftHard.HARDWARE_SEQUENT;
                    break;
            }
        }

        private void comboBoxFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            IOPort.Frequency = (byte)(this.comboBoxFrequency.SelectedIndex + 1);
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
