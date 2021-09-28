using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Drawing.Drawing2D;

namespace CardReader2
{
    public partial class AdminView : UserControl
    {
        public AdminView()
        {
            InitializeComponent();
        }

        private void AdminView_Load(object sender, EventArgs e)
        {
            tableLayoutPanel17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            GlobalVariables.ParkerTypeList = GlobalFunctions.GetParkerType();
            GlobalVariables.AreaList = GlobalFunctions.GetArea();
            GlobalVariables.TerminalList = GlobalFunctions.GetTerminals();

            accountNew_ParkerType.DataSource = GlobalVariables.ParkerTypeList;
            accountNew_ParkerType.DisplayMember = "Member_Type";
            accountNew_ParkerType.ValueMember = "Member_ID";

            accountNew_AreaList.DataSource = GlobalVariables.AreaList;
            accountNew_AreaList.DisplayMember = "Area_Name";
            accountNew_AreaList.ValueMember = "Area_ID";

            accountUpdate_ParkerType.DataSource = GlobalVariables.ParkerTypeList;
            accountUpdate_ParkerType.DisplayMember = "Member_Type";
            accountUpdate_ParkerType.ValueMember = "Member_ID";

            accountUpdate_AreaList.DataSource = GlobalVariables.AreaList;
            accountUpdate_AreaList.DisplayMember = "Area_Name";
            accountUpdate_AreaList.ValueMember = "Area_ID";

            accountDisplay_AreaList.DataSource = GlobalVariables.AreaList;
            accountDisplay_AreaList.DisplayMember = "Area_Name";
            accountDisplay_AreaList.ValueMember = "Area_ID";

        }

        private void sideMenuButton1_Click(object sender, EventArgs e)
        {
            SideMenuButton button = (SideMenuButton)sender;

            switch (button.Name)
            {
                case ("MenuSettingsButton"):
                    MenuSettingsButton.ActiveButton = true;
                    MenuAccountsButton.ActiveButton = false;
                    MenuRecordsButton.ActiveButton = false;
                    MenuClearButton.ActiveButton = false;
                    MenuMemberButton.ActiveButton = false;

                    MemberPanel.Visible = false;
                    SettingsPanel.Visible = true;
                    AccountsPanel.Visible = false;
                    RecordsPanel.Visible = false;
                    ClearPanel.Visible = false;

                    LoadSettings();
                    break;
                case ("MenuAccountsButton"):
                    MenuSettingsButton.ActiveButton = false;
                    MenuAccountsButton.ActiveButton = true;
                    MenuRecordsButton.ActiveButton = false;
                    MenuClearButton.ActiveButton = false;
                    MenuMemberButton.ActiveButton = false;

                    MemberPanel.Visible = false;
                    SettingsPanel.Visible = false;
                    AccountsPanel.Visible = true;
                    RecordsPanel.Visible = false;
                    ClearPanel.Visible = false;

                    clearAccountDisplayData();
                    GlobalVariables.SystemView = SystemView.DisplayAccount;
                    break;
                case ("MenuRecordsButton"):
                    MenuSettingsButton.ActiveButton = false;
                    MenuAccountsButton.ActiveButton = false;
                    MenuRecordsButton.ActiveButton = true;
                    MenuClearButton.ActiveButton = false;
                    MenuMemberButton.ActiveButton = false;

                    MemberPanel.Visible = false;
                    SettingsPanel.Visible = false;
                    AccountsPanel.Visible = false;
                    RecordsPanel.Visible = true;
                    ClearPanel.Visible = false;

                    refreshDataTable();
                    break;
                case ("MenuMemberButton"):
                    MenuSettingsButton.ActiveButton = false;
                    MenuAccountsButton.ActiveButton = false;
                    MenuMemberButton.ActiveButton = true;
                    MenuRecordsButton.ActiveButton = false;
                    MenuClearButton.ActiveButton = false;
                    MenuMemberButton.ActiveButton = false;

                    MemberPanel.Visible = false;
                    MemberPanel.Visible = true;
                    SettingsPanel.Visible = false;
                    AccountsPanel.Visible = false;
                    RecordsPanel.Visible = false;
                    ClearPanel.Visible = false;

                    GlobalVariables.SystemView = SystemView.MemberMain;
                    ShowMember();
                    break;
                case ("MenuClearButton"):
                    MenuSettingsButton.ActiveButton = false;
                    MenuAccountsButton.ActiveButton = false;
                    MenuRecordsButton.ActiveButton = false;
                    MenuClearButton.ActiveButton = true;

                    ClearRecords();
                    GlobalVariables.SystemView = SystemView.ClearData;
                    SettingsPanel.Visible = false;
                    AccountsPanel.Visible = false;
                    RecordsPanel.Visible = false;
                    ClearPanel.Visible = true;
                    break;
            }
        }

        #region Settings

        public void LoadSettings()
        {
            Settings_TermName.Text = GlobalVariables.TerminalName;
            Settings_ServerAddress.Text = GlobalVariables.Server;
            Settings_DatabaseName.Text = GlobalVariables.ServerDatabaseName;
            Settings_Port.Text = GlobalVariables.ServerPort;
            Settings_Username.Text = GlobalVariables.ServerUsername;
            Settings_Password.Text = GlobalVariables.ServerPassword;
            Settings_FTPLoc.Text = GlobalVariables.FTP_IP;
            Settings_GrandWingLoc.Text = GlobalVariables.GrandWing_ImageLoc;
        }

        public bool CheckChanges()
        {
            List<bool> checkChanges = new List<bool>();

            checkChanges.Add((Settings_TermName.Text == GlobalVariables.TerminalName) ? true : false);
            checkChanges.Add((Settings_ServerAddress.Text == GlobalVariables.Server) ? true : false);
            checkChanges.Add((Settings_Port.Text == GlobalVariables.ServerPort) ? true : false);
            checkChanges.Add((Settings_DatabaseName.Text == GlobalVariables.ServerDatabaseName) ? true : false);
            checkChanges.Add((Settings_Username.Text == GlobalVariables.ServerUsername) ? true : false);
            checkChanges.Add((Settings_Password.Text == GlobalVariables.ServerPassword) ? true : false);
            checkChanges.Add((Settings_FTPLoc.Text == GlobalVariables.FTP_IP) ? true : false);
            checkChanges.Add((Settings_GrandWingLoc.Text == GlobalVariables.GrandWing_ImageLoc) ? true : false);

            foreach (bool item in checkChanges)
            {
                if (!item)
                {
                    return false;
                }
            }

            return true;
        }

        public void updateConfiguration()
        {
            //Terminal Config 
            string[] terminal_lines = { Settings_TermName.Text,
                                 Settings_ServerAddress.Text,
                                 Settings_Port.Text,
                                 Settings_DatabaseName.Text,
                                 Settings_Username.Text,
                                 Settings_Password.Text,
                                 Settings_FTPLoc.Text,
                                 Settings_GrandWingLoc.Text
                             };

            System.IO.File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\CONFIG\TerminalConfig.dat", terminal_lines);
        }

        private void Settings_SaveButton_Click(object sender, EventArgs e)
        {
            updateConfiguration();
            Application.Restart();
            Environment.Exit(0);
        }

        private void Settings_TermName_TextChanged(object sender, EventArgs e)
        {
            if (!CheckChanges())
            {
                Settings_SaveButton.Enabled = true;
                Settings_SaveButton.BackColor = Color.White;
            }
            else
            {
                Settings_SaveButton.Enabled = false;
                Settings_SaveButton.BackColor = Color.Gray;
            }
        }

        private void Menu_InitializeButton_Click(object sender, EventArgs e)
        {
            SideMenuButton button = (SideMenuButton)sender;
        }


        private void Settings_Password_TextChanged(object sender, EventArgs e)
        {
            if (!CheckChanges())
            {
                Settings_SaveButton.Enabled = true;
                Settings_SaveButton.BackColor = Color.White;
            }
            else
            {
                Settings_SaveButton.Enabled = false;
                Settings_SaveButton.BackColor = Color.Gray;
            }
        }

