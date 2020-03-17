using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace SNOEC_GUI
{
    public partial class DR4_TX_Form : Form
    {
        private MACOM_006409 macom_006409 = null;
        private const int maxCells = 16 * 8;

        public DR4_TX_Form()
        {
            InitializeComponent();

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

        private void btnReadWrite_Click(object sender, EventArgs e)
        {
            this.btnReadWrite.Enabled = false;
            this.btnReadWrite.BackColor = Color.Yellow;
            this.btnReadWrite.Refresh();

            try
            {
                if (this.tabControl1.SelectedTab.ToString().Contains("SPI Operation"))
                {
                    if (this.radioButton_Read.Checked)
                    {
                        this.SPI_Read();
                    }
                    if (this.radioButton_Write.Checked)
                    {
                        DialogResult result = MessageBox.Show("Are you sure to write", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            this.SPI_Write();
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

        private void SPI_Read()
        {
            //clear cells
            for (int i = 0; i < maxCells; i++)
            {
                this.dataGridView4.Rows[i / 16].Cells[i % 16].Value = null;
            }
            this.dataGridView4.Refresh();
            int counts = (int)this.numericUpDownBytes.Value;

            short[] buff = new short[counts];

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
                    return;
                }

                if (macom_006409 == null)
                {
                    macom_006409 = new MACOM_006409();
                }

                for (ushort i = 0; i < counts; i++)
                {
                    buff[i] = macom_006409.ReadReg((ushort)(regAdress + i));
                    if (buff[i] == -1)
                    {
                        MessageBox.Show("macom_006409 write ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            int length = Math.Min(maxCells, buff.Length);

            for (int i = 0; i < length; i++)
            {
                this.dataGridView4.Rows[i / 16].Cells[i % 16].Value = Convert.ToString(buff[i], 16).ToUpper();
            }
        }

        private void SPI_Write()
        {
            int counts = (int)this.numericUpDownBytes.Value;
            ushort[] writeData = new ushort[counts];
            if (writeData.Length == 0)
            {
                return;
            }

            try
            {
                for (int i = 0; i < writeData.Length; i++)
                {
                    object ob = this.dataGridView4.Rows[i / 16].Cells[i % 16].Value;
                    if (ob == null)
                    {
                        return;
                    }
                    writeData[i] = ushort.Parse((string)ob, System.Globalization.NumberStyles.HexNumber);
                }
            }
            catch
            {
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                return;
            }


            if (macom_006409 == null)
            {
                macom_006409 = new MACOM_006409();
            }

            for (ushort i = 0; i < counts; i++)
            {
                if (macom_006409.WriteReg((ushort)(regAdress + i), writeData[i]) == false)
                {
                    MessageBox.Show("macom_006409 write ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            return;

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

        private void DR4_TX_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnVendorRead_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtVendorID.Text = "";
                this.txtVendorID.Refresh();
                Thread.Sleep(100);
                if (macom_006409 == null)
                {
                    macom_006409 = new MACOM_006409();
                }
                ushort vendorid = macom_006409.GetVendorID();
                this.txtVendorID.Text = "0x" + Convert.ToString(vendorid, 16).ToUpper();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnLoad_Batch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtBatch_FilePath.Text == "")
                {
                    return;
                }

                this.btnLoad_Batch.Enabled = false;
                this.btnLoad_Batch.BackColor = Color.Yellow;
                this.btnLoad_Batch.Refresh();

                DirectoryInfo directoryInfo = Directory.GetParent(this.txtBatch_FilePath.Text);
                if (!directoryInfo.Exists)
                {
                    MessageBox.Show("File is no exist", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnLoad_Batch.Enabled = true;
                    this.btnLoad_Batch.BackColor = SystemColors.Control;
                    return;
                }

                DataTable setting_table = this.GetExcelTable(this.txtBatch_FilePath.Text);

                for (int row = 0; row < setting_table.Rows.Count; row++)
                {
                    int regAddress = 0;
                    int writeData = 0;
                    try
                    {

                        byte[] buf_regAdress = Algorithm.HexStringToBytes(setting_table.Rows[row][1].ToString());
                        for (int i = 0; i < buf_regAdress.Length; i++)
                        {
                            regAddress += buf_regAdress[i] << 8 * i;
                        }

                        byte[] buf_writeData = Algorithm.HexStringToBytes(setting_table.Rows[row][2].ToString());
                        for (int i = 0; i < buf_writeData.Length; i++)
                        {
                            writeData += buf_writeData[i] << 8 * i;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btnLoad_Batch.Enabled = true;
                        this.btnLoad_Batch.BackColor = SystemColors.Control;
                        return;
                    }

                    if (macom_006409 == null)
                    {
                        macom_006409 = new MACOM_006409();
                    }

                    if (macom_006409.WriteReg((ushort)regAddress, (ushort)writeData) == false)
                    {
                        MessageBox.Show("No link. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.btnLoad_Batch.Enabled = true;
                        this.btnLoad_Batch.BackColor = SystemColors.Control;
                        return;
                    }
                }

                this.btnLoad_Batch.Enabled = true;
                this.btnLoad_Batch.BackColor = SystemColors.Control;
                MessageBox.Show("Done", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No link.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }  

        private void btnBrowse_Batch_File_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;//该值确定是否可以选择多个文件
                dialog.Title = "请选择文件夹";
                dialog.Filter = "EXCEL文件|*.xlsx|TXT文件|*.txt";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtBatch_FilePath.Text = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "file path error.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
