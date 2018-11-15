using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;
using System.Threading;

namespace SNOEC_GUI
{
    public partial class CMIS_Form : Form
    {
        private QSFP_DD_SNOEC dut;

        private TextBox[] txts_dmi_TxPower = new TextBox[4];
        private TextBox[] txts_dmi_TxBias = new TextBox[4];
        private TextBox[] txts_dmi_RxPower = new TextBox[4];
        private TextBox[] txts_adc_TxPower = new TextBox[4];
        private TextBox[] txts_adc_TxBias = new TextBox[4];
        private TextBox[] txts_adc_RxPower = new TextBox[4];
        private Chart[,] chart = new Chart[5, 8];
        private const int maxCells = 16 * 8;

        public CMIS_Form()
        {
            InitializeComponent();
            this.labelDate.Text = DateTime.Now.ToShortDateString() + "   " + DateTime.Now.ToShortTimeString();
            this.comboBoxDeviceIndex.SelectedIndex = 1;
            this.comboBoxSoftHard.SelectedIndex = 0;
            this.comboBoxFrequency.SelectedIndex = 3;


            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView6.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView6.AllowUserToAddRows = false;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = this.dataGridView1.Width / this.dataGridView1.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView4.Columns.Count; i++)
            {
                this.dataGridView4.Columns[i].Width = this.dataGridView4.Width / this.dataGridView4.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView6.Columns.Count; i++)
            {
                this.dataGridView6.Columns[i].Width = this.dataGridView6.Width / this.dataGridView6.Columns.Count;
            }
            this.dataGridView1.Rows.Add(8);
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView4.Rows.Add(8);
            this.dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView6.Rows.Add(8);
            this.dataGridView6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

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

            txts_adc_TxPower[0] = txtTxPowerADC_Ch1;
            txts_adc_TxPower[1] = txtTxPowerADC_Ch2;
            txts_adc_TxPower[2] = txtTxPowerADC_Ch3;
            txts_adc_TxPower[3] = txtTxPowerADC_Ch4;

            txts_adc_TxBias[0] = txtTxBiasADC_Ch1;
            txts_adc_TxBias[1] = txtTxBiasADC_Ch2;
            txts_adc_TxBias[2] = txtTxBiasADC_Ch3;
            txts_adc_TxBias[3] = txtTxBiasADC_Ch4;

            txts_adc_RxPower[0] = txtRxPowerADC_Ch1;
            txts_adc_RxPower[1] = txtRxPowerADC_Ch2;
            txts_adc_RxPower[2] = txtRxPowerADC_Ch3;
            txts_adc_RxPower[3] = txtRxPowerADC_Ch4;

            //initial chart[,]
            chart[0, 0] = this.chart1;
            chart[0, 1] = this.chart2;
            chart[0, 2] = this.chart3;
            chart[0, 3] = this.chart4;
            chart[0, 4] = this.chart5;
            chart[0, 5] = this.chart6;
            chart[0, 6] = this.chart7;
            chart[0, 7] = this.chart8;

            chart[1, 0] = this.chart9;
            chart[1, 1] = this.chart10;
            chart[1, 2] = this.chart11;
            chart[1, 3] = this.chart12;
            chart[1, 4] = this.chart13;
            chart[1, 5] = this.chart14;
            chart[1, 6] = this.chart15;
            chart[1, 7] = this.chart16;

            chart[2, 0] = this.chart17;
            chart[2, 1] = this.chart18;
            chart[2, 2] = this.chart19;
            chart[2, 3] = this.chart20;
            chart[2, 4] = this.chart21;
            chart[2, 5] = this.chart22;
            chart[2, 6] = this.chart23;
            chart[2, 7] = this.chart24;

            chart[3, 0] = this.chart25;
            chart[3, 1] = this.chart26;
            chart[3, 2] = this.chart27;
            chart[3, 3] = this.chart28;
            chart[3, 4] = this.chart29;
            chart[3, 5] = this.chart30;
            chart[3, 6] = this.chart31;
            chart[3, 7] = this.chart32;

