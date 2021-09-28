using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CardReader2
{
    public partial class SideMenuButton : UserControl
    {
        public SideMenuButton()
        {
            InitializeComponent();
            WireAllControls(this);
        }

        #region Control Custom Properties
        
        public Image IconValue;
        public Color BannerValue;
        public Color BackgroundColorValue;
        public string LabelValue;

        [Description("Text of button"), Category("Custom Property")]
        public string ButtonLabel
        {
            get
            {
                return LabelValue;
            }
            set
            {
                LabelValue = value;
                Text.Text = value;
            }
        }

        [Description("Icon of button"), Category("Custom Property")]
        public Image Image
        {
            get
            {
                return IconValue;
            }
            set
            {
                IconValue = value;
                Icon.Image = value;
            }
        }

        [Description("Color of left handside banner"), Category("Custom Property")]
        public Color BannerColor
        {
            get
            {
                return BannerValue;
            }
            set
            {
                BannerValue = value;
                Banner.BackColor = value;
            }
        }

        [Description("Sets the Background Color"), Category("Custom Property")]
        public Color BackgroundColor
        {
            get
            {
                return BackgroundColorValue;
            }
            set
            {
                BackgroundColorValue = value;
                tableLayoutPanel1.BackColor = value;
            }
        }
        #endregion



        public Boolean IsButtonActive = false;
        [Description("Sets the Background Color"), Category("Custom Property")]
        public Boolean ActiveButton
        {
            get
            {
                return IsButtonActive;
            }
            set
            {
                IsButtonActive = value;
                if(IsButtonActive)
                {
                    tableLayoutPanel1.BackColor = Color.FromArgb(245, 171, 53);
                    BannerColor = Color.FromArgb(228, 27, 35);
                }
                if(!IsButtonActive)
                {
                    tableLayoutPanel1.BackColor = Color.Transparent;
                    BannerColor = Color.FromArgb(245, 171, 53);
                }
            }
        }


        private void WireAllControls(Control cont)
        {
            foreach (Control ctl in cont.Controls)
            {
                ctl.Click += button_Click;
                if (ctl.HasChildren)
                {
                    WireAllControls(ctl);
                }
            }
        }

        public void button_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, EventArgs.Empty);
        }
    }
}
