using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SNOEC_GUI
{
    public partial class MainForm : Form
    {
        private QSFP28_SNOEC dut;
        private TextBox[] txts_dmi_TxPower = new TextBox[4];
        private TextBox[] txts_dmi_TxBias = new TextBox[4];
        private TextBox[] txts_dmi_RxPower = new TextBox[4];

        public MainForm()
        {
            InitializeComponent();

            this.tabControl1.SelectedIndex = 1;

            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView3.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView3.AllowUserToAddRows = false;
            this.dataGridView4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.AllowUserToAddRows = false;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = this.dataGridView1.Width / this.dataGridView1.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView2.Columns.Count; i++)
            {
                this.dataGridView2.Columns[i].Width = this.dataGridView2.Width / this.dataGridView2.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView3.Columns.Count; i++)
            {
                this.dataGridView3.Columns[i].Width = this.dataGridView3.Width / this.dataGridView3.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView4.Columns.Count; i++)
            {
                this.dataGridView4.Columns[i].Width = this.dataGridView4.Width / this.dataGridView4.Columns.Count;
            }
            this.dataGridView1.Rows.Add(6);
            this.dataGridView2.Rows.Add(6);
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView3.Rows.Add(6);
            this.dataGridView4.Rows.Add(6);
            this.dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            txts_dmi_TxPower[0] = txtDMI_TxPower_Ch1;
            txts_dmi_TxPower[1] = txtDMI_TxPower_Ch2;
            txts_dmi_TxPower[2] = txtDMI_TxPower_Ch3;
            txts_dmi_TxPower[3] = txtDMI_TxPower_Ch4;
            
            txts_dmi_TxBias[0] = txtDMI_TxBias_Ch1;
            txts_dmi_TxBias[1] = txtDMI_TxBias_Ch2;
            txts_dmi_TxBias[2] = txtDMI_TxBias_Ch3;
            txts_dmi_TxBias[3] = txtDMI_TxBias_Ch4;
            
            txts_dmi_RxPower[0] = txtDMI_RxPower_Ch1;
            txts_dmi_RxPower[1] = txtDMI_RxPower_Ch2;
            txts_dmi_RxPower[2] = txtDMI_RxPower_Ch3;
            txts_dmi_RxPower[3] = txtDMI_RxPower_Ch4;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.tabControl1.SelectedTab.ToString().Contains("I2C Write"))
            {
                this.btnReadWrite.Text = "Write";
            }
            else
            {
                this.btnReadWrite.Text = "Read";
            }
        }

        private void btnReadWrite_Click(object sender, EventArgs e)
        {
            IOPort.Frequency = (byte)(this.comboBoxFrequency.SelectedIndex + 1);

            string path = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            FilePath.LogFile = Application.StartupPath + @"\Log\" + path;
            dut = new QSFP28_SNOEC(this.comboBoxDeviceIndex.SelectedIndex);
            string partNumber = dut.ReadPn();
            string serialNumber = dut.ReadSN();

            if(this.tabControl1.SelectedTab.ToString().Contains("DMI/ADC"))
            {
                this.Read_DMI_ADC();
            }
        }

        private void Read_DMI_ADC()
        {
            this.txtDMI_Temp.Text = dut.ReadDmiTemp().ToString();
            this.txtDMI_VCC.Text = dut.ReadDmiVcc().ToString();
            for (int i = 0; i < txts_dmi_TxBias.Length; i++)
            {
                txts_dmi_TxBias[i].Text = dut.ReadDmiBias(i + 1).ToString();
                txts_dmi_TxPower[i].Text = dut.ReadDmiTxP(i + 1).ToString();
                txts_dmi_RxPower[i].Text = dut.ReadDmiRxP(i + 1).ToString();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                byte i, re;
                OnEasyB_I2C.serialNumber = new StringBuilder(255);
                byte MaxDevNum = OnEasyB_I2C.USBIO_GetMaxNumofDev();
                List<String> list = new List<string>();

                for (i = 0; i < MaxDevNum; i++)
                {
                    re = OnEasyB_I2C.USBIO_GetSerialNo(i, OnEasyB_I2C.serialNumber);
                    if (re != 0)
                    {
                        list.Add(OnEasyB_I2C.serialNumber.ToString());
                    }
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("disconnect to I2C", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("disconnect to I2C", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
    
}