            chart[4, 0] = this.chart33;
            chart[4, 1] = this.chart34;
            chart[4, 2] = this.chart35;
            chart[4, 3] = this.chart36;
            chart[4, 4] = this.chart37;
            chart[4, 5] = this.chart38;
            chart[4, 6] = this.chart39;
            chart[4, 7] = this.chart40;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnReadWrite.Enabled = true;

            if (this.tabControl1.SelectedTab.ToString().Contains("I2C Write"))
            {
                this.btnReadWrite.Text = "Write";
            }
            else
            {
                this.btnReadWrite.Text = "Read";
            }

            if (this.tabControl1.SelectedTab.ToString().Contains("Driver"))
            {
                dut = new QSFP_DD_SNOEC();
                this.btnReadWrite.Enabled = false;
            }

            if (this.tabControl1.SelectedTab.ToString().Contains("MacomChip"))
            {
                this.btnReadWrite.Text = "Execute";
                this.comboBoxMacomChip_Operation.SelectedIndex = 0;
                this.comboBoxMacomChip_Select.SelectedIndex = 0;
            }
        }

        private void ClearTextBox()
        {
            this.txtDMI_Temp.Text = "NaN";
            this.txtDMI_VCC.Text = "NaN";
            this.txtTempADC.Text = "NaN";
            this.txtVccADC.Text = "NaN";
            this.txtFW_Version.Text = "NaN";
            for (int i = 0; i < txts_dmi_TxBias.Length; i++)
            {
                txts_dmi_TxBias[i].Text = "NaN";
                txts_dmi_TxPower[i].Text = "NaN";
                txts_dmi_RxPower[i].Text = "NaN";

                txts_adc_TxBias[i].Text = "NaN";
                txts_adc_TxPower[i].Text = "NaN";
                txts_adc_RxPower[i].Text = "NaN";
            }
        }

