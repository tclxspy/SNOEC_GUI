namespace SNOEC_GUI
{
    partial class DR4_TX_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageI2C_Operation = new System.Windows.Forms.TabPage();
            this.btnBrowse_Batch_File = new System.Windows.Forms.Button();
            this.btnLoad_Batch = new System.Windows.Forms.Button();
            this.txtBatch_FilePath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.radioButton_Write = new System.Windows.Forms.RadioButton();
            this.radioButton_Read = new System.Windows.Forms.RadioButton();
            this.btnExport = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.dataGridView4 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn33 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn34 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn35 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn36 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn37 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn38 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn39 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn40 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn42 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn48 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnVendorRead = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVendorID = new System.Windows.Forms.TextBox();
            this.btnReadWrite = new System.Windows.Forms.Button();
            this.txtRegAdress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownBytes = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageI2C_Operation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBytes)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(62)))), ((int)(((byte)(91)))));
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.panel1.Size = new System.Drawing.Size(764, 522);
            this.panel1.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tabControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.ForeColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(10, 107);
            this.panel4.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(744, 405);
            this.panel4.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageI2C_Operation);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(744, 405);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageI2C_Operation
            // 
            this.tabPageI2C_Operation.Controls.Add(this.btnBrowse_Batch_File);
            this.tabPageI2C_Operation.Controls.Add(this.btnLoad_Batch);
            this.tabPageI2C_Operation.Controls.Add(this.txtBatch_FilePath);
            this.tabPageI2C_Operation.Controls.Add(this.groupBox1);
            this.tabPageI2C_Operation.Controls.Add(this.radioButton_Write);
            this.tabPageI2C_Operation.Controls.Add(this.radioButton_Read);
            this.tabPageI2C_Operation.Controls.Add(this.btnExport);
            this.tabPageI2C_Operation.Controls.Add(this.label54);
            this.tabPageI2C_Operation.Controls.Add(this.dataGridView4);
            this.tabPageI2C_Operation.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPageI2C_Operation.Location = new System.Drawing.Point(4, 25);
            this.tabPageI2C_Operation.Name = "tabPageI2C_Operation";
            this.tabPageI2C_Operation.Size = new System.Drawing.Size(736, 376);
            this.tabPageI2C_Operation.TabIndex = 4;
            this.tabPageI2C_Operation.Text = "SPI Operation";
            this.tabPageI2C_Operation.UseVisualStyleBackColor = true;
            // 
            // btnBrowse_Batch_File
            // 
            this.btnBrowse_Batch_File.Location = new System.Drawing.Point(10, 307);
            this.btnBrowse_Batch_File.Name = "btnBrowse_Batch_File";
            this.btnBrowse_Batch_File.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse_Batch_File.TabIndex = 32;
            this.btnBrowse_Batch_File.Text = "Browse";
            this.btnBrowse_Batch_File.UseVisualStyleBackColor = true;
            this.btnBrowse_Batch_File.Click += new System.EventHandler(this.btnBrowse_Batch_File_Click);
            // 
            // btnLoad_Batch
            // 
            this.btnLoad_Batch.Location = new System.Drawing.Point(641, 307);
            this.btnLoad_Batch.Name = "btnLoad_Batch";
            this.btnLoad_Batch.Size = new System.Drawing.Size(75, 23);
            this.btnLoad_Batch.TabIndex = 30;
            this.btnLoad_Batch.Text = "Load";
            this.btnLoad_Batch.UseVisualStyleBackColor = true;
            this.btnLoad_Batch.Click += new System.EventHandler(this.btnLoad_Batch_Click);
            // 
            // txtBatch_FilePath
            // 
            this.txtBatch_FilePath.Location = new System.Drawing.Point(106, 308);
            this.txtBatch_FilePath.Name = "txtBatch_FilePath";
            this.txtBatch_FilePath.Size = new System.Drawing.Size(509, 22);
            this.txtBatch_FilePath.TabIndex = 31;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 287);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(736, 89);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load script file .txt|.xlsx";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(-97, 29);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 32;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // radioButton_Write
            // 
            this.radioButton_Write.AutoSize = true;
            this.radioButton_Write.Location = new System.Drawing.Point(413, 16);
            this.radioButton_Write.Name = "radioButton_Write";
            this.radioButton_Write.Size = new System.Drawing.Size(54, 20);
            this.radioButton_Write.TabIndex = 28;
            this.radioButton_Write.Text = "Write";
            this.radioButton_Write.UseVisualStyleBackColor = true;
            // 
            // radioButton_Read
            // 
            this.radioButton_Read.AutoSize = true;
            this.radioButton_Read.Checked = true;
            this.radioButton_Read.Location = new System.Drawing.Point(255, 16);
            this.radioButton_Read.Name = "radioButton_Read";
            this.radioButton_Read.Size = new System.Drawing.Size(52, 20);
            this.radioButton_Read.TabIndex = 27;
            this.radioButton_Read.TabStop = true;
            this.radioButton_Read.Text = "Read";
            this.radioButton_Read.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(633, 16);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 24;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label54.Location = new System.Drawing.Point(3, 41);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(148, 16);
            this.label54.TabIndex = 2;
            this.label54.Text = "Hex data (max 128 counts)";
            // 
            // dataGridView4
            // 
            this.dataGridView4.AllowUserToResizeColumns = false;
            this.dataGridView4.AllowUserToResizeRows = false;
            this.dataGridView4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView4.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn33,
            this.dataGridViewTextBoxColumn34,
            this.dataGridViewTextBoxColumn35,
            this.dataGridViewTextBoxColumn36,
            this.dataGridViewTextBoxColumn37,
            this.dataGridViewTextBoxColumn38,
            this.dataGridViewTextBoxColumn39,
            this.dataGridViewTextBoxColumn40,
            this.dataGridViewTextBoxColumn41,
            this.dataGridViewTextBoxColumn42,
            this.dataGridViewTextBoxColumn43,
            this.dataGridViewTextBoxColumn44,
            this.dataGridViewTextBoxColumn45,
            this.dataGridViewTextBoxColumn46,
            this.dataGridViewTextBoxColumn47,
            this.dataGridViewTextBoxColumn48});
            this.dataGridView4.Location = new System.Drawing.Point(-1, 60);
            this.dataGridView4.MultiSelect = false;
            this.dataGridView4.Name = "dataGridView4";
            this.dataGridView4.RowHeadersVisible = false;
            this.dataGridView4.RowHeadersWidth = 50;
            this.dataGridView4.Size = new System.Drawing.Size(736, 186);
            this.dataGridView4.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn33
            // 
            this.dataGridViewTextBoxColumn33.Frozen = true;
            this.dataGridViewTextBoxColumn33.HeaderText = "0";
            this.dataGridViewTextBoxColumn33.Name = "dataGridViewTextBoxColumn33";
            this.dataGridViewTextBoxColumn33.Width = 46;
            // 
            // dataGridViewTextBoxColumn34
            // 
            this.dataGridViewTextBoxColumn34.Frozen = true;
            this.dataGridViewTextBoxColumn34.HeaderText = "1";
            this.dataGridViewTextBoxColumn34.Name = "dataGridViewTextBoxColumn34";
            this.dataGridViewTextBoxColumn34.Width = 46;
            // 
            // dataGridViewTextBoxColumn35
            // 
            this.dataGridViewTextBoxColumn35.Frozen = true;
            this.dataGridViewTextBoxColumn35.HeaderText = "2";
            this.dataGridViewTextBoxColumn35.Name = "dataGridViewTextBoxColumn35";
            this.dataGridViewTextBoxColumn35.Width = 46;
            // 
            // dataGridViewTextBoxColumn36
            // 
            this.dataGridViewTextBoxColumn36.Frozen = true;
            this.dataGridViewTextBoxColumn36.HeaderText = "3";
            this.dataGridViewTextBoxColumn36.Name = "dataGridViewTextBoxColumn36";
            this.dataGridViewTextBoxColumn36.Width = 46;
            // 
            // dataGridViewTextBoxColumn37
            // 
            this.dataGridViewTextBoxColumn37.Frozen = true;
            this.dataGridViewTextBoxColumn37.HeaderText = "4";
            this.dataGridViewTextBoxColumn37.Name = "dataGridViewTextBoxColumn37";
            this.dataGridViewTextBoxColumn37.Width = 46;
            // 
            // dataGridViewTextBoxColumn38
            // 
            this.dataGridViewTextBoxColumn38.Frozen = true;
            this.dataGridViewTextBoxColumn38.HeaderText = "5";
            this.dataGridViewTextBoxColumn38.Name = "dataGridViewTextBoxColumn38";
            this.dataGridViewTextBoxColumn38.Width = 46;
            // 
            // dataGridViewTextBoxColumn39
            // 
            this.dataGridViewTextBoxColumn39.Frozen = true;
            this.dataGridViewTextBoxColumn39.HeaderText = "6";
            this.dataGridViewTextBoxColumn39.Name = "dataGridViewTextBoxColumn39";
            this.dataGridViewTextBoxColumn39.Width = 46;
            // 
            // dataGridViewTextBoxColumn40
            // 
            this.dataGridViewTextBoxColumn40.Frozen = true;
            this.dataGridViewTextBoxColumn40.HeaderText = "7";
            this.dataGridViewTextBoxColumn40.Name = "dataGridViewTextBoxColumn40";
            this.dataGridViewTextBoxColumn40.Width = 46;
            // 
            // dataGridViewTextBoxColumn41
            // 
            this.dataGridViewTextBoxColumn41.Frozen = true;
            this.dataGridViewTextBoxColumn41.HeaderText = "8";
            this.dataGridViewTextBoxColumn41.Name = "dataGridViewTextBoxColumn41";
            this.dataGridViewTextBoxColumn41.Width = 46;
            // 
            // dataGridViewTextBoxColumn42
            // 
            this.dataGridViewTextBoxColumn42.Frozen = true;
            this.dataGridViewTextBoxColumn42.HeaderText = "9";
            this.dataGridViewTextBoxColumn42.Name = "dataGridViewTextBoxColumn42";
            this.dataGridViewTextBoxColumn42.Width = 46;
            // 
            // dataGridViewTextBoxColumn43
            // 
            this.dataGridViewTextBoxColumn43.Frozen = true;
            this.dataGridViewTextBoxColumn43.HeaderText = "A";
            this.dataGridViewTextBoxColumn43.Name = "dataGridViewTextBoxColumn43";
            this.dataGridViewTextBoxColumn43.Width = 46;
            // 
            // dataGridViewTextBoxColumn44
            // 
            this.dataGridViewTextBoxColumn44.Frozen = true;
            this.dataGridViewTextBoxColumn44.HeaderText = "B";
            this.dataGridViewTextBoxColumn44.Name = "dataGridViewTextBoxColumn44";
            this.dataGridViewTextBoxColumn44.Width = 46;
            // 
            // dataGridViewTextBoxColumn45
            // 
            this.dataGridViewTextBoxColumn45.Frozen = true;
            this.dataGridViewTextBoxColumn45.HeaderText = "C";
            this.dataGridViewTextBoxColumn45.Name = "dataGridViewTextBoxColumn45";
            this.dataGridViewTextBoxColumn45.Width = 46;
            // 
            // dataGridViewTextBoxColumn46
            // 
            this.dataGridViewTextBoxColumn46.Frozen = true;
            this.dataGridViewTextBoxColumn46.HeaderText = "D";
            this.dataGridViewTextBoxColumn46.Name = "dataGridViewTextBoxColumn46";
            this.dataGridViewTextBoxColumn46.Width = 46;
            // 
            // dataGridViewTextBoxColumn47
            // 
            this.dataGridViewTextBoxColumn47.Frozen = true;
            this.dataGridViewTextBoxColumn47.HeaderText = "E";
            this.dataGridViewTextBoxColumn47.Name = "dataGridViewTextBoxColumn47";
            this.dataGridViewTextBoxColumn47.Width = 46;
            // 
            // dataGridViewTextBoxColumn48
            // 
            this.dataGridViewTextBoxColumn48.Frozen = true;
            this.dataGridViewTextBoxColumn48.HeaderText = "F";
            this.dataGridViewTextBoxColumn48.Name = "dataGridViewTextBoxColumn48";
            this.dataGridViewTextBoxColumn48.Width = 46;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnVendorRead);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtVendorID);
            this.panel3.Controls.Add(this.btnReadWrite);
            this.panel3.Controls.Add(this.txtRegAdress);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.numericUpDownBytes);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 38);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(744, 69);
            this.panel3.TabIndex = 2;
            // 
            // btnVendorRead
            // 
            this.btnVendorRead.ForeColor = System.Drawing.Color.Black;
            this.btnVendorRead.Location = new System.Drawing.Point(203, 23);
            this.btnVendorRead.Name = "btnVendorRead";
            this.btnVendorRead.Size = new System.Drawing.Size(75, 23);
            this.btnVendorRead.TabIndex = 26;
            this.btnVendorRead.Text = "Read";
            this.btnVendorRead.UseVisualStyleBackColor = true;
            this.btnVendorRead.Click += new System.EventHandler(this.btnVendorRead_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 19);
            this.label1.TabIndex = 25;
            this.label1.Text = "Vendor_Id";
            // 
            // txtVendorID
            // 
            this.txtVendorID.Location = new System.Drawing.Point(82, 21);
            this.txtVendorID.Name = "txtVendorID";
            this.txtVendorID.ReadOnly = true;
            this.txtVendorID.Size = new System.Drawing.Size(100, 25);
            this.txtVendorID.TabIndex = 24;
            this.txtVendorID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnReadWrite
            // 
            this.btnReadWrite.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadWrite.ForeColor = System.Drawing.Color.Black;
            this.btnReadWrite.Location = new System.Drawing.Point(644, 8);
            this.btnReadWrite.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnReadWrite.Name = "btnReadWrite";
            this.btnReadWrite.Size = new System.Drawing.Size(83, 48);
            this.btnReadWrite.TabIndex = 13;
            this.btnReadWrite.Text = "Execute";
            this.btnReadWrite.UseVisualStyleBackColor = true;
            this.btnReadWrite.Click += new System.EventHandler(this.btnReadWrite_Click);
            // 
            // txtRegAdress
            // 
            this.txtRegAdress.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRegAdress.Location = new System.Drawing.Point(416, 21);
            this.txtRegAdress.Name = "txtRegAdress";
            this.txtRegAdress.Size = new System.Drawing.Size(62, 23);
            this.txtRegAdress.TabIndex = 12;
            this.txtRegAdress.Text = "0x0200";
            this.txtRegAdress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(496, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 19);
            this.label8.TabIndex = 8;
            this.label8.Text = "Counts";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(323, 24);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 19);
            this.label7.TabIndex = 8;
            this.label7.Text = "Reg Address";
            // 
            // numericUpDownBytes
            // 
            this.numericUpDownBytes.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownBytes.Location = new System.Drawing.Point(559, 22);
            this.numericUpDownBytes.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.numericUpDownBytes.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.numericUpDownBytes.Name = "numericUpDownBytes";
            this.numericUpDownBytes.Size = new System.Drawing.Size(59, 23);
            this.numericUpDownBytes.TabIndex = 7;
            this.numericUpDownBytes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(10, 5);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(744, 33);
            this.panel2.TabIndex = 1;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(259, 2);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(249, 31);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Macom 006409 Chip";
            // 
            // DR4_TX_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 522);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DR4_TX_Form";
            this.Text = "DR4_TX_Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DR4_TX_Form_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageI2C_Operation.ResumeLayout(false);
            this.tabPageI2C_Operation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView4)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBytes)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageI2C_Operation;
        private System.Windows.Forms.Button btnBrowse_Batch_File;
        private System.Windows.Forms.Button btnLoad_Batch;
        private System.Windows.Forms.TextBox txtBatch_FilePath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton radioButton_Write;
        private System.Windows.Forms.RadioButton radioButton_Read;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.DataGridView dataGridView4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn33;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn34;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn35;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn36;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn37;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn38;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn39;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn40;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn41;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn42;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn43;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn44;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn45;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn46;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn47;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn48;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtRegAdress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownBytes;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button btnReadWrite;
        private System.Windows.Forms.Button btnVendorRead;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVendorID;
    }
}