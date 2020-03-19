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
    public partial class SFP28_Form : Form
    {
        private SFP28_SNOEC dut;
        private int deviceAddress = 0xA0;
        private const int maxCells = 16 * 8;

        public SFP28_Form()
        {
            InitializeComponent();

            this.comboBoxDeviceIndex.SelectedIndex = 1;
            this.comboBoxSoftHard.SelectedIndex = 0;
            this.comboBoxFrequency.SelectedIndex = 0;

            this.tabControl1.SelectedIndex = 1;

            this.dataGridView4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.AllowUserToAddRows = false;
            this.dataGridView6.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView6.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView6.AllowUserToAddRows = false;

            for (int i = 0; i < this.dataGridView4.Columns.Count; i++)
            {
                this.dataGridView4.Columns[i].Width = this.dataGridView4.Width / this.dataGridView4.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView6.Columns.Count; i++)
            {
                this.dataGridView6.Columns[i].Width = this.dataGridView6.Width / this.dataGridView6.Columns.Count;
            }

            this.dataGridView4.Rows.Add(8);
            this.dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView6.Rows.Add(8);
            this.dataGridView6.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

        private void btnReadWrite_Click(object sender, EventArgs e)
        {
            this.btnReadWrite.Enabled = false;
            this.btnReadWrite.BackColor = Color.Yellow;
            this.btnReadWrite.Refresh();
            deviceAddress = 0;
            try
            {
                byte[] buf_deviceAddress = Algorithm.HexStringToBytes(this.domainUpDownDeviceAddress.Text);

                for (int i = 0; i < buf_deviceAddress.Length; i++)
                {
                    deviceAddress += buf_deviceAddress[i] << 8 * i;
                }
            }
            catch
            {
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                return;
            }

            try
            {
                if (this.tabControl1.SelectedTab.ToString().Contains("I2C Operation"))
                {
                    if (this.radioButtonI2C_Read.Checked)
                    {
                        this.I2C_Read();
                    }
                    if (this.radioButtonI2C_Write.Checked)
                    {
                        DialogResult result = MessageBox.Show("Are you sure to write", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            this.I2C_Write();
                        }
                    }
                }

                if (this.tabControl1.SelectedTab.ToString().Contains("EPIC"))
                {
                    Tab_EPIC();
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

        private void Tab_EPIC()
        {
            int maxCells_dataGridView6 = 16 * 8;
            byte map_page_chip_control = 0x06;

            byte map_address_chip_control_regAddress = 0;
            byte map_address_chip_control_reg_data_length = 0;
            byte map_address_chip_control_reg_read_trigger = 0;
            byte map_address_chip_control_reg_write_trigger = 0;

            byte value_map_address_chip_control_regAddress = 0;
            byte value_map_address_chip_control_reg_data_length = 0;
            byte value_map_address_chip_control_reg_read_trigger = 0;
            byte value_map_address_chip_control_reg_write_trigger = 0;

            byte map_update_chip_reg_page = 0x07;

            map_address_chip_control_regAddress = 0x80;
            map_address_chip_control_reg_data_length = 0x81;
            map_address_chip_control_reg_read_trigger = 0x82;
            map_address_chip_control_reg_write_trigger = 0x83;

            value_map_address_chip_control_regAddress = (byte)this.numericUpDownSemtechChip_Address.Value;
            value_map_address_chip_control_reg_data_length = (byte)this.numericUpDownSemtechChip_Bytes.Value;
            byte[] buffer = new byte[(int)this.numericUpDownSemtechChip_Bytes.Value*4];

            if (this.comboBoxSemtechChip_Operation.SelectedIndex == 0)//read
            {
                //clear cells
                for (int i = 0; i < maxCells_dataGridView6; i++)
                {
                    this.dataGridView6.Rows[i / 16].Cells[i % 16].Value = null;
                }
                this.dataGridView6.Refresh();

                if ((int)this.numericUpDownSemtechChip_Bytes.Value > 0)
                {
                    value_map_address_chip_control_reg_read_trigger = 1;
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_regAddress, new byte[] { value_map_address_chip_control_regAddress });                    
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_data_length, new byte[] { value_map_address_chip_control_reg_data_length });
                    dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_read_trigger, new byte[] { value_map_address_chip_control_reg_read_trigger });
                    buffer = dut.ReadReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_update_chip_reg_page, 0x80, buffer.Length);


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

            if (this.comboBoxSemtechChip_Operation.SelectedIndex == 1)//write
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
                dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_regAddress, new byte[] { value_map_address_chip_control_regAddress });
                dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_data_length, new byte[] { value_map_address_chip_control_reg_data_length });
                dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_update_chip_reg_page, 0x80, buffer);
                dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, map_page_chip_control, map_address_chip_control_reg_write_trigger, new byte[] { value_map_address_chip_control_reg_write_trigger });
            }
        }


        private void I2C_Read()
        {
            //clear cells
            for (int i = 0; i < maxCells; i++)
            {
                this.dataGridView4.Rows[i / 16].Cells[i % 16].Value = null;
            }
            this.dataGridView4.Refresh();
            byte[] buff = new byte[(int)this.numericUpDownBytes.Value];
            if ((int)this.numericUpDownBytes.Value > 0)
            {
                int regAdress = 0;
                try
                {
                    byte[] buf_regAdress = Algorithm.HexStringToBytes(this.txtRegAdress.Text);

                    for (int i = 0; i < buf_regAdress.Length; i++)
                    {
                        regAdress += buf_regAdress[i] << 8 * i;
                    }
                }
                catch
                {
                    MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnReadWrite.Enabled = true;
                    this.btnReadWrite.BackColor = SystemColors.Control;
                    return;
                }

                buff = dut.ReadReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, (int)this.numericUpDownPage.Value, regAdress, (int)this.numericUpDownBytes.Value);
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
                this.dataGridView4.Rows[i / 16].Cells[i % 16].Value = Convert.ToString(buff[i], 16).ToUpper();
            }
        }

        private void I2C_Write()
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
            int regAdress = 0;
            try
            {
                byte[] buf_regAdress = Algorithm.HexStringToBytes(this.txtRegAdress.Text);

                for (int i = 0; i < buf_regAdress.Length; i++)
                {
                    regAdress += buf_regAdress[i] << 8 * i;
                }
            }
            catch
            {
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                return;
            }

            byte[] buff = dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, (int)this.numericUpDownPage.Value, regAdress, writeData);
            if (buff == null)
            {
                this.btnReadWrite.Enabled = true;
                this.btnReadWrite.BackColor = SystemColors.Control;
                return;
            }
        }

        private void SFP28_Form_Load(object sender, EventArgs e)
        {
            try
            {
                dut = new SFP28_SNOEC();
                this.tabControl1.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SFP28_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnReadWrite.Enabled = true;

            if (this.tabControl1.SelectedTab.ToString().Contains("I2C Operation"))
            {
                this.btnReadWrite.Text = "Execute";
            }
            else if (this.tabControl1.SelectedTab.ToString().Contains("EPIC"))
            {
                this.btnReadWrite.Text = "Execute";
                this.comboBoxSemtechChip_Operation.SelectedIndex = 0;
            }
            else
            {
                this.btnReadWrite.Text = "Read";
            }
        }

        private void btnBrowse_I2C_Batch_File_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "EXCEL文件|*.xlsx|TXT文件|*.txt";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtI2C_Batch_FilePath.Text = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "file path error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

                //add to kill excel.exe -nicholas 20181128
                foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("Excel"))
                {
                    if (string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        p.Kill();
                    }
                }

                return dt;
            }
            catch
            {
                return null;
            }
        }

        private void btnLoad_I2C_Batch_Click(object sender, EventArgs e)
        {
            try
            {
                IOPort.SoftHard softHard = IOPort.SoftHard.SerialPort;
                switch (this.comboBoxSoftHard.Text)
                {
                    case "OnEasyB_I2C":
                        softHard = IOPort.SoftHard.OnEasyB_I2C;
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
                                MessageBox.Show("Disconnect to OnEasyB_I2C", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Disconnect to OnEasyB_I2C", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        this.comboBoxDeviceIndex.SelectedIndex = 0;
                        break;

                    case "SerialPort":
                        softHard = IOPort.SoftHard.SerialPort;
                        break;

                    default:
                        softHard = IOPort.SoftHard.SerialPort;
                        break;
                }

                if (this.txtI2C_Batch_FilePath.Text == "")
                {
                    return;
                }

                this.btnLoad_I2C_Batch.Enabled = false;
                this.btnLoad_I2C_Batch.BackColor = Color.Yellow;
                this.btnLoad_I2C_Batch.Refresh();

                DirectoryInfo directoryInfo = Directory.GetParent(this.txtI2C_Batch_FilePath.Text);
                if (!directoryInfo.Exists)
                {
                    MessageBox.Show("File is no exist", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnLoadSetting.Enabled = true;
                    this.btnLoadSetting.BackColor = SystemColors.Control;
                    return;
                }

                DataTable setting_table = this.GetExcelTable(this.txtI2C_Batch_FilePath.Text);

                for (int row = 0; row < setting_table.Rows.Count; row++)
                {
                    byte[] deviceAddress = new byte[1];
                    int regAddress = 0;
                    byte[] page_num = new byte[1];
                    byte[] writeData = new byte[1];

                    try
                    {
                        deviceAddress = Algorithm.HexStringToBytes(setting_table.Rows[row][0].ToString());
                        page_num = Algorithm.HexStringToBytes(setting_table.Rows[row][1].ToString());

                        byte[] buf_regAdress = Algorithm.HexStringToBytes(setting_table.Rows[row][2].ToString());

                        for (int i = 0; i < buf_regAdress.Length; i++)
                        {
                            regAddress += buf_regAdress[i] << 8 * i;
                        }

                        writeData = Algorithm.HexStringToBytes(setting_table.Rows[row][3].ToString());

                    }
                    catch
                    {
                        MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btnLoad_I2C_Batch.Enabled = true;
                        this.btnLoad_I2C_Batch.BackColor = SystemColors.Control;
                        return;
                    }

                    byte[] buff = dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress[0], page_num[0], regAddress, writeData);
                    
                    if (buff == null)
                    {
                        this.btnLoad_I2C_Batch.Enabled = true;
                        this.btnLoad_I2C_Batch.BackColor = SystemColors.Control;
                        return;
                    }
                }

                this.btnLoad_I2C_Batch.Enabled = true;
                this.btnLoad_I2C_Batch.BackColor = SystemColors.Control;
                MessageBox.Show("Done", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowseSetting_Click(object sender, EventArgs e)
        {
            try
            {              

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "EXCEL文件|*.xlsx";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtSettingFilePath.Text = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "file path error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadSetting_Click(object sender, EventArgs e)
        {
            try
            {
                this.btnLoadSetting.Enabled = false;
                this.btnLoadSetting.BackColor = Color.Yellow;
                this.btnLoadSetting.Refresh();

                if (this.txtSettingFilePath.Text == "")
                {
                    return;
                }

                DirectoryInfo directoryInfo = Directory.GetParent(this.txtSettingFilePath.Text);
                if (!directoryInfo.Exists)
                {
                    MessageBox.Show("File is no exist", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnLoadSetting.Enabled = true;
                    this.btnLoadSetting.BackColor = SystemColors.Control;
                    return;
                }

                DataTable setting_table = this.GetExcelTable(this.txtSettingFilePath.Text);
                int cycles = setting_table.Rows.Count / 8;

                byte[] buff = new byte[128];
                int i = 0;
                for (int row = 0; row < setting_table.Rows.Count; row++)
                {
                    for (int colunm = 0; colunm < 16; colunm++)
                    {
                        object ob = setting_table.Rows[row][colunm + 1].ToString();
                        if (ob == null)
                        {
                            this.btnLoadSetting.Enabled = true;
                            this.btnLoadSetting.BackColor = SystemColors.Control;
                            return;
                        }
                        buff[i++] = byte.Parse((string)ob, System.Globalization.NumberStyles.HexNumber);
                    }
                    if (i == 128)
                    {
                        i = 0;
                        object ob = setting_table.Rows[row - 7][0].ToString();
                        if (ob == null)
                        {
                            this.btnLoadSetting.Enabled = true;
                            this.btnLoadSetting.BackColor = SystemColors.Control;
                            return;
                        }
                        short page_addr = short.Parse((string)ob, System.Globalization.NumberStyles.HexNumber);
                        byte page_num = (byte)((page_addr >> 8) & 0xFF);
                        byte addr_start = (byte)(page_addr & 0xFF);
                        dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, page_num, addr_start, buff);
                    }

                }

                dut.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress, 0x06, 0x87, new byte[1] { 1 });

                this.btnLoadSetting.Enabled = true;
                this.btnLoadSetting.BackColor = SystemColors.Control;
                MessageBox.Show("Done", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