        private int deviceAddress = 0xA0;
        private void btnReadWrite_Click(object sender, EventArgs e)
        {
            this.btnReadWrite.Enabled = false;
            this.btnReadWrite.BackColor = Color.Yellow;
            this.btnReadWrite.Refresh();
            deviceAddress = 0xA0;
            switch (this.domainUpDownDeviceAddress.Text)
            {
                case "0xA0":
                    deviceAddress = 0xA0;
                    break;

                case "0xA2":
                    deviceAddress = 0xA2;
                    break;

                case "0xA8":
                    deviceAddress = 0xA8;
                    break;

                default:
                    deviceAddress = 0xA0;
                    break;
            }

            try
            {

                if (this.tabControl1.SelectedTab.ToString().Contains("Ch On/Off"))
                {
                    //this.ChOnOff();
                }

                if (this.tabControl1.SelectedTab.ToString().Contains("DMI/ADC"))
                {
                    //this.Tab_DMI_ADC();
                }

                if (this.tabControl1.SelectedTab.ToString().Contains("Alarm/Warning"))
                {
                    //for (int i = 0; i < 4; i++)
                    //{
                    //    chart[0, i].BackColor = this.GetColor(dut.GetInteTxPowerAlarm(i + 1));
                    //    chart[0, i + 4].BackColor = this.GetColor(dut.GetInteTxPowerWarning(i + 1));
                    //    chart[1, i].BackColor = this.GetColor(dut.GetInteTxBiasAlarm(i + 1));
                    //    chart[1, i + 4].BackColor = this.GetColor(dut.GetInteTxBiasWarning(i + 1));
                    //    chart[2, i].BackColor = this.GetColor(dut.GetInteRxPowerAlarm(i + 1));
                    //    chart[2, i + 4].BackColor = this.GetColor(dut.GetInteRxPowerWarning(i + 1));
                    //}
                    //this.chart25.BackColor = this.chart26.BackColor = this.chart27.BackColor = this.chart28.BackColor = this.GetColor(dut.GetInteVccAlarm());
                    //this.chart29.BackColor = this.chart30.BackColor = this.chart31.BackColor = this.chart32.BackColor = this.GetColor(dut.GetInteVccWarning());
                    //this.chart33.BackColor = this.chart34.BackColor = this.chart35.BackColor = this.chart36.BackColor = this.GetColor(dut.GetInteTempAlarm());
                    //this.chart37.BackColor = this.chart38.BackColor = this.chart39.BackColor = this.chart40.BackColor = this.GetColor(dut.GetInteTempWarning());
                }



                if (this.tabControl1.SelectedTab.ToString().Contains("I2C Read"))
                {
                    this.Tab_I2C_Read();
                }

                if (this.tabControl1.SelectedTab.ToString().Contains("I2C Write"))
                {
                    this.Tab_I2C_Write();
                }

                if (this.tabControl1.SelectedTab.ToString().Contains("MacomChip"))
                {
                    this.Tab_MacomChip();
                }
            }

            catch
            {
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.btnReadWrite.Enabled = true;
            this.btnReadWrite.BackColor = SystemColors.Control;
        }

        //private void Tab_DMI_ADC()
        //{
        //    ClearTextBox();

        //    this.txtDMI_Temp.Text = dut.ReadDmiTemp().ToString();
        //    this.txtDMI_VCC.Text = dut.ReadDmiVcc().ToString();
        //    if (QSFP28_SNOEC.company == QSFP28_SNOEC.Company.SNOEC)
        //    {
        //        this.txtTempADC.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.TemperatureAdc, 0).ToString();
        //        this.txtVccADC.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.VccAdc, 0).ToString();
        //    }
        //    this.txtFW_Version.Text = dut.ReadFWRev();
        //    for (int i = 0; i < txts_dmi_TxBias.Length; i++)
        //    {
        //        txts_dmi_TxBias[i].Text = dut.ReadDmiBias(i + 1).ToString();
        //        txts_dmi_TxPower[i].Text = dut.ReadDmiTxP(i + 1).ToString();
        //        txts_dmi_RxPower[i].Text = dut.ReadDmiRxP(i + 1).ToString();

        //        if (QSFP28_SNOEC.company == QSFP28_SNOEC.Company.SNOEC)
        //        {
        //            txts_adc_TxBias[i].Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.TxBiasAdc, (i + 1)).ToString();
        //            txts_adc_TxPower[i].Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.TxPowerAdc, (i + 1)).ToString();
        //            txts_adc_RxPower[i].Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.RxPowerAdc, (i + 1)).ToString();
        //        }
        //    }
        //}

        private void Tab_I2C_Read()
        {
            //clear cells
            for (int i = 0; i < maxCells; i++)
            {
                this.dataGridView1.Rows[i / 16].Cells[i % 16].Value = null;
            }
            this.dataGridView1.Refresh();

            byte[] buff = new byte[(int)this.numericUpDownBytes.Value];
            if ((int)this.numericUpDownBytes.Value > 0)
            {

                buff = dut.ReadReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, (int)this.numericUpDownPage.Value, (int)this.numericUpDownRegAddress.Value, (int)this.numericUpDownBytes.Value);
                if (buff == null)
                {
                    this.btnReadWrite.Enabled = true;
                    this.btnReadWrite.BackColor = SystemColors.Control;
                    return;
                }
            }

            int length = Math.Min(maxCells, buff.Length);

            for (int i = 0; i < length; i++)
            {
                this.dataGridView1.Rows[i / 16].Cells[i % 16].Value = Convert.ToString(buff[i], 16).ToUpper();
            }
        }

