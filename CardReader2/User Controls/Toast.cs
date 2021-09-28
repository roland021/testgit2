using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardReader2
{
    public partial class Toast : UserControl
    {
        public Toast()
        {
            InitializeComponent();
            WireAllControls(this);
        }

        #region Control Custom Properties

        public string LabelValue;
        public Image IconValue;


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
                toastIcon.Image = value;
            }
        }

        [Description("Header color of toast card"), Category("Custom Property")]
        public Color HeaderColor
        {
            get
            {
                return toastCard.color;
            }
            set
            {
                toastCard.color = value;
            }
        }

        [Description("Back color of toast card"), Category("Custom Property")]
        public Color BodyColor
        {
            get
            {
                return toastCard.BackColor;
            }
            set
            {
                toastCard.BackColor = value;
            }
        }

        #endregion

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


        private void button_Click(object sender, EventArgs e)
        {
            this.InvokeOnClick(this, EventArgs.Empty);
        }

    }
}
