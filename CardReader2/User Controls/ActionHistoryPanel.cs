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
    public partial class ActionHistoryPanel : UserControl
    {
        public ActionHistoryPanel()
        {
            InitializeComponent();
        }

        #region Custom Properties

        string _Action;
        string _Description;
        string _Time;
        string _Date;

        [Description("Action"), Category("Custom Property")]
        public string Action
        {
            get
            {
                return _Action;
            }
            set
            {
                _Action = value;
                actionLbl.Text = value;
            }
        }

        [Description("Description"), Category("Custom Property")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                descriptionLbl.Text = value;
            }
        }

        [Description("Time"), Category("Custom Property")]
        public string Time
        {
            get
            {
                return _Time;
            }
            set
            {
                _Time = value;
                timeLbl.Text = value;
            }
        }

        [Description("Date"), Category("Custom Property")]
        public string Date
        {
            get
            {
                return _Date;
            }
            set
            {
                _Date = value;
                dateLbl.Text = value;
            }
        }
        #endregion
    }
}