        private void Tab_I2C_Write()
        {
            byte[] writeData = new byte[(int)this.numericUpDownBytes.Value];
            if (writeData.Length == 0)
            {
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                return;
            }

            try
            {
                for (int i = 0; i < writeData.Length; i++)
                {
                    object ob = this.dataGridView4.Rows[i / 16].Cells[i % 16].Value;
                    if (ob == null)
                    {
                        this.btnReadWrite.Enabled = true;
                        this.btnReadWrite.BackColor = SystemColors.Control;
                        return;
                    }
                    writeData[i] = byte.Parse((string)ob, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch
            {
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                return;
            }

            byte[] buff = dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, (int)this.numericUpDownPage.Value, (int)this.numericUpDownRegAddress.Value, writeData);
            if (buff == null)
            {
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                return;
            }
        }

        private void Tab_MacomChip()
        {
            int maxCells_dataGridView6 = 16 * 8;
            byte map_page_chip_control = 0xC0;
            byte map_address_chip_control_i2c_address = 0;
            byte map_address_chip_control_page_select = 0;
            byte map_address_chip_control_regAddress = 0;
            byte map_address_chip_control_reg_read_trigger = 0;
            byte map_address_chip_control_reg_read_data = 0;
            byte map_address_chip_control_reg_write_data = 0;
            byte map_address_chip_control_reg_write_trigger = 0;

            byte value_map_address_chip_control_i2c_address = 0;
            byte value_map_address_chip_control_page_select = 0;
            byte value_map_address_chip_control_regAddress = 0;
            byte value_map_address_chip_control_reg_read_trigger = 0;
            byte value_map_address_chip_control_reg_read_data = 0;
            byte value_map_address_chip_control_reg_write_data = 0;
            byte value_map_address_chip_control_reg_write_trigger = 0;
            if (this.radioButtonMacomChip_Block1.Checked)
            {
                map_address_chip_control_i2c_address = 0x80;
                map_address_chip_control_page_select = 0x81;
                map_address_chip_control_regAddress = 0x82;
                map_address_chip_control_reg_read_trigger = 0x83;
                map_address_chip_control_reg_read_data = 0x84;
                map_address_chip_control_reg_write_data = 0x85;
                map_address_chip_control_reg_write_trigger = 0x86;
            }
            else if (this.radioButtonMacomChip_Block2.Checked)
            {
                map_address_chip_control_i2c_address = 0x87;
                map_address_chip_control_page_select = 0x88;
                map_address_chip_control_regAddress = 0x89;
                map_address_chip_control_reg_read_trigger = 0x8A;
                map_address_chip_control_reg_read_data = 0x8B;
                map_address_chip_control_reg_write_data = 0x8C;
                map_address_chip_control_reg_write_trigger = 0x8D;
            }
            else
            {
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                MessageBox.Show("Please select block.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.comboBoxMacomChip_Select.SelectedIndex == 0)
            {
                value_map_address_chip_control_i2c_address = 0x18;
            }
            if (this.comboBoxMacomChip_Select.SelectedIndex == 1)
            {
                value_map_address_chip_control_i2c_address = 0x1C;
            }
            if (this.comboBoxMacomChip_Select.SelectedIndex == 2)
            {
                value_map_address_chip_control_i2c_address = 0x16;
            }
            if (this.comboBoxMacomChip_Select.SelectedIndex == 3)
            {
                value_map_address_chip_control_i2c_address = 0x20;
            }

            value_map_address_chip_control_page_select = (byte)this.numericUpDownMacomChip_Page.Value;
            value_map_address_chip_control_regAddress = (byte)this.numericUpDownMacomChip_Address.Value;
            byte[] buffer = new byte[(int)this.numericUpDownMacomChip_Bytes.Value];

            if (this.comboBoxMacomChip_Operation.SelectedIndex == 0)//read
            {
                //clear cells
                for (int i = 0; i < maxCells_dataGridView6; i++)
                {
                    this.dataGridView6.Rows[i / 16].Cells[i % 16].Value = null;
                }
                this.dataGridView6.Refresh();

                if ((int)this.numericUpDownMacomChip_Bytes.Value > 0)
                {
                    value_map_address_chip_control_reg_read_trigger = 1;
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_i2c_address, new byte[] { value_map_address_chip_control_i2c_address });
                        dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_page_select, new byte[] { value_map_address_chip_control_page_select });
                        dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_regAddress, new byte[] { value_map_address_chip_control_regAddress });
                        dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_read_trigger, new byte[] { value_map_address_chip_control_reg_read_trigger });
                        value_map_address_chip_control_reg_read_data = dut.ReadReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_read_data, 1)[0];
                        buffer[i] = value_map_address_chip_control_reg_read_data;
                        value_map_address_chip_control_regAddress++;
                    }

                    if (buffer == null)
                    {
                        this.btnReadWrite.Enabled = true;
                        this.btnReadWrite.BackColor = SystemColors.Control;
                        return;
                    }
                }

