namespace SNOEC_GUI
{
    partial class ProductSelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductSelectForm));
            this.btnSelect_QSFP28 = new System.Windows.Forms.Button();
            this.btnSelect_QSFP_DD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelect_QSFP28
            // 
            this.btnSelect_QSFP28.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect_QSFP28.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.btnSelect_QSFP28.Location = new System.Drawing.Point(95, 30);
            this.btnSelect_QSFP28.Name = "btnSelect_QSFP28";
            this.btnSelect_QSFP28.Size = new System.Drawing.Size(111, 68);
            this.btnSelect_QSFP28.TabIndex = 1;
            this.btnSelect_QSFP28.Text = "QSFP28";
            this.btnSelect_QSFP28.UseVisualStyleBackColor = false;
            this.btnSelect_QSFP28.Click += new System.EventHandler(this.btnSelect_QSFP28_Click);
            // 
            // btnSelect_QSFP_DD
            // 
            this.btnSelect_QSFP_DD.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect_QSFP_DD.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.btnSelect_QSFP_DD.Location = new System.Drawing.Point(292, 30);
            this.btnSelect_QSFP_DD.Name = "btnSelect_QSFP_DD";
            this.btnSelect_QSFP_DD.Size = new System.Drawing.Size(111, 68);
            this.btnSelect_QSFP_DD.TabIndex = 1;
            this.btnSelect_QSFP_DD.Text = "QSFP-DD";
            this.btnSelect_QSFP_DD.UseVisualStyleBackColor = false;
            this.btnSelect_QSFP_DD.Click += new System.EventHandler(this.btnSelect_QSFP_DD_Click);
            // 
            // ProductSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 134);
            this.Controls.Add(this.btnSelect_QSFP_DD);
            this.Controls.Add(this.btnSelect_QSFP28);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "ProductSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProductSelect";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSelect_QSFP28;
        private System.Windows.Forms.Button btnSelect_QSFP_DD;
    }
}