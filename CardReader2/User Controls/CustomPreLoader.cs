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
    public partial class CustomPreLoader : UserControl
    {
        private static List<Color> ControlColor = new List<Color>();
        public CustomPreLoader()
        {
            InitializeComponent();

            bunifuColorTransition1.Color1 = Color.FromArgb(245, 171, 53);
            bunifuColorTransition1.Color2 = Color.FromArgb(228, 27, 35);

            ControlColor.Add(Color.FromArgb(228, 27, 35));
            ControlColor.Add(Color.FromArgb(245, 171, 53));
            ControlColor.Add(Color.FromArgb(228, 27, 35));
            ControlColor.Add(Color.FromArgb(245, 171, 53));
            ControlColor.Add(Color.FromArgb(228, 27, 35));
            ControlColor.Add(Color.FromArgb(245, 171, 53));

            bunifuCircleProgressbar1.ProgressColor = ControlColor[cur_color];
        }

        #region Custom Properties 

        int cur_color = 0;
        int dir = 1;

        private static Color _DefaultColor1;
        public Color DefaultColor1
        {
            get { return _DefaultColor1; }
            set { _DefaultColor1 = value; }
        }

        private static Color _DefaultColor2;
        public Color DefaultColor2
        {
            get { return _DefaultColor2; }
            set { _DefaultColor2 = value; }
        }

        private static Color _1stColor;
        public Color Color1
        {
            get { return _1stColor; }
            set { _1stColor = value; }

        }

        private static Color _2ndColor;
        public Color Color2
        {
            get { return _2ndColor; }
            set { _2ndColor = value; }

        }

        private static Color _3rdColor;
        public Color Color3
        {
            get { return _3rdColor; }
            set { _3rdColor = value; }

        }

        private static Color _4thColor;
        public Color Color4
        {
            get { return _4thColor; }
            set { _4thColor = value; }
        }

        private static Color _5thColor;
        public Color Color5
        {
            get { return _5thColor; }
            set { _5thColor = value; }
        }

        private static Color _6thColor;
        public Color Color6
        {
            get { return _6thColor; }
            set { _6thColor = value; }
        }

        #endregion 

        #region Animation Methods 

        private void timer_stretch_Tick(object sender, EventArgs e)
        {
            if (bunifuCircleProgressbar1.Value == 90)
            {
                dir = -1;
                bunifuCircleProgressbar1.animationIterval = 4;
                SwitchColor();
            }
            else if (bunifuCircleProgressbar1.Value == 10)
            {
                dir = +1;
                bunifuCircleProgressbar1.animationIterval = 1;
                SwitchColor();
            }
            bunifuCircleProgressbar1.Value += dir;
        }

        void SwitchColor()
        {
            bunifuColorTransition1.Color1 = ControlColor[cur_color];

            if (cur_color < ControlColor.Count - 1)
            {
                cur_color++;
            }
            else
            {
                cur_color = 0;
            }
            bunifuColorTransition1.Color1 = ControlColor[cur_color];

            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (bunifuColorTransition1.ProgessValue < 100)
            {
                bunifuColorTransition1.ProgessValue++;
                bunifuCircleProgressbar1.ProgressColor = bunifuColorTransition1.Value;
                label1.ForeColor = bunifuColorTransition1.Value;
            }
            else
            {
                timer2.Stop();
                bunifuColorTransition1.Color1 = bunifuColorTransition1.Color2;
                bunifuColorTransition1.ProgessValue = 0;

            }
        }

        #endregion
    }
}
