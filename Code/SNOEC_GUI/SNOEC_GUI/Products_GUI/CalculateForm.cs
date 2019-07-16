using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SNOEC_GUI
{
    public partial class CalculateForm : Form
    {
        private QSFP28_SNOEC dut;
        private DUTCoeffControlByPN dataTable_DUTCoeffControlByPN;

        public CalculateForm(QSFP28_SNOEC qsfp_dut, DUTCoeffControlByPN table)
        {
            InitializeComponent();

            this.dut = qsfp_dut;
            this.dataTable_DUTCoeffControlByPN = table;

            this.dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView2.AllowUserToAddRows = false;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].Width = this.dataGridView1.Width / this.dataGridView1.Columns.Count;
            }

            for (int i = 0; i < this.dataGridView2.Columns.Count; i++)
            {
                this.dataGridView2.Columns[i].Width = this.dataGridView2.Width / this.dataGridView2.Columns.Count;
            }

            this.dataGridView1.Rows.Add(1);
            this.dataGridView2.Rows.Add(1);
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

        private void btnConvert_1_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtDEC_1.Clear();
                int length = 2;

                if(this.radioButton_UInt16_1.Checked || this.radioButton_Int16_1.Checked)
                {
                    length = 2;
                }
                else if (this.radioButton_IEEE754_1.Checked)
                {
                    length = 4;
                }
                else
                {
                    return;
                }


                byte[] buffer = new byte[length];

                for (int i = 0; i < buffer.Length; i++)
                {
                    object ob = this.dataGridView1.Rows[0].Cells[i].Value;
                    if (ob == null)
                    {
                        return;
                    }
                    buffer[i] = byte.Parse((string)ob, System.Globalization.NumberStyles.HexNumber);
                }


                if (this.radioButton_UInt16_1.Checked)
                {
                    this.txtDEC_1.Text = System.BitConverter.ToUInt16(buffer, 0).ToString();
                }
                else if (this.radioButton_Int16_1.Checked)
                {
                    this.txtDEC_1.Text = System.BitConverter.ToInt16(buffer, 0).ToString();
                }
                else if (this.radioButton_IEEE754_1.Checked)
                {
                    this.txtDEC_1.Text = System.BitConverter.ToSingle(buffer, 0).ToString();
                }
                else
                {
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConvert_2_Click(object sender, EventArgs e)
        {
            try
            {
                //clear cells
                for (int i = 0; i < 4; i++)
                {
                    this.dataGridView2.Rows[0].Cells[i].Value = null;
                }

                if (this.txtDEC_2.Text == "")
                {
                    return;
                }

                byte[] buffer = new byte[2];

                if (this.radioButton_UInt16_2.Checked)
                {
                    buffer = System.BitConverter.GetBytes(Convert.ToUInt16(this.txtDEC_2.Text));
                }
                else if (this.radioButton_Int16_2.Checked)
                {
                    buffer = System.BitConverter.GetBytes(Convert.ToInt16(this.txtDEC_2.Text));
                }
                else if (this.radioButton_IEEE754_2.Checked)
                {
                    buffer = System.BitConverter.GetBytes(Convert.ToSingle(this.txtDEC_2.Text));
                }
                else
                {
                    return;
                }

                for (int i = 0; i < buffer.Length; i++)
                {
                    this.dataGridView2.Rows[0].Cells[i].Value = Convert.ToString(buffer[i], 16).ToUpper();
                }
            }
            catch
            {
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {            
                byte[] buff_ADC = new byte[2] { 0xF5, 0x09 };
                byte[] buff_Slope = new byte[2] { 0x45, 0x19 };
                byte[] buff_Shift = new byte[2] { 0x09, 0x00 };
                byte[] buff_Offset = new byte[2] { 0x50, 0xFD };

                UInt16 dec_ADC = System.BitConverter.ToUInt16(buff_ADC, 0);
                Int16 dec_Slope = System.BitConverter.ToInt16(buff_Slope, 0);
                Int16 dec_Shift = System.BitConverter.ToInt16(buff_Shift, 0);
                Int16 dec_Offset = System.BitConverter.ToInt16(buff_Offset, 0);

                int channel = 0;

                if (this.radioButton_Ch0.Checked) { channel = 1; }
                if (this.radioButton_Ch1.Checked) { channel = 2; }
                if (this.radioButton_Ch2.Checked) { channel = 3; }
                if (this.radioButton_Ch3.Checked) { channel = 4; }

                if (this.checkBoxRead.Checked)
                { 
                    if (this.radioButton_Temp.Checked)
                    {
                        this.txtADC_Dec.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.TemperatureAdc,0).ToString();
                        this.txtSlope_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTempSlope, 0);
                        this.txtShift_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTempShift, 0);
                        this.txtOffset_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTempOffset, 0);

                        this.txtReading_DMI.Text = dut.ReadDmiTemp().ToString("f3");
                    }
                    else if (this.radioButton_Vcc.Checked)
                    {
                        this.txtADC_Dec.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.VccAdc, 0).ToString();
                        this.txtSlope_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiVccSlope, 0);
                        this.txtShift_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiVccShift, 0);
                        this.txtOffset_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiVccOffset, 0);

                        this.txtReading_DMI.Text = dut.ReadDmiVcc().ToString("f3");
                    }
                    else if (this.radioButton_TxPower.Checked)
                    {
                        this.txtADC_Dec.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.TxPowerAdc, channel).ToString();
                        this.txtSlope_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTxpowerSlope, channel);
                        this.txtShift_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTxpowerShift, channel);
                        this.txtOffset_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTxpowerOffset, channel);

                        this.txtReading_DMI.Text = dut.ReadDmiTxP(1).ToString("f3");
                    }
                    else if (this.radioButton_RxPower.Checked)
                    {
                        this.txtADC_Dec.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.RxPowerAdc, channel).ToString();
                        this.txtSlope_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiRxpowerSlope, channel);
                        this.txtShift_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiRxpowerShift, channel);
                        this.txtOffset_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiRxpowerOffset, channel);

                        this.txtReading_DMI.Text = dut.ReadDmiRxP(1).ToString("f3");
                    }
                    else if (this.radioButton_Ibias.Checked)
                    {
                        this.txtADC_Dec.Text = dut.ReadADC(QSFP28_SNOEC.NameOfADC.TxBiasAdc, channel).ToString();
                        this.txtSlope_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTxBiasSlope, channel);
                        this.txtShift_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTxBiasShift, channel);
                        this.txtOffset_Dec.Text = dut.GetCoeff(QSFP28_SNOEC.Coeff.DmiTxBiasOffset, channel);

                        this.txtReading_DMI.Text = dut.ReadDmiBias(1).ToString("f3");
                    }


                    dec_ADC = Convert.ToUInt16(this.txtADC_Dec.Text);
                    dec_Slope = Convert.ToInt16(this.txtSlope_Dec.Text);
                    dec_Shift = Convert.ToInt16(this.txtShift_Dec.Text);
                    dec_Offset = Convert.ToInt16(this.txtOffset_Dec.Text);

                    buff_ADC = BitConverter.GetBytes(dec_ADC);
                    buff_Slope = BitConverter.GetBytes(dec_Slope);
                    buff_Shift = BitConverter.GetBytes(dec_Shift);
                    buff_Offset = BitConverter.GetBytes(dec_Offset);

                    this.txtADC_Hex.Text = Algorithm.BytesTohexString(buff_ADC);
                    this.txtSlope_Hex.Text = Algorithm.BytesTohexString(buff_Slope);
                    this.txtShift_Hex.Text = Algorithm.BytesTohexString(buff_Shift);
                    this.txtOffset_Hex.Text = Algorithm.BytesTohexString(buff_Offset);
                }
                else
                {
                    buff_ADC = Algorithm.HexStringToBytes(this.txtADC_Hex.Text);
                    buff_Slope = Algorithm.HexStringToBytes(this.txtSlope_Hex.Text);
                    buff_Shift = Algorithm.HexStringToBytes(this.txtShift_Hex.Text);
                    buff_Offset = Algorithm.HexStringToBytes(this.txtOffset_Hex.Text);


                    dec_ADC = System.BitConverter.ToUInt16(buff_ADC, 0);
                    dec_Slope = System.BitConverter.ToInt16(buff_Slope, 0);
                    dec_Shift = System.BitConverter.ToInt16(buff_Shift, 0);
                    dec_Offset = System.BitConverter.ToInt16(buff_Offset, 0);

                    this.txtADC_Dec.Text = dec_ADC.ToString();
                    this.txtSlope_Dec.Text = dec_Slope.ToString();
                    this.txtShift_Dec.Text = dec_Shift.ToString();
                    this.txtOffset_Dec.Text = dec_Offset.ToString();
                }

                double dec_Calculate_MSA = dec_ADC * dec_Slope / Math.Pow(2, dec_Shift) + dec_Offset;
                this.txtCalculate_MSA.Text = dec_Calculate_MSA.ToString("f3");

                if (this.radioButton_Temp.Checked) { this.txtCalculate_DMI.Text = (dec_Calculate_MSA / 256).ToString("f3"); }
                if (this.radioButton_Vcc.Checked) { this.txtCalculate_DMI.Text = (dec_Calculate_MSA / 10000).ToString("f3"); }
                if (this.radioButton_TxPower.Checked) { this.txtCalculate_DMI.Text = (10 * Math.Log10(dec_Calculate_MSA / 10000)).ToString("f3"); }
                if (this.radioButton_RxPower.Checked) { this.txtCalculate_DMI.Text = (10 * Math.Log10(dec_Calculate_MSA / 10000)).ToString("f3"); }
                if (this.radioButton_Ibias.Checked) { this.txtCalculate_DMI.Text = (dec_Calculate_MSA * 2 / 1000).ToString("f3"); }
            }
            catch(Exception ex)
            {
                if (this.checkBoxRead.Checked)
                {
                    MessageBox.Show("No link. please click read buton fistly on tha main GUI", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var content = this.dataGridView1.CurrentCell.Value;
            if (content == null)
            {
                return;
            }

            Regex reg = new Regex(@"^[0-9a-fA-F]{1,2}$");
            if (!reg.IsMatch((string)content))
            {
                this.dataGridView1.CurrentCell.Value = null;
                MessageBox.Show("Unfomart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
