namespace CardReader2
{
    partial class Toast
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Toast));
            this.toastCard = new Bunifu.Framework.UI.BunifuCards();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toastIcon = new Bunifu.Framework.UI.BunifuImageButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.errorPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toastErrorDetailsLabel = new System.Windows.Forms.Label();
            this.toastErrorLabel = new System.Windows.Forms.Label();
            this.successPanel = new System.Windows.Forms.TableLayoutPanel();
            this.toastOkLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toastCard.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toastIcon)).BeginInit();
            this.panel1.SuspendLayout();
            this.errorPanel.SuspendLayout();
            this.successPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toastCard
            // 
            this.toastCard.BackColor = System.Drawing.Color.ForestGreen;
            this.toastCard.BorderRadius = 5;
            this.toastCard.BottomSahddow = true;
            this.toastCard.color = System.Drawing.Color.OliveDrab;
            this.toastCard.Controls.Add(this.panel2);
            this.toastCard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.toastCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toastCard.LeftSahddow = false;
            this.toastCard.Location = new System.Drawing.Point(0, 0);
            this.toastCard.Name = "toastCard";
            this.toastCard.RightSahddow = true;
            this.toastCard.ShadowDepth = 20;
            this.toastCard.Size = new System.Drawing.Size(445, 187);
            this.toastCard.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.37923F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.62077F));
            this.tableLayoutPanel1.Controls.Add(this.toastIcon, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(423, 159);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // toastIcon
            // 
            this.toastIcon.BackColor = System.Drawing.Color.Transparent;
            this.toastIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toastIcon.Location = new System.Drawing.Point(3, 3);
            this.toastIcon.Name = "toastIcon";
            this.toastIcon.Size = new System.Drawing.Size(97, 153);
            this.toastIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.toastIcon.TabIndex = 0;
            this.toastIcon.TabStop = false;
            this.toastIcon.Zoom = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.errorPanel);
            this.panel1.Controls.Add(this.successPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(106, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 153);
            this.panel1.TabIndex = 1;
            // 
            // errorPanel
            // 
            this.errorPanel.ColumnCount = 1;
            this.errorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.errorPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.errorPanel.Controls.Add(this.toastErrorDetailsLabel, 0, 1);
            this.errorPanel.Controls.Add(this.toastErrorLabel, 0, 0);
            this.errorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorPanel.Location = new System.Drawing.Point(0, 0);
            this.errorPanel.Name = "errorPanel";
            this.errorPanel.RowCount = 2;
            this.errorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.96685F));
            this.errorPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.03315F));
            this.errorPanel.Size = new System.Drawing.Size(314, 153);
            this.errorPanel.TabIndex = 2;
            // 
            // toastErrorDetailsLabel
            // 
            this.toastErrorDetailsLabel.AutoSize = true;
            this.toastErrorDetailsLabel.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toastErrorDetailsLabel.ForeColor = System.Drawing.Color.White;
            this.toastErrorDetailsLabel.Location = new System.Drawing.Point(3, 39);
            this.toastErrorDetailsLabel.Name = "toastErrorDetailsLabel";
            this.toastErrorDetailsLabel.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.toastErrorDetailsLabel.Size = new System.Drawing.Size(135, 21);
            this.toastErrorDetailsLabel.TabIndex = 6;
            this.toastErrorDetailsLabel.Text = "LOGIN FAILED";
            // 
            // toastErrorLabel
            // 
            this.toastErrorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toastErrorLabel.AutoSize = true;
            this.toastErrorLabel.Font = new System.Drawing.Font("Quicksand", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toastErrorLabel.ForeColor = System.Drawing.Color.White;
            this.toastErrorLabel.Location = new System.Drawing.Point(3, 2);
            this.toastErrorLabel.Name = "toastErrorLabel";
            this.toastErrorLabel.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.toastErrorLabel.Size = new System.Drawing.Size(187, 35);
            this.toastErrorLabel.TabIndex = 5;
            this.toastErrorLabel.Text = "LOGIN FAILED";
            // 
            // successPanel
            // 
            this.successPanel.ColumnCount = 1;
            this.successPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.successPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.successPanel.Controls.Add(this.toastOkLabel, 0, 0);
            this.successPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.successPanel.Location = new System.Drawing.Point(0, 0);
            this.successPanel.Name = "successPanel";
            this.successPanel.RowCount = 1;
            this.successPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.successPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 181F));
            this.successPanel.Size = new System.Drawing.Size(314, 153);
            this.successPanel.TabIndex = 0;
            // 
            // toastOkLabel
            // 
            this.toastOkLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.toastOkLabel.AutoSize = true;
            this.toastOkLabel.Font = new System.Drawing.Font("Quicksand", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toastOkLabel.ForeColor = System.Drawing.Color.White;
            this.toastOkLabel.Location = new System.Drawing.Point(3, 59);
            this.toastOkLabel.Name = "toastOkLabel";
            this.toastOkLabel.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.toastOkLabel.Size = new System.Drawing.Size(254, 35);
            this.toastOkLabel.TabIndex = 4;
            this.toastOkLabel.Text = "LOGIN SUCCESSFUL";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 5);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(443, 179);
            this.panel2.TabIndex = 3;
            // 
            // Toast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toastCard);
            this.Name = "Toast";
            this.Size = new System.Drawing.Size(445, 187);
            this.toastCard.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toastIcon)).EndInit();
            this.panel1.ResumeLayout(false);
            this.errorPanel.ResumeLayout(false);
            this.errorPanel.PerformLayout();
            this.successPanel.ResumeLayout(false);
            this.successPanel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCards toastCard;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        public Bunifu.Framework.UI.BunifuImageButton toastIcon;
        public System.Windows.Forms.TableLayoutPanel errorPanel;
        public System.Windows.Forms.Label toastErrorDetailsLabel;
        public System.Windows.Forms.Label toastErrorLabel;
        public System.Windows.Forms.TableLayoutPanel successPanel;
        public System.Windows.Forms.Label toastOkLabel;
        private System.Windows.Forms.Panel panel2;

    }
}
