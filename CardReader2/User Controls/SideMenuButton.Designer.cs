namespace CardReader2
{
    partial class SideMenuButton
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Banner = new System.Windows.Forms.Panel();
            this.Text = new System.Windows.Forms.Label();
            this.Icon = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Banner, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Text, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Icon, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(263, 102);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Banner
            // 
            this.Banner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(171)))), ((int)(((byte)(53)))));
            this.Banner.Dock = System.Windows.Forms.DockStyle.Left;
            this.Banner.Location = new System.Drawing.Point(3, 3);
            this.Banner.Name = "Banner";
            this.Banner.Size = new System.Drawing.Size(12, 96);
            this.Banner.TabIndex = 0;
            // 
            // Text
            // 
            this.Text.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Text.AutoSize = true;
            this.Text.Font = new System.Drawing.Font("Raleway", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Text.ForeColor = System.Drawing.Color.White;
            this.Text.Location = new System.Drawing.Point(72, 36);
            this.Text.Name = "Text";
            this.Text.Size = new System.Drawing.Size(178, 29);
            this.Text.TabIndex = 2;
            this.Text.Text = "BUTTON TEXT";
            // 
            // Icon
            // 
            this.Icon.Location = new System.Drawing.Point(21, 3);
            this.Icon.Name = "Icon";
            this.Icon.Size = new System.Drawing.Size(45, 96);
            this.Icon.TabIndex = 1;
            this.Icon.TabStop = false;
            // 
            // SideMenuButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SideMenuButton";
            this.Size = new System.Drawing.Size(263, 102);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel Banner;
        private System.Windows.Forms.PictureBox Icon;
        private System.Windows.Forms.Label Text;
    }
}