                int length = Math.Min(maxCells, buffer.Length);

                for (int i = 0; i < length; i++)
                {
                    this.dataGridView6.Rows[i / 16].Cells[i % 16].Value = Convert.ToString(buffer[i], 16).ToUpper();
                }
            }

            if (this.comboBoxMacomChip_Operation.SelectedIndex == 1)//write
            {
                if (buffer.Length == 0)
                {
                    this.btnReadWrite.Enabled = true;
                    this.btnReadWrite.BackColor = SystemColors.Control;
                    return;
                }

                try
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        object ob = this.dataGridView6.Rows[i / 16].Cells[i % 16].Value;
                        if (ob == null)
                        {
                            this.btnReadWrite.Enabled = true;
                            this.btnReadWrite.BackColor = SystemColors.Control;
                            return;
                        }
                        buffer[i] = byte.Parse((string)ob, System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch
                {
                    MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnReadWrite.Enabled = true;
                    this.btnReadWrite.BackColor = SystemColors.Control;
                    return;
                }
                value_map_address_chip_control_reg_write_trigger = 1;
                for (int i = 0; i < buffer.Length; i++)
                {
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_i2c_address, new byte[] { value_map_address_chip_control_i2c_address });
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_page_select, new byte[] { value_map_address_chip_control_page_select });
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_regAddress, new byte[] { value_map_address_chip_control_regAddress });
                    value_map_address_chip_control_reg_write_data = buffer[i];
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_write_data, new byte[] { value_map_address_chip_control_reg_write_data });
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_write_trigger, new byte[] { value_map_address_chip_control_reg_write_trigger });
                    value_map_address_chip_control_regAddress++;
                }
            }
        }

        //normal:0, low:1, high:2
        private Color GetColor(int value)
        {
            switch (value)
            {
                case 0:
                    return Color.Lime;

                case 1:
                    return Color.Gray;

                case 2:
                    return Color.Red;

                default:
                    return Color.Lime;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.labelTitle.Text = "400G QSFP-DD GUI";

                dut = new QSFP_DD_SNOEC();
                this.tabControl1.SelectedIndex = 5;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        //private void btnTxCh1_4_Dis_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btnTxCh1_4_Dis.BackColor == Color.Lime)
        //        {
        //            if (dut.SetSoftTxDis() == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh1_4_Dis.BackColor = Color.Gray;
        //            btnTxCh1_Dis.BackColor = Color.Gray;
        //            btnTxCh2_Dis.BackColor = Color.Gray;
        //            btnTxCh3_Dis.BackColor = Color.Gray;
        //            btnTxCh4_Dis.BackColor = Color.Gray;
        //        }
        //        else
        //        {
        //            if (dut.TxAllChannelEnable() == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh1_4_Dis.BackColor = Color.Lime;
        //            btnTxCh1_Dis.BackColor = Color.Lime;
        //            btnTxCh2_Dis.BackColor = Color.Lime;
        //            btnTxCh3_Dis.BackColor = Color.Lime;
        //            btnTxCh4_Dis.BackColor = Color.Lime;
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnTxCh1_Dis_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btnTxCh1_Dis.BackColor == Color.Lime)
        //        {
        //            if (dut.SetSoftTxDis(1) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh1_Dis.BackColor = Color.Gray;
        //            btnTxCh1_4_Dis.BackColor = Color.Gray;
        //        }
        //        else
        //        {
        //            if (dut.TxChannelEnable(1) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh1_Dis.BackColor = Color.Lime;
        //            if ((btnTxCh1_Dis.BackColor == Color.Lime) && (btnTxCh2_Dis.BackColor == Color.Lime) && (btnTxCh3_Dis.BackColor == Color.Lime) && (btnTxCh4_Dis.BackColor == Color.Lime))
        //            {
        //                btnTxCh1_4_Dis.BackColor = Color.Lime;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnTxCh2_Dis_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btnTxCh2_Dis.BackColor == Color.Lime)
        //        {
        //            if (dut.SetSoftTxDis(2) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh2_Dis.BackColor = Color.Gray;
        //            btnTxCh1_4_Dis.BackColor = Color.Gray;
        //        }
        //        else
        //        {
        //            if (dut.TxChannelEnable(2) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh2_Dis.BackColor = Color.Lime;
        //            if ((btnTxCh1_Dis.BackColor == Color.Lime) && (btnTxCh2_Dis.BackColor == Color.Lime) && (btnTxCh3_Dis.BackColor == Color.Lime) && (btnTxCh4_Dis.BackColor == Color.Lime))
        //            {
        //                btnTxCh1_4_Dis.BackColor = Color.Lime;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnTxCh3_Dis_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btnTxCh3_Dis.BackColor == Color.Lime)
        //        {
        //            if (dut.SetSoftTxDis(3) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh3_Dis.BackColor = Color.Gray;
        //            btnTxCh1_4_Dis.BackColor = Color.Gray;
        //        }
        //        else
        //        {
        //            if (dut.TxChannelEnable(3) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh3_Dis.BackColor = Color.Lime;
        //            if ((btnTxCh1_Dis.BackColor == Color.Lime) && (btnTxCh2_Dis.BackColor == Color.Lime) && (btnTxCh3_Dis.BackColor == Color.Lime) && (btnTxCh4_Dis.BackColor == Color.Lime))
        //            {
        //                btnTxCh1_4_Dis.BackColor = Color.Lime;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void btnTxCh4_Dis_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (btnTxCh4_Dis.BackColor == Color.Lime)
        //        {
        //            if (dut.SetSoftTxDis(4) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh4_Dis.BackColor = Color.Gray;
        //            btnTxCh1_4_Dis.BackColor = Color.Gray;
        //        }
        //        else
        //        {
        //            if (dut.TxChannelEnable(4) == false)
        //            {
        //                throw new Exception();
        //            }
        //            btnTxCh4_Dis.BackColor = Color.Lime;
        //            if ((btnTxCh1_Dis.BackColor == Color.Lime) && (btnTxCh2_Dis.BackColor == Color.Lime) && (btnTxCh3_Dis.BackColor == Color.Lime) && (btnTxCh4_Dis.BackColor == Color.Lime))
        //            {
        //                btnTxCh1_4_Dis.BackColor = Color.Lime;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void tabControl1_Selected(object sender, TabControlEventArgs e)
        //{
        //    try
        //    {
        //        if (this.tabControl1.SelectedTab.ToString().Contains("Ch On/Off"))
        //        {
        //            this.ChOnOff();
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void ChOnOff()
        //{
        //    try
        //    {
        //        Button[] btns = new Button[10];
        //        btns[0] = this.btnTxCh1_4_Dis;
        //        btns[1] = this.btnTxCh1_Dis;
        //        btns[2] = this.btnTxCh2_Dis;
        //        btns[3] = this.btnTxCh3_Dis;
        //        btns[4] = this.btnTxCh4_Dis;
        //        btns[5] = this.btnRxCh1_4_Dis;
        //        btns[6] = this.btnRxCh1_Dis;
        //        btns[7] = this.btnRxCh2_Dis;
        //        btns[8] = this.btnRxCh3_Dis;
        //        btns[9] = this.btnRxCh4_Dis;

        //        dut = new QSFP28_SNOEC(dataTable_DUTCoeffControlByPN);
        //        byte[] buff = dut.GetTxChEnStatus();

        //        if ((buff[0] & 0x0F) == 0x0F)
        //        {
        //            btns[0].BackColor = Color.Gray;
        //        }
        //        else
        //        {
        //            btns[0].BackColor = Color.Lime;
        //        }

        //        for (int i = 0; i < 4; i++)
        //        {
        //            if (((buff[0] >> i) & 1) == 1)
        //            {
        //                btns[i + 1].BackColor = Color.Gray;
        //            }
        //            else
        //            {
        //                btns[i + 1].BackColor = Color.Lime;
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("No link.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        //private void qSFP28SR4ToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    this.qSFP28SR4ToolStripMenuItem.Checked = true;
        //    this.qSFP28CWDM4ToolStripMenuItem.Checked = false;
        //    this.labelTitle.Text = "QSFP28 SR4 GUI";
        //    string strFileSourse = Application.StartupPath + @"\Map\" + "QSFP28_SR4_Map" + ".xlsx";
        //    dataTable_DUTCoeffControlByPN = new DUTCoeffControlByPN(GetExcelTable(strFileSourse));
        //    dut = new QSFP28_SNOEC(dataTable_DUTCoeffControlByPN);
        //}

        /// <summary>
        /// 将Excel文件读取到DataTable
        /// </summary>
        /// <param name="excelFilePath"></param>
        /// <returns></returns>
        public DataTable GetExcelTable(string excelFilePath)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Sheets sheets;
                Microsoft.Office.Interop.Excel.Workbook workbook;
                System.Data.DataTable dt = new System.Data.DataTable();
                if (app == null)
                {
                    return null;
                }


                object oMissiong = System.Reflection.Missing.Value;
                workbook = app.Workbooks.Open(excelFilePath, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);


                //将数据读入到DataTable中——Start   
                sheets = workbook.Worksheets;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);
                if (worksheet == null)
                    return null;


                string cellContent;
                int iRowCount = worksheet.UsedRange.Rows.Count;
                int iColCount = worksheet.UsedRange.Columns.Count;
                Microsoft.Office.Interop.Excel.Range range;


                for (int iRow = 1; iRow <= iRowCount; iRow++)
                {
                    DataRow dr = dt.NewRow();


                    for (int iCol = 1; iCol <= iColCount; iCol++)
                    {
                        range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[iRow, iCol];


                        cellContent = (range.Value2 == null) ? "" : range.Text.ToString();


                        if (iRow == 1)
                        {
                            dt.Columns.Add(cellContent);
                        }
                        else
                        {
                            dr[iCol - 1] = cellContent;
                        }
                    }


                    if (iRow != 1)
                        dt.Rows.Add(dr);
                }


                //将数据读入到DataTable中——End
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();


                return dt;
            }
            catch
            {
                return null;
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm helpFrom = new HelpForm();
            helpFrom.ShowDialog();
        }

        private void dataGridView4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var content = this.dataGridView4.CurrentCell.Value;
            if (content == null)
            {
                return;
            }

            Regex reg = new Regex(@"^[0-9a-fA-F]{1,2}$");
            if (!reg.IsMatch((string)content))
            {
                this.dataGridView4.CurrentCell.Value = null;
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    TestForm testFrom = new TestForm(dut);
        //    testFrom.ShowDialog();
        //}

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

        //private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    CalculateForm calculateFrom = new CalculateForm(dut, dataTable_DUTCoeffControlByPN);
        //    calculateFrom.Show();
        //}

        //private void luxshareToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    QSFP28_SNOEC.company = QSFP28_SNOEC.Company.Luxshare;
        //    this.innoToolStripMenuItem.Checked = false;
        //    this.genericToolStripMenuItem.Checked = true;
        //    this.sNOECToolStripMenuItem.Checked = false;
        //}

        private void dataGridView6_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var content = this.dataGridView6.CurrentCell.Value;
            if (content == null)
            {
                return;
            }

            Regex reg = new Regex(@"^[0-9a-fA-F]{1,2}$");
            if (!reg.IsMatch((string)content))
            {
                this.dataGridView6.CurrentCell.Value = null;
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QSFP_DD_From_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnRead_SN_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtSN.Text = dut.ReadSN();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRead_FW_Version_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtFW.Text = dut.ReadFWRev();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRead_PN_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtPN.Text = dut.ReadPN();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRead_VendorName_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtVendorName.Text = dut.ReadVendorName();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

