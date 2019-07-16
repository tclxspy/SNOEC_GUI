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
    public partial class Edemux_Form : Form
    {
        private QSFP28_SNOEC dut;
        private int deviceAddress = 0xA0;
        private const int maxCells = 16 * 8;
        public Edemux_Form()
        {
            InitializeComponent();
            this.comboBoxDeviceIndex.SelectedIndex = 1;
            this.comboBoxSoftHard.SelectedIndex = 0;
            this.comboBoxFrequency.SelectedIndex = 0;

            this.tabControl1.SelectedIndex = 1;

            this.dataGridView4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView4.AllowUserToAddRows = false;

            for (int i = 0; i < this.dataGridView4.Columns.Count; i++)
            {
                this.dataGridView4.Columns[i].Width = this.dataGridView4.Width / this.dataGridView4.Columns.Count;
            }


            this.dataGridView4.Rows.Add(8);
            this.dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnReadWrite.Enabled = true;

            if (this.tabControl1.SelectedTab.ToString().Contains("I2C Operation"))
            {
                this.btnReadWrite.Text = "Execute";
            }
            else
            {
                this.btnReadWrite.Text = "Read";
            }
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

                if (this.tabControl1.SelectedTab.ToString().Contains("SETTING"))
                {

                }

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

        private void Edemux_Form_Load(object sender, EventArgs e)
        {
            try
            {
                dut = new QSFP28_SNOEC();
                QSFP28_SNOEC.company = QSFP28_SNOEC.Company.Nopasswords_Nopage;
                this.tabControl1.SelectedIndex = 6;
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
                
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
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
               
        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestForm testFrom = new TestForm(dut);
            testFrom.ShowDialog();
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
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Disconnect to OnEasyB_I2C", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    this.comboBoxDeviceIndex.SelectedIndex = 0;
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
            //IOPort.Frequency = (byte)(this.comboBoxFrequency.SelectedIndex + 1);
            SNOEC_USB_I2C.USB_I2C.I2C_standard_speed = (this.comboBoxFrequency.SelectedIndex == 0) ? false : true;
        }      
  
        private void Edemux_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void checkBoxSoft_I2C_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBoxSoft_I2C.Checked)
            {
                SNOEC_USB_I2C.USB_I2C.i2c_soft_enable = true;
            }
            else
            {
                SNOEC_USB_I2C.USB_I2C.i2c_soft_enable = false;
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
                    this.btnLoad_I2C_Batch.Enabled = true;
                    this.btnLoad_I2C_Batch.BackColor = SystemColors.Control;
                    return;
                }

                DataTable setting_table = this.GetExcelTable(this.txtI2C_Batch_FilePath.Text);                

                for (int row = 0; row < setting_table.Rows.Count; row++)
                {
                    byte[] deviceAddress = new byte[1];
                    int regAddress = 0;
                    byte[] writeData = new byte[1];

                    try
                    {
                        deviceAddress = Algorithm.HexStringToBytes(setting_table.Rows[row][0].ToString());

                        byte[] buf_regAdress = Algorithm.HexStringToBytes(setting_table.Rows[row][1].ToString());

                        for (int i = 0; i < buf_regAdress.Length; i++)
                        {
                            regAddress += buf_regAdress[i] << 8 * i;
                        }

                        writeData = Algorithm.HexStringToBytes(setting_table.Rows[row][2].ToString());

                    }
                    catch
                    {
                        MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btnLoad_I2C_Batch.Enabled = true;
                        this.btnLoad_I2C_Batch.BackColor = SystemColors.Control;
                        return;
                    }


                    byte[] buff = IOPort.WriteReg(this.comboBoxDeviceIndex.SelectedIndex, deviceAddress[0], regAddress, softHard, writeData);
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            string fileName = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "TXT文件|*.txt";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "file path error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                DirectoryInfo directoryInfo = Directory.GetParent(fileName);

                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }

                FileStream fileStream = null;
                StreamWriter writer = null;
                FileInfo fileInfo = new FileInfo(fileName);
                if (!fileInfo.Exists)
                {
                    fileStream = fileInfo.Create();
                }
                else
                {
                    fileStream = fileInfo.Open(FileMode.Append, FileAccess.Write);
                }
                writer = new StreamWriter(fileStream);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("// " + DateTime.Now.ToShortDateString() + "   " + DateTime.Now.ToShortTimeString());
                sb.AppendLine("// i2c address: " + this.domainUpDownDeviceAddress.Text);
                sb.AppendLine("// page num: " + this.numericUpDownPage.Value);
                sb.AppendLine("// reg address: " + this.txtRegAdress.Text);
                sb.AppendLine("// length: " + this.numericUpDownBytes.Value);

                foreach (DataGridViewRow dr in this.dataGridView4.Rows)
                {
                    string temp = "";
                    for (int i = 0; i < this.dataGridView4.ColumnCount; i++)
                    {
                        if(dr.Cells[i].Value == null)
                        {
                            sb.AppendLine(temp);
                            goto save;
                        }

                        if (i == this.dataGridView4.ColumnCount - 1)
                        {
                            temp += dr.Cells[i].Value;
                        }
                        else
                        {
                            temp += dr.Cells[i].Value + "\t";
                        }
                    }
                    sb.AppendLine(temp);
                }
                save: writer.WriteLine(sb.ToString());
                writer.Close();
                writer.Dispose();
                fileStream.Close();
                fileStream.Dispose();

                MessageBox.Show("Done", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

