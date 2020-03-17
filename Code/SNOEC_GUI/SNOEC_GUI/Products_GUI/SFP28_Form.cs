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
    }
}
