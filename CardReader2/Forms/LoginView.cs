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
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (GlobalFunctions.checkAdminLogin(Login_Username.Text, Login_Password.Text))
            {                
                GlobalFunctions.MainForm.login.Visible = false;
                GlobalFunctions.MainForm.admin.Visible = true;

                GlobalFunctions.MainForm.admin.UserLabel.Text = GlobalVariables.AdminName;
                if(GlobalVariables.AdminType == 1)
                {
                    GlobalFunctions.MainForm.admin.MenuAccountsButton.Visible = true;
                    GlobalFunctions.MainForm.admin.MenuRecordsButton.Visible = true;
                    GlobalFunctions.MainForm.admin.MenuClearButton.Visible = true;
                    GlobalFunctions.MainForm.admin.MenuSettingsButton.Visible = true;
                }
                else
                {
                    GlobalFunctions.MainForm.admin.MenuAccountsButton.Visible = false;
                    GlobalFunctions.MainForm.admin.MenuRecordsButton.Visible = false;
                }
            }
        }

        private void Login_Password_OnValueChanged(object sender, EventArgs e)
        {
            Login_Password.isPassword = true;
        }
    }
}