        private void Settings_SaveButton_Click_1(object sender, EventArgs e)
        {
            updateConfiguration();
            Application.Restart();
            Environment.Exit(0);
        }
        #endregion

        #region Records

        string queryBuilder = "";

        public async void refreshDataTable()
        {
            DataTable recordsTableData = new DataTable();
            UIControl(false);

            loadingPanel.Visible = true;


            await Task.Run(() => recordsTableData = GlobalFunctions.GetAccountRecords());

            // set Area Overview deprecated due to heavy query overload
            //setAreaOverview();
            RecordsDataGrid.DataSource = recordsTableData;
            GlobalVariables.RecordsTable = recordsTableData;
            setTableHeaders();

            Action action = () => loadingPanel.Visible = false;
            loadingPanel.Invoke(action);

            UIControl(true);
        }

        public void setTableHeaders()
        {


            RecordsDataGrid.Columns["cardholder_id"].HeaderText = "Cardholder ID";
            RecordsDataGrid.Columns["cardholder_id"].Visible = false;
            RecordsDataGrid.Columns["card_status"].HeaderText = "Cardholder ID";
            RecordsDataGrid.Columns["card_status"].Visible = false;

            RecordsDataGrid.Columns["cardserial"].HeaderText = "Card Serial";
            RecordsDataGrid.Columns["name"].HeaderText = "Name";
            RecordsDataGrid.Columns["member_type"].HeaderText = "Member Type";
            RecordsDataGrid.Columns["platenum"].HeaderText = "Plate Number";


        }


        public void UIControl(bool set)
        {
            if (set)
            {
                MenuRecordsButton.Enabled = true;
                MenuAccountsButton.Enabled = true;
                MenuClearButton.Enabled = true;
                MenuSettingsButton.Enabled = true;
            }
            else
            {
                MenuRecordsButton.Enabled = false;
                MenuAccountsButton.Enabled = false;
                MenuClearButton.Enabled = false;
                MenuSettingsButton.Enabled = false;
            }
        }

        private void settingsRecordButton_Click(object sender, EventArgs e)
        {
            if (searchParametersPanel.Visible)
                searchParametersPanel.Visible = false;
            else
                searchParametersPanel.Visible = true;
        }

        private void searchRecordsButton_Click(object sender, EventArgs e)
        {
            searchDataTable(recordsSearchBox.Text);
        }

        private void parkIDCheckBox_Click(object sender, EventArgs e)
        {
            searchDataTable(recordsSearchBox.Text);
        }

        private void recordsSearchBox_TextChanged(object sender, EventArgs e)
        {
            searchDataTable(recordsSearchBox.Text);
        }

        public void searchDataTable(string searchstring)
        {
            DataTable recordsDataTable = new DataTable();
            RecordsDataGrid.ClearSelection();

            try
            {
                checkSearchBoxes();
                DataTable tb = GlobalVariables.RecordsTable.Copy();
                if (searchstring.Length >= 1)
                {
                    DataRow[] filteredRows = tb.Select(queryBuilder);

                    if (filteredRows.Length != 0)
                    {
                        tb = filteredRows.CopyToDataTable();
                        RecordsDataGrid.DataSource = tb;
                        setTableHeaders();
                    }
                    else
                    {
                        tb.Rows.Clear();
                        RecordsDataGrid.DataSource = tb;
                        setTableHeaders();
                    }
                }
                else if (searchstring.Length == 0)
                {
                    RecordsDataGrid.DataSource = GlobalVariables.RecordsTable;
                    setTableHeaders();
                }


            }
            catch (Exception exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Table Error", BottomText = exc.Message });
            }
        }



        public void checkSearchBoxes()
        {
            queryBuilder = "";
            if (records_NameCheckbox.Checked)
            {
                queryBuilder += "name LIKE '%" + recordsSearchBox.Text + "%' OR ";
            }
            if (records_CardSerialCheckbox.Checked)
            {
                queryBuilder += "cardserial LIKE '%" + recordsSearchBox.Text + "%' OR ";
            }
            if (records_MemberTypeCheckbox.Checked)
            {
                queryBuilder += "member_type LIKE '%" + recordsSearchBox.Text + "%' OR ";
            }
            if (records_PlateNumberCheckbox.Checked)
            {
                queryBuilder += "platenum LIKE '%" + recordsSearchBox.Text + "%' OR ";
            }

            if (queryBuilder != "")
            {
                queryBuilder = queryBuilder.Remove(queryBuilder.Length - 3);
            }


        }

