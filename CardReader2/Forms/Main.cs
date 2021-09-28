using Acs.Readers;
using Acs.Readers.Pcsc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardReader2
{
    public partial class Main : Form
    {
        public LoginView login = new LoginView();
        public AdminView admin = new AdminView();

        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            await Task.Run(() => LoadForm());

            Action action = () => loadingPanel.Visible = false;
            loadingPanel.Invoke(action);            
        }

        public void LoadForm()
        {
            GlobalFunctions.ReadTerminalConfig();
            GlobalFunctions.SQLInitialize();
            GlobalFunctions.MainForm = this;
            GlobalVariables.TerminalID = GlobalFunctions.GetTerminalID();
            CardReader.ACR122Init();
            InitThread();

            BeginInvoke((MethodInvoker)delegate
            {
                MainPanel.Controls.Clear();
                MainPanel.Controls.Add(login);
                MainPanel.Controls.Add(admin);
                login.Dock = DockStyle.Fill;
                admin.Dock = DockStyle.Fill;
                admin.Visible = false;

                GlobalVariables.SystemView = SystemView.AdminLogin;
            });
        }

        private void toast_Click(object sender, EventArgs e)
        {
            toast.Visible = false;
        }

        #region ACS Card Reader 

        private PcscCardPolling cardPoller;
        private PcscReader userCardReader;
        private object userCardLock = new object();

        public void InitThread()
        {

            //Initialize card polling instance
            cardPoller = new PcscCardPolling();

            //Register to on card found event
            cardPoller.OnCardFound += new Acs.Readers.CardStatusChangeDelegate(cardPoller_OnCardFound);

            //Register to on card removed
            cardPoller.OnCardRemoved += new Acs.Readers.CardStatusChangeDelegate(cardPoller_OnCardRemoved);

            //Load all connected readers to the list of readers that will be checked by the poller
            cardPoller.fillReader();

            //Start card polling and check for a valid SAM co   nnected
            //IMPORTANT: You must stop the poller when the application is closing by calling the .stop() method of the object
            cardPoller.start();

        }

        private void cardPoller_OnCardRemoved(object sender, CardPollingEventArg e)
        {
            if (userCardReader != null && userCardReader.readerName == e.reader)
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    //On Card Removed
                    switch (GlobalVariables.SystemView)
                    {
                        case (SystemView.NewAccount):
                            admin.NewAccount_CardRemoved();
                            break;
                        case (SystemView.DisplayAccount):
                            admin.DisplayAccount_RemoveCard();
                            break;
                        case (SystemView.UpdateAccount):
                            admin.UpdateAccount_CardRemoved();
                            break;
                        case (SystemView.ClearData):
                            admin.SearchRecord_CardRemoved();
                            break;
                        case (SystemView.MemberMain):
                            admin.MemberMain_Clear();
                            break;
                        case (SystemView.MemberUpdate):
                            admin.UpdateMember_CardRemoved();
                            break;

                        
                    }
                });
                return;
            }
        }

        private void cardPoller_OnCardFound(object sender, CardPollingEventArg e)
        {
            try
            {
                handleUserCardFound(e.reader);
            }
            catch (Exception exc) { MessageBox.Show(exc.Message); }
        }

        public void handleUserCardFound(string readerName)
        {
            lock (userCardLock)
            {
                userCardReader = new PcscReader(readerName);
                userCardReader.connect();

                //On Card Found
                switch(GlobalVariables.SystemView)
                {
                    case (SystemView.NewAccount):
                        admin.NewAccount_CardFound();
                        break;
                    case (SystemView.DisplayAccount):
                        admin.DisplayAccount_CardFound(CardReader.ReadingOfCards());
                        break;
                    case (SystemView.UpdateAccount):
                        admin.UpdateAccount_CardFound();
                        break;
                    case (SystemView.ClearData):
                        admin.SearchRecord_CardFound(CardReader.ReadingOfCards());
                        break;
                    case (SystemView.MemberNew):
                        admin.NewMember_CardFound();
                        break;
                    case (SystemView.MemberMain):                        
                        admin.Member_Main_SetData(GlobalFunctions.Member_Search_CardSerial(CardReader.ReadingOfCards()));
                        break;
                    case (SystemView.MemberUpdate):
                        admin.UpdateMember_CardFound();
                        break;

                }
            }
        }


        #endregion
        string awaitme = "";
        bool readingmsr = false;
        private void Main_KeyPress(object sender, KeyPressEventArgs e)      
        {
            if (e.KeyChar == ';')
            {
                readingmsr = true;
                awaitme += e.KeyChar;
            }

            if (readingmsr)
            {
                awaitme += e.KeyChar;                
                e.Handled = true;

                if (awaitme.Contains("?"))
                {
                    awaitme = awaitme.Replace(";", "");
                    awaitme = awaitme.Replace("?", "");
                    awaitme = awaitme.Replace("\r", "");

                    if(GlobalVariables.SystemView == SystemView.MemberNew)
                    {
                        admin.NewMember_CardSwipe(awaitme);                        
                    }

                    if(GlobalVariables.SystemView == SystemView.MemberMain)
                    {
                        admin.Member_Main_SetData(GlobalFunctions.Member_Search_MagData(awaitme));
                    }

                    if(GlobalVariables.SystemView == SystemView.MemberUpdate)
                    {
                        admin.UpdateMember_CardSwipe(awaitme);
                    }

                    awaitme = "";
                    e.Handled = false;
                    readingmsr = false;
                }
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