        private void RecordsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex <= RecordsDataGrid.RowCount)
            {
                MenuRecordsButton.ActiveButton = false;
                MenuAccountsButton.ActiveButton = true;

                RecordsPanel.Visible = false;
                AccountsPanel.Visible = true;
                GlobalVariables.SystemView = SystemView.DisplayAccount;
                clearAccountDisplayData();

                //If Account has Card Serial
                if (RecordsDataGrid.Rows[e.RowIndex].Cells[1].FormattedValue.ToString() != "")
                {
                    DisplayAccount_CardFound(RecordsDataGrid.Rows[e.RowIndex].Cells[1].FormattedValue.ToString());
                }
                //If Account has NO Card Serial
                else
                {
                    int userid = Int32.TryParse(RecordsDataGrid.Rows[e.RowIndex].Cells[0].FormattedValue.ToString(), out userid) ? userid : 0;
                    int cardholderid = Int32.TryParse(RecordsDataGrid.Rows[e.RowIndex].Cells[1].FormattedValue.ToString(), out cardholderid) ? cardholderid : 0;
                    DisplayAccount_ByID(cardholderid);
                }
            }
        }

        #endregion

        #region Accounts

        //ACS Card
        public void NewAccount_CardFound()
        {
            CardReader.ACR122Pooling();
            string CardSerial = CardReader.ReadingOfCards();
            accountNew_CardSerial.Text = CardSerial;

            accountNew_CardSerialPrompt.Visible = true;
            if (GlobalFunctions.validateCardSerial(CardSerial))
                accountNew_CardSerialPrompt.Image = global::CardReader2.Properties.Resources.Ok_Icon1;
            else
                accountNew_CardSerialPrompt.Image = global::CardReader2.Properties.Resources.Error_Icon1;


        }

        public void NewAccount_CardRemoved()
        {
            accountNew_CardSerial.Text = "";
            accountNew_CardSerialPrompt.Visible = false;
        }

        public void UpdateAccount_CardFound()
        {
            CardReader.ACR122Pooling();
            string CardSerial = CardReader.ReadingOfCards();
            accountUpdate_CardSerial.Text = CardSerial;

            accountUpdate_CardSerialPrompt.Visible = true;
            if (GlobalFunctions.validateCardSerial(CardSerial))
                accountUpdate_CardSerialPrompt.Image = global::CardReader2.Properties.Resources.Ok_Icon1;
            else
                accountUpdate_CardSerialPrompt.Image = global::CardReader2.Properties.Resources.Error_Icon1;
        }

        public void UpdateAccount_CardRemoved()
        {
            accountUpdate_CardSerial.Text = GlobalVariables.CardData.CardSerial;
            accountUpdate_CardSerialPrompt.Visible = false;
        }

        public void DisplayAccount_CardFound(string cardSerial)
        {
            string CardSerial = cardSerial;
            int CardHolderID = 0;
            int UserID = 0;

            if (Int32.TryParse(GlobalFunctions.SearchCardHolderID(CardSerial), out CardHolderID))
            {
                if (CardHolderID != 0)
                {
                    GlobalVariables.CardData = GlobalFunctions.SearchAccount(CardSerial);
                    SetAccount();
                }

            }
            else
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Error", BottomText = "Account not found" });
            }

        }

        public void DisplayAccount_RemoveCard()
        {
            clearAccountDisplayData();
        }

        #region New Account




        private void account_CreateNewButton_Click(object sender, EventArgs e)
        {
            GlobalVariables.SystemView = SystemView.NewAccount;
            clearAccountNewData();

            account_DisplayPanel.Visible = false;
            account_NewPanel.Visible = true;
        }

        private void accountNew_Cancel_Click(object sender, EventArgs e)
        {
            GlobalVariables.SystemView = SystemView.DisplayAccount;
            clearAccountNewData();
            account_DisplayPanel.Visible = true;
            account_NewPanel.Visible = false;
        }

        public List<Area> SetAreaList()
        {
            List<Area> selectedArea = new List<Area>();

            foreach (Area checkedArea in accountNew_AreaList.CheckedItems)
            {
                selectedArea.Add(checkedArea);
            }

            return selectedArea;
        }

        private void accountNew_SaveButton_Click(object sender, EventArgs e)
        {
            CardHolder newCardHolder = new CardHolder();

            if (ValidateNewInputs())
            {
                newCardHolder.Firstname = accountNew_FirstName.Text;
                newCardHolder.Lastname = accountNew_LastName.Text;
                newCardHolder.CardSerial = accountNew_CardSerial.Text;

                newCardHolder.parkingData.Platenum = accountNew_PlateNum.Text;
                newCardHolder.parkingData.ParkerType_ID = (int)accountNew_ParkerType.SelectedValue;
                newCardHolder.parkingData.AreaList = SetAreaList();

                GlobalFunctions.CreateAccount(newCardHolder);
                clearAccountNewData();
            }
        }

        public class CheckList
        {
            public bool validate = false;
            public string error = "";
        }

        public bool ValidateNewInputs()
        {
            List<CheckList> checklist = new List<CheckList>();

            checklist.Add(new CheckList { validate = accountNew_FirstName.Text != "" ? true : false, error = "Missing First Name" });
            checklist.Add(new CheckList { validate = accountNew_LastName.Text != "" ? true : false, error = "Missing Last Name" });
            checklist.Add(new CheckList { validate = accountNew_CardSerial.Text != "" ? true : false, error = "Card Serial Missing" });
            checklist.Add(new CheckList { validate = GlobalFunctions.validateCardSerial(accountNew_CardSerial.Text) ? true : false, error = "Card Serial is already being used" });

            checklist.Add(new CheckList { validate = accountNew_PlateNum.Text != "" ? true : false, error = "Missing Plate Number" });
            checklist.Add(new CheckList { validate = accountNew_ParkerType.SelectedItem != null ? true : false, error = "Select a Parker Type" });

            bool checkarea = false;
            foreach (Area area in SetAreaList())
            {
                checkarea = true;
            }
            checklist.Add(new CheckList { validate = checkarea ? true : false, error = "Select at least one area" });

            foreach (CheckList item in checklist)
            {
                if (!item.validate)
                {
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Account Error", BottomText = item.error });
                    return false;
                }
            }

            return true;
        }

        public void clearAccountNewData()
        {
            accountNew_FirstName.Text = "";
            accountNew_LastName.Text = "";
            accountNew_CardSerial.Text = "";
            accountNew_CardSerialPrompt.Visible = false;
            accountNew_PlateNum.Text = "";
            accountNew_ParkerType.SelectedIndex = -1;


            while (accountNew_AreaList.CheckedIndices.Count > 0)
                accountNew_AreaList.SetItemChecked(accountNew_AreaList.CheckedIndices[0], false);

        }
        #endregion

        #region Display Account

        public void DisplayAccount_ByID(int CardHolder_ID)
        {

            if (CardHolder_ID != 0)
            {
                GlobalVariables.CardData = GlobalFunctions.SearchAccount_ByID(CardHolder_ID);
                SetAccount();
            }
            if (CardHolder_ID == 0)
            {
                //Account Not found
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Account Error", BottomText = "Account not found in Cardholder Records" });
            }
        }

        public void SetAccount()
        {
            account_UpdateButton.BackColor = Color.White;
            account_UpdateButton.Enabled = true;
            account_DeactivateButton.BackColor = Color.White;
            account_DeactivateButton.Enabled = true;

            accountDisplay_Name.Text = GlobalVariables.CardData.Firstname + " " + GlobalVariables.CardData.Lastname;
            accountDisplay_PlateNumber.Text = GlobalVariables.CardData.parkingData.Platenum;
            accountDisplay_MemberType.Text = GlobalVariables.CardData.parkingData.ParkerType_Name;

            if (!GlobalVariables.CardData.parkingData.CardHolderStatus)
            {
                accountDisplay_Disabled.Visible = true;
            }
            else
            {
                accountDisplay_Disabled.Visible = false;
            }

            if (GlobalVariables.CardData.parkingData.CardHolder_ID != 0)
                ReadAreaList();
            else
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Caution, HeaderText = "Account Error", BottomText = "This account has missing information regarding parking data, it is recommended that you update this account with complete parking information" });



            if (!GlobalVariables.CardData.parkingData.CardHolderStatus)
            {
                DisabledAccount();
            }
        }

        public void DisabledAccount()
        {
            GlobalFunctions.CallToast(new ToastData { type = ToastType.Caution, HeaderText = "Account Disabled", BottomText = "No changes are allowed" });
            account_UpdateButton.BackColor = Color.Gray;
            account_UpdateButton.Enabled = false;
            account_DeactivateButton.Text = "Reactivate";
        }

        public void ReadAreaList()
        {
            foreach (Area selected in GlobalVariables.CardData.parkingData.AreaList)
            {
                accountDisplay_AreaList.SetItemChecked(accountDisplay_AreaList.Items.IndexOf(selected), true);
            }
        }

        public void clearAccountDisplayData()
        {
            account_NewPanel.Visible = false;
            account_UpdatePanel.Visible = false;
            account_DisplayPanel.Visible = true;
            userAccountPanel.Visible = true;
            userHistoryPanel.Visible = false;

            accountDisplay_Name.Text = "Account Name";
            accountDisplay_PlateNumber.Text = "Plate Number";
            accountDisplay_DateCreated.Text = "Date Created";
            accountDisplay_Username.Text = "Username";
            accountDisplay_MemberType.Text = "Member Type";
            account_DeactivateButton.Text = "Deactivate";

            accountDisplay_UserType_Parker.Visible = false;
            accountDisplay_UserType_Cashier.Visible = false;
            accountDisplay_UserType_Admin.Visible = false;
            accountDisplay_UserTypeAudit.Visible = false;
            accountDisplay_UserTypeTranspoManager.Visible = false;
            accountDisplay_UserInfoPanel.Visible = false;
            accountDisplay_CautionIcon.Visible = false;
            accountDisplay_Disabled.Visible = false;

            account_UpdateButton.BackColor = Color.Gray;
            account_UpdateButton.Enabled = false;
            account_DeactivateButton.BackColor = Color.Gray;
            account_DeactivateButton.Enabled = false;

            while (accountDisplay_AreaList.CheckedIndices.Count > 0)
                accountDisplay_AreaList.SetItemChecked(accountDisplay_AreaList.CheckedIndices[0], false);

            accountDisplay_AccountTab.BackColor = Color.LightGray;
        }



        #endregion

        #region Update Account

        private void account_UpdateButton_Click(object sender, EventArgs e)
        {

            account_DisplayPanel.Visible = false;
            account_UpdatePanel.Visible = true;
            clearAccountUpdate();
            setAccountUpdate();

            GlobalVariables.SystemView = SystemView.UpdateAccount;
        }

        private void accountUpdate_Cancel_Click(object sender, EventArgs e)
        {
            GlobalVariables.SystemView = SystemView.DisplayAccount;
            account_DisplayPanel.Visible = true;
            account_UpdatePanel.Visible = false;
        }

        public void clearAccountUpdate()
        {
            accountUpdate_FirstName.Text = "";
            accountUpdate_LastName.Text = "";
            accountUpdate_CardSerial.Text = "";
            accountUpdate_CardSerialPrompt.Visible = false;
            accountUpdate_PlateNumber.Text = "";
            accountUpdate_ParkerType.SelectedIndex = -1;


            while (accountUpdate_AreaList.CheckedIndices.Count > 0)
                accountUpdate_AreaList.SetItemChecked(accountUpdate_AreaList.CheckedIndices[0], false);
        }

        public void setAccountUpdate()
        {
            accountUpdate_Save.BackColor = Color.Gray;
            accountUpdate_Save.Enabled = false;


            accountUpdate_FirstName.Text = GlobalVariables.CardData.Firstname;
            accountUpdate_LastName.Text = GlobalVariables.CardData.Lastname;
            accountUpdate_CardSerial.Text = GlobalVariables.CardData.CardSerial;


            accountUpdate_PlateNumber.Text = GlobalVariables.CardData.parkingData.Platenum;

            if (GlobalVariables.CardData.parkingData.ParkerType_ID != 0)
                accountUpdate_ParkerType.SelectedValue = GlobalVariables.CardData.parkingData.ParkerType_ID;

            if (GlobalVariables.CardData.parkingData.AreaList != null)
                ReadAreaList_Update();

        }

        public List<Area> SetArea_Update()
        {
            List<Area> selectedArea = new List<Area>();

            foreach (Area checkedArea in accountUpdate_AreaList.CheckedItems)
            {
                selectedArea.Add(checkedArea);
            }

            return selectedArea;
        }

        public void ReadAreaList_Update()
        {
            foreach (Area selected in GlobalVariables.CardData.parkingData.AreaList)
            {
                accountUpdate_AreaList.SetItemChecked(accountUpdate_AreaList.Items.IndexOf(selected), true);
            }
        }

        //Detect Changes
        private void accountUpdate_PlateNumber_OnValueChanged(object sender, EventArgs e)
        {
            if (GlobalVariables.CardData != null && GlobalVariables.SystemView == SystemView.UpdateAccount)
            {

                if (!checkUpdateChanges())
                {
                    accountUpdate_Save.Enabled = true;
                    accountUpdate_Save.BackColor = Color.White;
                }
                else
                {
                    accountUpdate_Save.Enabled = false;
                    accountUpdate_Save.BackColor = Color.Gray;
                }
            }
        }

        private void accountUpdate_ParkerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalVariables.CardData != null && GlobalVariables.SystemView == SystemView.UpdateAccount)
            {

                if (!checkUpdateChanges())
                {
                    accountUpdate_Save.Enabled = true;
                    accountUpdate_Save.BackColor = Color.White;
                }
                else
                {
                    accountUpdate_Save.Enabled = false;
                    accountUpdate_Save.BackColor = Color.Gray;
                }
            }
        }

        private void accountUpdate_AreaList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (GlobalVariables.CardData != null && GlobalVariables.SystemView == SystemView.UpdateAccount)
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (!checkUpdateChanges())
                    {
                        accountUpdate_Save.Enabled = true;
                        accountUpdate_Save.BackColor = Color.White;
                    }
                    else
                    {
                        accountUpdate_Save.Enabled = false;
                        accountUpdate_Save.BackColor = Color.Gray;
                    }
                }));
            }
        }

        public bool checkUpdateChanges()
        {
            List<bool> checkChanges = new List<bool>();

            checkChanges.Add((accountUpdate_FirstName.Text == GlobalVariables.CardData.Firstname) ? true : false);
            checkChanges.Add((accountUpdate_LastName.Text == GlobalVariables.CardData.Lastname) ? true : false);
            checkChanges.Add((accountUpdate_CardSerial.Text == GlobalVariables.CardData.CardSerial) ? true : false);


            List<Area> AreaUpdateList = SetArea_Update();
            checkChanges.Add((accountUpdate_PlateNumber.Text == GlobalVariables.CardData.parkingData.Platenum) ? true : false);

            checkChanges.Add((accountUpdate_CardSerial.Text == GlobalVariables.CardData.CardSerial) ? true : false);

            if (GlobalVariables.CardData.parkingData.ParkerType_ID != 0)
                checkChanges.Add((Convert.ToString(accountUpdate_ParkerType.SelectedValue) == Convert.ToString(GlobalVariables.CardData.parkingData.ParkerType_ID)) ? true : false);

            if (GlobalVariables.CardData.parkingData.AreaList != null)
                checkChanges.Add((GlobalVariables.CardData.parkingData.AreaList.All(AreaUpdateList.Contains) && AreaUpdateList.Count == GlobalVariables.CardData.parkingData.AreaList.Count) ? true : false);


            foreach (bool item in checkChanges)
            {
                if (!item)
                {
                    return false;
                }
            }

            return true;
        }

        private void accountUpdate_Save_Click(object sender, EventArgs e)
        {


            if (ValidateUpdateInputs())
            {

                GlobalVariables.CardData.Firstname = accountUpdate_FirstName.Text;
                GlobalVariables.CardData.Lastname = accountUpdate_LastName.Text;
                GlobalVariables.CardData.CardSerial = accountUpdate_CardSerial.Text;


                GlobalVariables.CardData.parkingData.Platenum = accountUpdate_PlateNumber.Text;
                GlobalVariables.CardData.parkingData.ParkerType_ID = (int)accountUpdate_ParkerType.SelectedValue;
                GlobalVariables.CardData.parkingData.AreaList = SetArea_Update();

                GlobalFunctions.UpdateAccount(GlobalVariables.CardData.parkingData.CardHolder_ID);

                GlobalVariables.SystemView = SystemView.DisplayAccount;
                account_DisplayPanel.Visible = true;
                account_UpdatePanel.Visible = false;
                clearAccountDisplayData();
                if (GlobalVariables.CardData.CardSerial != "")
                {
                    DisplayAccount_CardFound(GlobalVariables.CardData.CardSerial);
                }
                else
                {
                    DisplayAccount_ByID(GlobalVariables.CardData.parkingData.CardHolder_ID);
                }
            }
        }

        public bool ValidateUpdateInputs()
        {
            List<CheckList> checklist = new List<CheckList>();

            checklist.Add(new CheckList { validate = accountUpdate_FirstName.Text != "" ? true : false, error = "Missing First Name" });
            checklist.Add(new CheckList { validate = accountUpdate_LastName.Text != "" ? true : false, error = "Missing Last Name" });
            checklist.Add(new CheckList { validate = accountUpdate_CardSerial.Text != "" ? true : false, error = "Card Serial Missing" });
            checklist.Add(new CheckList { validate = GlobalFunctions.validateCardSerial(accountUpdate_CardSerial.Text) || GlobalVariables.CardData.CardSerial == accountUpdate_CardSerial.Text ? true : false, error = "Card Serial is already being used" });


            checklist.Add(new CheckList { validate = accountUpdate_PlateNumber.Text != "" ? true : false, error = "Missing Plate Number" });
            checklist.Add(new CheckList { validate = accountUpdate_ParkerType.SelectedItem != null ? true : false, error = "Select a Parker Type" });

            bool checkarea = false;
            foreach (Area area in SetArea_Update())
            {
                checkarea = true;
            }
            checklist.Add(new CheckList { validate = checkarea ? true : false, error = "Select at least one area" });

            foreach (CheckList item in checklist)
            {
                if (!item.validate)
                {
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Account Error", BottomText = item.error });
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Disable Account


        private void account_DeactivateButton_Click(object sender, EventArgs e)
        {
            if (GlobalVariables.CardData != null)
            {
                if (!GlobalVariables.CardData.parkingData.CardHolderStatus)
                {
                    GlobalFunctions.ReactivateAccount();
                    DisplayAccount_CardFound(GlobalVariables.CardData.CardSerial);
                }
                else
                {
                    GlobalFunctions.DisableAccount();
                    DisplayAccount_CardFound(GlobalVariables.CardData.CardSerial);
                }
            }
        }


        #endregion


        #endregion

        #region Display Logs


        private void accountDisplay_AccountTab_Click(object sender, EventArgs e)
        {
            userAccountPanel.Visible = true;
            userHistoryPanel.Visible = false;
        }

        private void accountDisplay_HistoryTab_Click(object sender, EventArgs e)
        {
            userAccountPanel.Visible = false;
            userHistoryPanel.Visible = true;
            DisplayLogs();
        }

        public void DisplayLogs()
        {
            try
            {
                if (GlobalVariables.CardData != null)
                {
                    actionHistoryPanelHolder.Controls.Clear();
                    List<ActionHistory> Logs = GlobalFunctions.getHistoryLog();

                    foreach (ActionHistory logitem in Logs)
                    {
                        ActionHistoryPanel logpanel = new ActionHistoryPanel();
                        logpanel.Name = "logpanel" + logitem.ActionHistoryID;
                        logpanel.Action = logitem.Action;
                        logpanel.Description = logitem.Description;
                        logpanel.Time = logitem.date.ToString("hh:mm tt");
                        logpanel.Date = logitem.date.ToString("dd MMMM yyyy");
                        logpanel.Anchor = AnchorStyles.Top;

                        logpanel.BringToFront();

                        actionHistoryPanelHolder.Controls.Add(logpanel);

                    }

                    foreach (ActionHistoryPanel logpanel2 in actionHistoryPanelHolder.Controls)
                    {
                        logpanel2.BringToFront();
                    }
                }
            } catch (Exception exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Log Error", BottomText = exc.Message });
            }
        }

        #endregion

        #region Clear Records

        public void ClearRecords()
        {
            GlobalVariables.TransactionData = new TransactionData();
            recordDisplay_ParkingStatusIcon.Image = global::CardReader2.Properties.Resources.Parking_Status_Grey;
            recordDisplay_PicturePanel.Controls.Clear();

            recordDisplay_Time.Visible = false;
            vehicleImagePanel.Visible = false;

            recordDisplay_PlateNumber.Text = "";
            recordDisplay_TicketNumber.Text = "";
            recordDisplay_EntryArea.Text = "";

            recordDisplay_EntryTerminal.Text = "";
            recordDisplay_EntryDate.Text = "";

            recordDisplay_PaymentInfoTable.Visible = false;
            recordDisplay_PaymentTerminal.Text = "";
            recordDisplay_PaymentArea.Text = "";
            recordDisplay_PaymentDate.Text = "";

            recordDisplay_ExitInfoTable.Visible = false;
            recordDisplay_ExitTerminal.Text = "";
            recordDisplay_ExitDate.Text = "";
            recordDisplay_ExitArea.Text = "";

            SetClearButton(false);

        }

        private async void recordDisplay_SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalVariables.TransactionData = new TransactionData();
                GlobalFunctions.SearchTransactionEntry(GlobalFunctions.SearchTicketNum(recordDisplay_SearchTextbox.Text));

                if (GlobalVariables.TransactionData.TransactionTicketNum != "")
                    SetTransactionData();

                recordDisplay_ImageLoading.Visible = true;

                await Task.Run(() => GetImages());
                DisplayImage();

                recordDisplay_ImageLoading.Visible = false;
            }
            catch (Exception exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Error", BottomText = exc.Message });
            }
        }

        public async void SearchRecord_CardFound(string CardSerial)
        {
            try
            {
                GlobalVariables.TransactionData = new TransactionData();
                GlobalFunctions.SearchTransactionEntry(GlobalFunctions.SearchTicketNum(CardSerial));

                if (GlobalVariables.TransactionData.TransactionTicketNum != "")
                {
                    SetTransactionData();
                    ///recordDisplay_ClearButton_Click(recordDisplay_ClearButton, EventArgs.Empty);
                }
                   

                recordDisplay_ImageLoading.Visible = true;

                await Task.Run(() => GetImages());
                DisplayImage();

                recordDisplay_ImageLoading.Visible = false;
            }
            catch (Exception exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Error", BottomText = exc.Message });
            }
        }

        public void SearchRecord_CardRemoved()
        {
            ClearRecords();
        }


        public void SetTransactionData()
        {
            //if not Exited
            if (!(GlobalVariables.TransactionData.TransactionEntryDate != "" && GlobalVariables.TransactionData.TransactionPaymentDate != "" && GlobalVariables.TransactionData.TransactionExitDate != ""))
            {
                //Check Overtime
                if (GlobalVariables.TransactionData.TransactionPaid && GlobalFunctions.CheckOvertime())
                {
                    DateTime serverTime = GlobalFunctions.getServerTime();

                    GlobalVariables.TransactionData.IsOvertime = true;
                }
            }



            //Set Parking Status
            if (!GlobalVariables.TransactionData.IsOvertime)
            {
                //Entry
                if (GlobalVariables.TransactionData.TransactionEntryDate != "" && GlobalVariables.TransactionData.TransactionPaymentDate == "" && GlobalVariables.TransactionData.TransactionExitDate == "")
                {
                    recordDisplay_ParkingStatusIcon.Image = global::CardReader2.Properties.Resources.Parking_Status_Entry;
                    recordDisplay_PaymentInfoTable.Visible = false;
                    recordDisplay_ExitInfoTable.Visible = false;

                    SetClearButton(true);
                }

                //Payment
                if (GlobalVariables.TransactionData.TransactionEntryDate != "" && GlobalVariables.TransactionData.TransactionPaymentDate != "" && GlobalVariables.TransactionData.TransactionExitDate == "")
                {
                    recordDisplay_ParkingStatusIcon.Image = global::CardReader2.Properties.Resources.Parking_Status_Paid;
                    recordDisplay_PaymentInfoTable.Visible = true;
                    recordDisplay_ExitInfoTable.Visible = false;

                    //Payment
                    recordDisplay_PaymentDate.Text = GlobalVariables.TransactionData.TransactionPaymentDate;
                    recordDisplay_PaymentArea.Text = GlobalVariables.AreaList.Where(x => x.Area_ID == GlobalVariables.TransactionData.TransactionPaymentAreaID).FirstOrDefault().Area_Name;
                    recordDisplay_PaymentTerminal.Text = GlobalVariables.TerminalList.Find(x => x.Terminal_ID == GlobalVariables.TransactionData.PaymentTermID).Terminal_Name;

                    recordDisplay_Time.Visible = true;
                    recordDisplay_Time.Text = "Time Remaining: " + GlobalFunctions.ComputeTimeRemaining();

                    SetClearButton(true);
                }

                //Exit
                if (GlobalVariables.TransactionData.TransactionEntryDate != "" && GlobalVariables.TransactionData.TransactionPaymentDate != "" && GlobalVariables.TransactionData.TransactionExitDate != "")
                {
                    recordDisplay_ParkingStatusIcon.Image = global::CardReader2.Properties.Resources.Parking_Status_Exited;
                    recordDisplay_PaymentInfoTable.Visible = true;
                    recordDisplay_ExitInfoTable.Visible = true;

                    //Exit
                    recordDisplay_PaymentDate.Text = GlobalVariables.TransactionData.TransactionPaymentDate;
                    recordDisplay_PaymentArea.Text = GlobalVariables.AreaList.Where(x => x.Area_ID == GlobalVariables.TransactionData.TransactionPaymentAreaID).FirstOrDefault().Area_Name;
                    recordDisplay_PaymentTerminal.Text = GlobalVariables.TerminalList.Find(x => x.Terminal_ID == GlobalVariables.TransactionData.PaymentTermID).Terminal_Name;

                    recordDisplay_ExitDate.Text = GlobalVariables.TransactionData.TransactionExitDate;
                    recordDisplay_ExitArea.Text = GlobalVariables.AreaList.Find(x => x.Area_ID == GlobalVariables.TerminalList.Find(y => y.Terminal_ID == GlobalVariables.TransactionData.ExitTermID).Terminal_Area.Area_ID).Area_Name;
                    recordDisplay_ExitTerminal.Text = GlobalVariables.TerminalList.Find(x => x.Terminal_ID == GlobalVariables.TransactionData.ExitTermID).Terminal_Name;

                    SetClearButton(false);
                }


            }
            else
            {
                recordDisplay_ParkingStatusIcon.Image = global::CardReader2.Properties.Resources.Parking_Status_Overstay;
                recordDisplay_PaymentInfoTable.Visible = false;
                recordDisplay_ExitInfoTable.Visible = false;

                recordDisplay_Time.Visible = true;
                recordDisplay_Time.Text = "Time Elapsed: " + GlobalFunctions.ComputeTimeElapsed();

                SetClearButton(true);
            }

            recordDisplay_PlateNumber.Text = GlobalVariables.TransactionData.PlateNumber;
            recordDisplay_TicketNumber.Text = GlobalVariables.TransactionData.TransactionTicketNum;
            recordDisplay_EntryArea.Text = GlobalVariables.AreaList.Find(x => x.Area_ID == GlobalVariables.TransactionData.TransactionEntryAreaID).Area_Name;

            //Entry
            recordDisplay_EntryDate.Text = GlobalVariables.TransactionData.TransactionEntryDate;
            recordDisplay_EntryTerminal.Text = GlobalVariables.TerminalList.Find(x => x.Terminal_ID == GlobalVariables.TransactionData.EntryTermID).Terminal_Name;
        }

        public async Task GetImages()
        {
            List<Bitmap> image = new List<Bitmap>();

            try
            {
                try
                {
                    string transactionid = GlobalVariables.TransactionData.TransactionID.ToString();
                    string imgloc_file = "http://" + GlobalVariables.FTP_IP + "/images/" + GlobalVariables.TransactionData.TransactionEntryAreaID + "/" + transactionid + "/";

                    WebClient ftpClient = new WebClient();
                    WebClient ftpClient2 = new WebClient();
                    WebClient ftpClient3 = new WebClient();
                    WebClient ftpClient4 = new WebClient();

                    #region AWAIT IMAGES 
                    if (GlobalVariables.TransactionData.TransactionEntryAreaID == 6)
                    {
                        Task<byte[]> entrycar_byteimage = ProcessURLAsync(GlobalVariables.GrandWing_ImageLoc + GlobalVariables.TransactionData.VehicleImageLoc, ftpClient);
                        Task<byte[]> entrydriver_byteimage = ProcessURLAsync(GlobalVariables.GrandWing_ImageLoc + GlobalVariables.TransactionData.ParkerImageLoc, ftpClient2);

                        //Retrieve Entry Car
                        try
                        {
                            byte[] entrycarimage = await entrycar_byteimage;
                            GlobalVariables.TransactionData.VehicleImages.Add(ByteToImage(entrycarimage));
                        }
                        catch (Exception exc)
                        {
                            GlobalVariables.TransactionData.EntryCar = null;
                        }

                        //Retrieve Entry Driver
                        try
                        {
                            byte[] entrydriverimage = await entrydriver_byteimage;
                            GlobalVariables.TransactionData.VehicleImages.Add(ByteToImage(entrydriverimage));
                        }
                        catch (Exception exc)
                        {
                            GlobalVariables.TransactionData.EntryDriver = null;
                        }
                    }
                    else
                    {
                        Task<byte[]> entrycar_byteimage = ProcessURLAsync(imgloc_file + "entrycar.jpg", ftpClient);
                        Task<byte[]> entrydriver_byteimage = ProcessURLAsync(imgloc_file + "entrydriver.jpg", ftpClient2);
                        Task<byte[]> exitcar_byteimage = ProcessURLAsync(imgloc_file + "exitcar.jpg", ftpClient3);
                        Task<byte[]> exitdriver_byteimage = ProcessURLAsync(imgloc_file + "exitdriver.jpg", ftpClient4);

                        //Retrieve Entry Car
                        try
                        {
                            byte[] entrycarimage = await entrycar_byteimage;
                            GlobalVariables.TransactionData.VehicleImages.Add(ByteToImage(entrycarimage));
                        }
                        catch (Exception exc)
                        {
                            GlobalVariables.TransactionData.EntryCar = null;
                        }

                        //Retrieve Entry Driver
                        try
                        {
                            byte[] entrydriverimage = await entrydriver_byteimage;
                            GlobalVariables.TransactionData.VehicleImages.Add(ByteToImage(entrydriverimage));
                        }
                        catch (Exception exc)
                        {
                            GlobalVariables.TransactionData.EntryDriver = null;
                        }

                        //Retrieve Exit Car
                        try
                        {
                            byte[] exitcarimage = await exitcar_byteimage;
                            GlobalVariables.TransactionData.VehicleImages.Add(ByteToImage(exitcarimage));
                        }
                        catch (Exception exc)
                        {
                            GlobalVariables.TransactionData.ExitCar = null;
                        }

                        //Retrieve Exit Driver
                        try
                        {
                            byte[] exitdriverimage = await exitdriver_byteimage;
                            GlobalVariables.TransactionData.VehicleImages.Add(ByteToImage(exitdriverimage));
                        }
                        catch (Exception exc)
                        {
                            GlobalVariables.TransactionData.ExitCar = null;
                        }
                    }
                    #endregion                  
                }
                catch (Exception exc)
                {
                    //MessageBox.Show("There was an error displaying image.");
                    //CallToast(MainForm, "Caution", "Image Retrieval Failed", exc.Message);
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Image Error", BottomText = exc.Message });
                }
            }
            catch (Exception ex)
            {
                //CallToast(MainForm, "Caution", "Image Retrieval Failed", ex.Message);
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Image Error", BottomText = ex.Message });
            }
        }

        async Task<byte[]> ProcessURLAsync(string url, WebClient client)
        {
            var byteArray = await client.DownloadDataTaskAsync(url);
            return byteArray;
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        public void DisplayImage()
        {
            recordDisplay_PicturePanel.Controls.Clear();
            foreach (Bitmap pic in GlobalVariables.TransactionData.VehicleImages)
            {
                PictureBox vehicleimage = new PictureBox();

                vehicleimage.Image = pic;
                vehicleimage.SizeMode = PictureBoxSizeMode.StretchImage;
                vehicleimage.Width = 220;
                vehicleimage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                vehicleimage.Click += new EventHandler(picture_Click);

                recordDisplay_PicturePanel.Controls.Add(vehicleimage);
                vehicleimage.Dock = DockStyle.Left;
            }
        }

        protected void picture_Click(object sender, EventArgs e)
        {
            PictureBox name = (PictureBox)sender;
            SetVehicleMain(name.Image);
            vehicleImagePanel.Visible = true;
        }

        private void closeVehicleDisplay_Click(object sender, EventArgs e)
        {
            vehicleImagePanel.Visible = false;
        }

        public void SetVehicleMain(Image image)
        {

            img = image;
            Graphics g = this.CreateGraphics();
            imgx = 0;
            imgy = 0;
            zoom = ((float)vehicleMainPicture.Width / (float)img.Width) * (img.HorizontalResolution / g.DpiX);

            vehicleMainPicture.Refresh();
        }

        #region Image Paint

        Image img;
        Point mouseDown;
        int startx = 0;                         // offset of image when mouse was pressed
        int starty = 0;
        int imgx = 0;                         // current offset of image
        int imgy = 0;

        bool mousepressed = false;  // true as long as left mousebutton is pressed
        float zoom = 1;


        private void vehicleMainPicture_MouseHover(object sender, EventArgs e)
        {

        }

        private void vehicleMainPicture_MouseLeave(object sender, EventArgs e)
        {

        }

        private void vehicleMainPicture_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                Point mousePosNow = mouse.Location;

                int deltaX = mousePosNow.X - mouseDown.X; // the distance the mouse has been moved since mouse was pressed
                int deltaY = mousePosNow.Y - mouseDown.Y;

                imgx = (int)(startx + (deltaX / zoom));  // calculate new offset of image based on the current zoom factor
                imgy = (int)(starty + (deltaY / zoom));

                vehicleMainPicture.Refresh();
            }
        }

        private void vehicleMainPicture_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouse = e as MouseEventArgs;

            if (mouse.Button == MouseButtons.Left)
            {
                if (!mousepressed)
                {
                    mousepressed = true;
                    mouseDown = mouse.Location;
                    startx = imgx;
                    starty = imgy;
                }
            }
        }

        private void vehicleMainPicture_MouseUp(object sender, MouseEventArgs e)
        {
            mousepressed = false;
        }

        public void UpImage()
        {

        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float oldzoom = zoom;

            if (e.Delta > 0)
            {
                zoom += 0.1F;
            }

            else if (e.Delta < 0)
            {
                zoom = Math.Max(zoom - 0.1F, 0.01F);
            }

            MouseEventArgs mouse = e as MouseEventArgs;
            Point mousePosNow = mouse.Location;

            int x = mousePosNow.X - vehicleMainPicture.Location.X;    // Where location of the mouse in the pictureframe
            int y = mousePosNow.Y - vehicleMainPicture.Location.Y;

            int oldimagex = (int)(x / oldzoom);  // Where in the IMAGE is it now
            int oldimagey = (int)(y / oldzoom);

            int newimagex = (int)(x / zoom);     // Where in the IMAGE will it be when the new zoom i made
            int newimagey = (int)(y / zoom);

            imgx = newimagex - oldimagex + imgx;  // Where to move image to keep focus on one point
            imgy = newimagey - oldimagey + imgy;

            vehicleMainPicture.Refresh();  // calls imageBox_Paint
        }

        private void vehicleMainPicture_Paint(object sender, PaintEventArgs e)
        {
            if (img != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.ScaleTransform(zoom, zoom);
                e.Graphics.DrawImage(img, imgx, imgy);
            }
        }



        #endregion 

        public void SetClearButton(bool set)
        {
            if (set)
            {
                recordDisplay_ClearButton.Enabled = set;
                recordDisplay_ClearButton.BackColor = Color.White;
            }
            else
            {
                recordDisplay_ClearButton.Enabled = set;
                recordDisplay_ClearButton.BackColor = Color.Gray;
            }
        }

        private void recordDisplay_ClearButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Entry
                if (GlobalVariables.TransactionData.TransactionEntryDate != "" && GlobalVariables.TransactionData.TransactionPaymentDate == "" && GlobalVariables.TransactionData.TransactionExitDate == "")
                {
                    GlobalFunctions.voidEntry(GlobalVariables.TerminalList.Find(x => (x.Terminal_Type == TerminalType.Exit) && (x.Terminal_Area.Area_ID == GlobalVariables.TransactionData.TransactionEntryAreaID)));
                    GlobalFunctions.CreateAuditLog(AuditLogActions.VoidedTransaction, GlobalVariables.AdminID);
                    ClearRecords();
                }
                //Payment
                else if (GlobalVariables.TransactionData.TransactionEntryDate != "" && GlobalVariables.TransactionData.TransactionPaymentDate != "" && GlobalVariables.TransactionData.TransactionExitDate == "")
                {
                    GlobalFunctions.exitEntry(GlobalVariables.TerminalList.Find(x => (x.Terminal_Type == TerminalType.Exit) && (x.Terminal_Area.Area_ID == GlobalVariables.TransactionData.TransactionEntryAreaID)));
                    GlobalFunctions.CreateAuditLog(AuditLogActions.VoidedTransaction, GlobalVariables.AdminID);
                    ClearRecords();
                }
                else
                {
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Clear Error", BottomText = "Transaction not valid for clearing" });
                }

            }
            catch (Exception exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Terminal Error", BottomText = exc.Message });
            }
        }

        #endregion

        #region Member

        private void ShowMember()
        {
            GlobalVariables.MemberData = new Member();

            Member_UpdateMember_Button.BackColor = Color.Gray;
            Member_UpdateMember_Button.Enabled = false;
            Member_DeactivateMember_Button.BackColor = Color.Gray;
            Member_DeactivateMember_Button.Enabled = false;
        }

        private void Member_NewMember_Button_Click(object sender, EventArgs e)
        {
            GlobalVariables.SystemView = SystemView.MemberNew;
            
            MemberPanel_New.Visible = true;
            MemberPanel_Update.Visible = false;
            MemberPanel_Main.Visible = false;
        }

        public void Member_Main_SetData(Member data)
        {
            //clear first
            MemberMain_Clear();

            
            //set data
            Member_DateCreated_Label.Text = data.DateCreated;
            Member_LastModified_Label.Text = data.Last_Modified;
            Member_CardSerial_Text.Text = data.CardSerial;
            Member_MagData_Text.Text = data.MagData;


            //member status
            if (!data.Member_Status)
            {
                if(data.MemberID != 0)
                {
                    Member_Disabled_Prompt.Visible = true;
                    Member_UpdateMember_Button.BackColor = Color.Gray;
                    Member_UpdateMember_Button.Enabled = false;
                    Member_DeactivateMember_Button.Text = "Reactivate";
                    Member_DeactivateMember_Button.BackColor = Color.White;
                    Member_DeactivateMember_Button.Enabled = true;
                }
                else
                {
                    Member_Disabled_Prompt.Visible = false;
                    Member_UpdateMember_Button.BackColor = Color.Gray;
                    Member_UpdateMember_Button.Enabled = false;                  
                    Member_DeactivateMember_Button.BackColor = Color.Gray;
                    Member_DeactivateMember_Button.Enabled = false;
                }
                
            }
            //enable buttons
            else
            {
                Member_Disabled_Prompt.Visible = false;
                Member_UpdateMember_Button.BackColor = Color.White;
                Member_UpdateMember_Button.Enabled = true;
                Member_DeactivateMember_Button.BackColor = Color.White;
                Member_DeactivateMember_Button.Enabled = true;
            }

            
        }
        //ACS Card for Member
        public void NewMember_CardFound()
        {
            CardReader.ACR122Pooling();
            string CardSerial = CardReader.ReadingOfCards();
            Member_New_CardSerial_Textbox.Text = CardSerial;

            Member_New_CardSerial_Icon.Visible = true;
            if (GlobalFunctions.validateCardSerial_Member(CardSerial))
                Member_New_CardSerial_Icon.Image = global::CardReader2.Properties.Resources.Ok_Icon1;
            else
            {
                Member_New_CardSerial_Icon.Image = global::CardReader2.Properties.Resources.Error_Icon1;
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "RFID Error", BottomText = "The RFID Serial is already in use" });
            }
                

        }
       
        public void NewMember_CardSwipe(string MagData)
        {
            Member_New_MagData_Textbox.Text = MagData;

            Member_New_MagData_Icon.Visible = true;
            if (GlobalFunctions.validateMagData_Member(MagData))
                Member_New_MagData_Icon.Image = global::CardReader2.Properties.Resources.Ok_Icon1;
            else
            {
                Member_New_MagData_Icon.Image = global::CardReader2.Properties.Resources.Error_Icon1;
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Swipe Error", BottomText = "This card is already in use" });
            }              
        }


        private void Member_New_Save_Button_Click(object sender, EventArgs e)
        {
            Member memberdata = new Member();

            memberdata.CardSerial = Member_New_CardSerial_Textbox.Text;
            memberdata.MagData = Member_New_MagData_Textbox.Text;
            memberdata.CreatedBy = GlobalVariables.AdminID;


            //if card serial is new
            if (GlobalFunctions.validateCardSerial_Member(memberdata.CardSerial))
            {
                //if mag data is new 
                if (GlobalFunctions.validateMagData_Member(memberdata.MagData))
                {
                    //Create new Member
                    if (GlobalFunctions.Member_New(memberdata))
                    {
                        //show new member if success
                        Member_Main_SetData(GlobalFunctions.Member_Search_CardSerial(memberdata.CardSerial));
                        MemberPanel_New.Visible = false;
                        MemberPanel_Main.Visible = true;
                        GlobalVariables.SystemView = SystemView.MemberMain;
                        GlobalFunctions.CallToast(new ToastData { type = ToastType.Success, HeaderText = "New Member Created", BottomText = "" });
                    }
                }
                else
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "RFID Error", BottomText = "The RFID Serial is already in use" });
            }
            else
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Swipe Error", BottomText = "This card is already in use" });

        }

        private void Member_New_Cancel_Button_Click(object sender, EventArgs e)
        {
            MemberPanel_New.Visible = false;
            MemberPanel_Main.Visible = true;
            GlobalVariables.SystemView = SystemView.MemberMain;
        }

        private void Member_UpdateMember_Button_Click(object sender, EventArgs e)
        {
            GlobalVariables.SystemView = SystemView.MemberUpdate;
            MemberPanel_Update.Visible = true;
            MemberPanel_Main.Visible = false;

            Member_Update_SetData(GlobalFunctions.Member_Search_CardSerial(Member_CardSerial_Text.Text));
        }

        public void UpdateMember_CardFound()
        {
            CardReader.ACR122Pooling();
            string CardSerial = CardReader.ReadingOfCards();
            string RecordSerial = GlobalFunctions.validateCardSerial_Member_Update(CardSerial);
            Member_New_CardSerial_Icon.Visible = false;

            //check if cardserial exists
            if (RecordSerial != ""){
                //if its does exist and is NOT the same cardserial as selected -then error
                if(Member_Update_CardSerial_Textbox.Text != RecordSerial)
                {
                    Member_Update_CardSerial_Textbox.Text = CardSerial;
                    Member_Update_CardSerial_Icon.Visible = true;
                    Member_Update_CardSerial_Icon.Image = global::CardReader2.Properties.Resources.Error_Icon1;
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "RFID Error", BottomText = "The RFID Serial is already in use" });
                }
                //if its the same -then ignore
                else
                {
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Caution, HeaderText = "RFID", BottomText = "You tapped the same card" });
                }
            }else
            {
                Member_Update_CardSerial_Textbox.Text = CardSerial;
                Member_Update_CardSerial_Icon.Visible = true;
                Member_Update_CardSerial_Icon.Image = global::CardReader2.Properties.Resources.Ok_Icon1;
            }         
        }

        public void UpdateMember_CardSwipe(string MagData)
        {
            string RecordData = GlobalFunctions.validateMagData_Member_Update(MagData);

            //check if magdata exists
            if (RecordData != "")
            {
                //if its does exist and is NOT the same cardserial as selected -then error
                if (Member_Update_MagData_Textbox.Text != RecordData)
                {
                    Member_Update_MagData_Textbox.Text = RecordData;
                    Member_Update_MagData_Icon.Visible = true;
                    Member_Update_MagData_Icon.Image = global::CardReader2.Properties.Resources.Error_Icon1;
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Swipe Error", BottomText = "This member card is already in use" });
                }
                //if its the same -then ignore
                else
                {
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Caution, HeaderText = "Swipe Caution", BottomText = "You swiped the same card" });
                }
            }
            else
            {
                Member_Update_MagData_Textbox.Text = MagData;
                Member_Update_MagData_Icon.Visible = true;
                Member_Update_MagData_Icon.Image = global::CardReader2.Properties.Resources.Ok_Icon1;
            }
        }

        public void UpdateMember_CardRemoved()
        {
            Member_Update_CardSerial_Icon.Visible = false;
            Member_Update_CardSerial_Textbox.Text = GlobalVariables.MemberData.CardSerial;
        }

        public void Member_Update_SetData(Member data)
        {
            Member_Update_CardSerial_Textbox.Text = data.CardSerial;
            Member_Update_MagData_Textbox.Text = data.MagData;
        }



        private void Member_Update_Save_Button_Click(object sender, EventArgs e)
        {
            bool cardserial_pass = false;
            string cardserial = GlobalFunctions.validateCardSerial_Member_Update(Member_Update_CardSerial_Textbox.Text);
            
            //if serial is NOT the same as selected serial and is NOT empty -then error
            if(cardserial != GlobalVariables.MemberData.CardSerial && cardserial != "")
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "RFID Error", BottomText = "The RFID Serial is already in use" });
            }
            else
            {
                cardserial_pass = true;
            }

            bool magdata_pass = false;
            string magdata = GlobalFunctions.validateMagData_Member_Update(Member_Update_MagData_Textbox.Text);

            //if magdata is NOT the same as selected magdata and is NOT empty -then error
            if (magdata != GlobalVariables.MemberData.MagData && magdata!= "")
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "MagData Error", BottomText = "The Member Card is already in use" });
            }
            else
            {
                magdata_pass = true;
            }



            if(cardserial_pass && magdata_pass)
            {
                Member data = new Member();
                data.CardSerial = Member_Update_CardSerial_Textbox.Text;
                data.MagData = Member_Update_MagData_Textbox.Text;
                data.MemberID = GlobalVariables.MemberData.MemberID;

                if (GlobalFunctions.UpdateMemberHolder(data))
                {
                    Member_Main_SetData(GlobalFunctions.Member_Search_CardSerial(data.CardSerial));
                    GlobalVariables.SystemView = SystemView.MemberMain;
                    MemberPanel_Main.Visible = true;
                    MemberPanel_Update.Visible = false;
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Success, HeaderText = "Member Successfully Updated", BottomText = "" });

                }
            }
        }

        private void Member_DeactivateMember_Button_Click(object sender, EventArgs e)
        {
            GlobalFunctions.Member_Search_CardSerial(Member_CardSerial_Text.Text);
            if (GlobalVariables.MemberData.Member_Status)
            {
                GlobalFunctions.DisableMemberHolder(GlobalVariables.MemberData.MemberID);
            }
            if (!GlobalVariables.MemberData.Member_Status)
            {
                GlobalFunctions.EnableMemberHolder(GlobalVariables.MemberData.MemberID);
            }

            Member_Main_SetData(GlobalFunctions.Member_Search_CardSerial(GlobalVariables.MemberData.CardSerial));
        }

        private void Member_Update_Cancel_Button_Click(object sender, EventArgs e)
        {
            GlobalVariables.SystemView = SystemView.MemberMain;
            MemberPanel_Main.Visible = true;
            MemberPanel_Update.Visible = false;
        }

        public void MemberMain_Clear()
        {
            GlobalVariables.MemberData = new Member();

            Member_DateCreated_Label.Text = "";
            Member_LastModified_Label.Text = "";
            Member_CardSerial_Text.Text = "";
            Member_MagData_Text.Text = "";

            Member_Disabled_Prompt.Visible = false;
            Member_UpdateMember_Button.BackColor = Color.Gray;
            Member_UpdateMember_Button.Enabled = false;
            Member_DeactivateMember_Button.Text = "Deactivate";
            Member_DeactivateMember_Button.BackColor = Color.Gray;
            Member_DeactivateMember_Button.Enabled = false;

          
        }

        public void MemberNew_Clear()
        {
            Member_New_CardSerial_Textbox.Text = "";
            Member_New_CardSerial_Icon.Visible = false;
            Member_New_MagData_Textbox.Text = "";
            Member_New_MagData_Icon.Visible = false;

        }
        #endregion
        //Prompts 
        private void accountDisplay_CautionIcon_Click(object sender, EventArgs e)
        {
            GlobalFunctions.CallToast(new ToastData { type = ToastType.Caution, HeaderText = "Account Error", BottomText = "This account has missing information regarding parking data, it is recommended that you update this account with complete parking information" });
        }

        private void tableLayoutPanel34_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Row == 1 && e.Column == 0)
            {
                e.Graphics.DrawLine(Pens.Black, e.CellBounds.Location, new Point(e.CellBounds.Right, e.CellBounds.Top));
            }
        }

        private void MenuSettingsExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

       
    }
}
