using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CardReader2
{
    class GlobalFunctions
    {
        static Main _Main;
        public static Main MainForm
        {
            get { return _Main; }
            set { _Main = value; }
        }

        public static void CallToast(ToastData data)
        {
            MainForm.BeginInvoke((MethodInvoker)delegate
            {
                switch (data.type)
                {
                    case (ToastType.Success):
                        MainForm.toast.HeaderColor = System.Drawing.Color.OliveDrab;
                        MainForm.toast.BodyColor = System.Drawing.Color.ForestGreen;
                        MainForm.toast.successPanel.Visible = true;
                        MainForm.toast.errorPanel.Visible = false;
                        MainForm.toast.toastOkLabel.Text = data.HeaderText;
                        MainForm.toast.Image = global::CardReader2.Properties.Resources.ok_icon;
                        MainForm.toast.BringToFront();
                        MainForm.toast.Visible = true;

                        break;
                    case (ToastType.Caution):
                        MainForm.toast.HeaderColor = System.Drawing.Color.Orange;
                        MainForm.toast.BodyColor = System.Drawing.Color.FromArgb(245, 171, 53);
                        MainForm.toast.successPanel.Visible = false;
                        MainForm.toast.errorPanel.Visible = true;
                        MainForm.toast.toastErrorLabel.Text = data.HeaderText;
                        MainForm.toast.toastErrorDetailsLabel.Text = data.BottomText;
                        MainForm.toast.Image = global::CardReader2.Properties.Resources.caution_icon_white;
                        MainForm.toast.BringToFront();
                        MainForm.toast.Visible = true;
                        break;
                    case (ToastType.Error):
                        MainForm.toast.HeaderColor = System.Drawing.Color.OrangeRed;
                        MainForm.toast.BodyColor = System.Drawing.Color.Firebrick;
                        MainForm.toast.successPanel.Visible = false;
                        MainForm.toast.errorPanel.Visible = true;
                        MainForm.toast.toastErrorLabel.Text = data.HeaderText;
                        MainForm.toast.toastErrorDetailsLabel.Text = data.BottomText;
                        MainForm.toast.Image = global::CardReader2.Properties.Resources.error_icon;
                        MainForm.toast.BringToFront();
                        MainForm.toast.Visible = true;
                        break;


                }
            });
        }

        #region MySql Connection
        private static MySqlConnection connection;

        static string _connectionString;
        public static string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public static void SQLInitialize()
        {
            connectionString = "SERVER=" + GlobalVariables.Server + ";" + "PORT=" + GlobalVariables.ServerPort + ";" + "DATABASE=" + GlobalVariables.ServerDatabaseName + ";" + "UID=" + GlobalVariables.ServerUsername + ";" + "PASSWORD=" + GlobalVariables.ServerPassword + ";" + "convert zero datetime=True;";
            connection = new MySqlConnection(connectionString);
        }

        public static MySqlConnection cmdconn()
        {
            connection = new MySqlConnection(connectionString);
            return connection;
        }

        //open connection to database
        public static bool OpenConnection()
        {
            try
            {
                // string[] stringArray = new string[] { GlobalVariables.ServerName };
                //  //mdm if PINGING of 1st Server failed, connect to the second one.
                //if( PINGCLASS.Pinging.PING(stringArray))

                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                    return true;
                }
                return false;
            }
            catch (MySqlException ex)
            {

                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                if (ex.Message == "Unable to connect to any of the specified MySQL hosts.")
                {
                    MessageBox.Show(ex.Message + "\nReconnect to Server?", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                {
                    //addMsgToLog(ex.Message);
                }
                //Not used since program BREAKS at addMsgToLog(ex.Message);
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");

                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                        //case 1042:
                        //    MessageBox.Show("Unable to connect to any of the specified MySQL hosts.");
                        //    break;
                }
                return false;
            }
        }

        //Close connection
        public static bool CloseConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                    return true;
                }
                return false;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        #endregion

        public static DateTime getServerTime()
        {
            try
            {
                DateTime serverTime = new DateTime();

                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT NOW() as ServerTime";
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    serverTime = x.GetDateTime(x.GetOrdinal("ServerTime"));
                    CloseConnection();
                    return serverTime;
                }
                else { CloseConnection(); return new DateTime(); }

                return serverTime;
            }
            catch (Exception exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Server Time Error", BottomText = exc.Message });
                return new DateTime();
            }
        }

        #region Config

        public static void ReadTerminalConfig()
        {
            try
            {
                // Read each line of the file into a string array. Each element 
                // of the array is one line of the file. 
                string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\CONFIG\TerminalConfig.dat");

                // Display the file contents by using a foreach loop.
                foreach (string line in lines)
                {
                    GlobalVariables.TerminalName = lines[0];
                    GlobalVariables.Server = lines[1];
                    GlobalVariables.ServerPort = lines[2];
                    GlobalVariables.ServerDatabaseName = lines[3];
                    GlobalVariables.ServerUsername = lines[4];
                    GlobalVariables.ServerPassword = lines[5];
                    GlobalVariables.FTP_IP = lines[6];
                    GlobalVariables.GrandWing_ImageLoc = lines[7];
                }
            }
            catch (Exception exc) { MessageBox.Show("Terminal Config Read Error: " + exc.Message); }
        }

        public static int GetTerminalID()
        {
            int TermID = 0;
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select term.* from tbl_terminal as term where term_name = @termname";
                cmd.Parameters.AddWithValue("@termname", GlobalVariables.TerminalName);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    TermID = x.GetInt32(x.GetOrdinal("term_id"));
                }


                CloseConnection();
                return TermID;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return TermID;
            }
        }
        #endregion

        #region Login

        public static bool checkAdminLogin(string Username, string Password)
        {
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select user.* from tbl_user as user where username = @username AND password = @password";
                cmd.Parameters.AddWithValue("@username", Username);
                cmd.Parameters.AddWithValue("@password", Password);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    if (x.GetBoolean(x.GetOrdinal("status")))
                    {
                        if (x.GetInt32(x.GetOrdinal("userlevel")) == 1 || x.GetInt32(x.GetOrdinal("userlevel")) == 3 || x.GetInt32(x.GetOrdinal("userlevel")) == 4)
                        {
                            GlobalVariables.AdminID = x.GetInt32(x.GetOrdinal("userid"));
                            GlobalVariables.AdminUsername = x.GetString(x.GetOrdinal("username"));
                            GlobalVariables.AdminPassword = x.GetString(x.GetOrdinal("password"));
                            GlobalVariables.AdminName = x.GetString(x.GetOrdinal("firstname")) + " " + x.GetString(x.GetOrdinal("lastname"));
                            GlobalVariables.AdminType = x.GetInt32(x.GetOrdinal("userlevel"));
                            CallToast(new ToastData() { type = ToastType.Success, HeaderText = "Login Success" });
                            CloseConnection();
                            return true;
                        }
                        else
                        {
                            CallToast(new ToastData() { type = ToastType.Error, HeaderText = "Login Failed", BottomText = "Unauthorized Access" });
                            CloseConnection();
                            return false;
                        }
                    }
                    else
                    {
                        CallToast(new ToastData() { type = ToastType.Error, HeaderText = "Login Failed", BottomText = "Account Disabled" });
                        CloseConnection();
                        return false;
                    }
                }

                CallToast(new ToastData() { type = ToastType.Error, HeaderText = "Login Failed", BottomText = "Account does not exist" });
                CloseConnection();
                return false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }
        }

        #endregion



        public static List<Area> getAreaList(List<int> AreaId)
        {
            List<Area> areaList = new List<Area>();
            foreach (Area area in GlobalVariables.AreaList)
            {
                foreach (int areaid in AreaId)
                {
                    if (area.Area_ID == areaid)
                    {
                        areaList.Add(area);
                    }
                }
            }
            return areaList;
        }

        public static bool CheckRegularCard(string cardserial)
        {
            CardHolder cardholder = new CardHolder();
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select parkinfo.park_id, parkinfo.ticketnum from tbl_parkinfo as parkinfo where parkinfo.cardholder is  null and parkinfo.status = 1 and cardserial = @cardserial limit 1";
                cmd.Parameters.AddWithValue("@cardserial", cardserial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    CloseConnection();
                    return false;
                }

                CloseConnection();
                return true;
            }
            catch (Exception exc)
            {

                MessageBox.Show(exc.Message);
                CloseConnection();
                return true;
            }
        }

        public static List<ParkerType> GetParkerType()
        {
            List<ParkerType> parkertype_list = new List<ParkerType>();
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * from tbl_member where member_status = 1";
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (x.Read())
                {
                    if (x.GetString(x.GetOrdinal("member_code")) == "JCK" || x.GetString(x.GetOrdinal("member_code")) == "MTR" || x.GetString(x.GetOrdinal("member_code")) == "EMP" || x.GetString(x.GetOrdinal("member_code")) == "EMPTR" || x.GetString(x.GetOrdinal("member_code")) == "EXC")
                    {
                        ParkerType parkertype = new ParkerType();
                        parkertype.Member_ID = x.GetInt32(x.GetOrdinal("member_id"));
                        parkertype.Member_Type = x.GetString(x.GetOrdinal("member_type"));
                        parkertype.Member_Code = x.GetString(x.GetOrdinal("member_code"));
                        parkertype.MemberStatus = x.GetBoolean(x.GetOrdinal("member_status"));

                        parkertype_list.Add(parkertype);
                    }
                }

                CloseConnection();
                return parkertype_list;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
                return parkertype_list;
            }
        }

        public static List<Area> GetArea()
        {
            List<Area> area_list = new List<Area>();
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * from tbl_area where area_status = 1";
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (x.Read())
                {
                    Area area = new Area();

                    area.Area_ID = x.GetInt32(x.GetOrdinal("area_id"));
                    area.Area_Name = x.GetString(x.GetOrdinal("area_name"));
                    area.Area_Code = x.GetString(x.GetOrdinal("area_code"));
                    area.AreaStatus = x.GetBoolean(x.GetOrdinal("area_status"));

                    area_list.Add(area);
                }

                CloseConnection();
                return area_list;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
                return area_list;
            }
        }

        public static List<Terminal> GetTerminals()
        {
            List<Terminal> term_list = new List<Terminal>();
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * from tbl_terminal";
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (x.Read())
                {
                    Terminal term = new Terminal();

                    term.Terminal_ID = x.GetInt32(x.GetOrdinal("term_id"));
                    term.Terminal_Name = x.GetString(x.GetOrdinal("term_name"));
                    term.Terminal_Type = (TerminalType)Enum.Parse(typeof(TerminalType), x.GetString(x.GetOrdinal("termtype")));
                    term.Terminal_Area = GlobalVariables.AreaList.Find(y => y.Area_ID == x.GetInt32(x.GetOrdinal("term_area")));
                    term.Terminal_Status = x.GetBoolean(x.GetOrdinal("term_status"));

                    term_list.Add(term);
                }

                CloseConnection();
                return term_list;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
                return term_list;
            }
        }

        public static void CreateAccount(CardHolder cardholder)
        {
            try
            {
                CreateAccount_CardHolder(cardholder);

                CallToast(new ToastData { type = ToastType.Success, HeaderText = "New Account Created", BottomText = "" });
            }

            catch (MySqlException ex)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Account Error", BottomText = ex.Message });
                CloseConnection();
            }
        }



        public static void CreateAccount_CardHolder(CardHolder cardholder)
        {
            try
            {
                OpenConnection();
                string query = "INSERT INTO tbl_cardholder(cardserial,member_type,area_id,platenum,firstname,lastname,card_status) VALUES (@cardserial, @membertype, @areaid, @platenum, @firstname, @lastname, 1);";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@cardserial", cardholder.CardSerial);


                    cmd.Parameters.AddWithValue("@membertype", cardholder.parkingData.ParkerType_ID);
                    cmd.Parameters.AddWithValue("@areaid", JsonConvert.SerializeObject(cardholder.parkingData.AreaList.Select(m => m.Area_ID)));
                    cmd.Parameters.AddWithValue("@firstname", cardholder.Firstname);
                    cmd.Parameters.AddWithValue("@lastname", cardholder.Lastname);
                    cmd.Parameters.AddWithValue("@platenum", cardholder.parkingData.Platenum);

                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }

            catch (MySqlException ex)
            {
                CloseConnection();
            }
        }

        #region Update Account 

        public static void UpdateAccount(int CardHolder_ID)
        {
            try
            {
                //Update Card Holder if Record Exists
                if (CardHolder_ID != 0)
                    UpdateCardHolder();

                CallToast(new ToastData { type = ToastType.Success, HeaderText = "Account Updated Successfully", BottomText = "" });

            } catch (Exception exc)
            {

            }
        }


        public static void UpdateCardHolder()
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_cardholder SET member_type = @membertype, area_id = @areaid, platenum = @platenum, firstname = @firstname, lastname = @lastname, cardserial = @cardserial where cardholder_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", GlobalVariables.CardData.parkingData.CardHolder_ID);
                    cmd.Parameters.AddWithValue("@membertype", GlobalVariables.CardData.parkingData.ParkerType_ID);
                    cmd.Parameters.AddWithValue("@areaid", JsonConvert.SerializeObject(GlobalVariables.CardData.parkingData.AreaList.Select(m => m.Area_ID)));
                    cmd.Parameters.AddWithValue("@platenum", GlobalVariables.CardData.parkingData.Platenum);
                    cmd.Parameters.AddWithValue("@firstname", GlobalVariables.CardData.Firstname);
                    cmd.Parameters.AddWithValue("@lastname", GlobalVariables.CardData.Lastname);
                    cmd.Parameters.AddWithValue("@cardserial", GlobalVariables.CardData.CardSerial);


                    cmd.ExecuteNonQuery();
                }

                CloseConnection();
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
            }
        }



        #endregion

        public static bool validateCardSerial(string CardSerial)
        {
            try
            {
                if (CardSerial != "")
                {
                    OpenConnection();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select cardserial from (select cardserial from tbl_cardholder as cardholder where cardholder.card_status = 1 UNION select cardserial from tbl_user as user where user.status = 1 UNION select cardserial from tbl_parkinfo as parkinfo where parkinfo.status = 1) as list where list.cardserial = @cardserial";
                    cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                    cmd.Connection = connection;
                    MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (x.Read())
                    {
                        CloseConnection();
                        return false;
                    }
                    CloseConnection();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Validation Error", BottomText = ex.Message });
                return false;
            }
        }

        public static bool validateCardholder_CardSerial(string CardSerial)
        {
            return false;
        }

        #region Search Account By Card Serial

        public static CardHolder SearchAccount(string CardSerial)
        {
            CardHolder cardholder = new CardHolder();
            try
            {

                cardholder = SearchGeneralCardHolder(CardSerial);
                cardholder.parkingData = SearchCardHolder(CardSerial);

                return cardholder;
            } catch (Exception exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Account Error", BottomText = exc.Message });
                return cardholder;
            }
        }




        public static CardHolder SearchGeneralCardHolder(string CardSerial)
        {
            CardHolder cardholder = new CardHolder();
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select cardholder.*, member.member_type as parker_name, area.area_name, parkinfo.park_id, parkinfo.ticketnum, parkinfo.paymentdate from tbl_cardholder as cardholder left join tbl_parkinfo as parkinfo on parkinfo.cardholder = cardholder.cardholder_id AND parkinfo.cardserial = cardholder.cardserial left join tbl_member as member on member.member_id = cardholder.member_type left join tbl_area as area on area.area_id = cardholder.area_id where cardholder.cardserial = @cardserial group by cardholder.cardserial order by parkinfo.park_id DESC";
                cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    cardholder.Firstname = x.GetString(x.GetOrdinal("firstname"));
                    cardholder.Lastname = x.GetString(x.GetOrdinal("lastname"));
                    cardholder.CardSerial = x.GetString(x.GetOrdinal("cardserial"));
                    cardholder.DateCreated = x.GetDateTime(x.GetOrdinal("date_created"));
                }
                else
                {
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Error", BottomText = "Account not found" });
                }

                CloseConnection();
                return cardholder;
            }
            catch (Exception ex)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Account Error", BottomText = ex.Message });
                CloseConnection();
                return cardholder;
            }
        }

        public static ParkingData SearchCardHolder(string CardSerial)
        {
            ParkingData cardholder = new ParkingData();
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select cardholder.*, member.member_type as parker_name, area.area_name, parkinfo.park_id, parkinfo.ticketnum, parkinfo.paymentdate from tbl_cardholder as cardholder left join tbl_parkinfo as parkinfo on parkinfo.cardholder = cardholder.cardholder_id AND parkinfo.cardserial = cardholder.cardserial left join tbl_member as member on member.member_id = cardholder.member_type left join tbl_area as area on area.area_id = cardholder.area_id where cardholder.cardserial = @cardserial group by cardholder.cardserial order by parkinfo.park_id DESC";
                cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    cardholder.CardHolder_ID = x.GetInt32(x.GetOrdinal("cardholder_id"));
                    cardholder.Platenum = x.GetString(x.GetOrdinal("platenum"));
                    cardholder.ParkerType_ID = x.GetInt32(x.GetOrdinal("member_type"));
                    cardholder.ParkerType_Name = x.GetString(x.GetOrdinal("parker_name"));
                    cardholder.AreaList = getAreaList(JsonConvert.DeserializeObject<List<int>>(x.GetString(x.GetOrdinal("area_id"))));
                    cardholder.CardHolderStatus = x.GetBoolean(x.GetOrdinal("card_status"));
                }

                CloseConnection();

                return cardholder;
            }
            catch (Exception ex)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Account Error", BottomText = ex.Message });
                CloseConnection();
                return cardholder;
            }
        }

        #endregion

        #region Search Account By ID

        public static CardHolder SearchAccount_ByID(int CardholderID)
        {
            CardHolder cardholder = new CardHolder();
            try
            {

                cardholder = SearchGeneralCardHolder(CardholderID);
                cardholder.parkingData = SearchCardHolder(CardholderID);

                return cardholder;
            }
            catch (Exception exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Account Error", BottomText = exc.Message });
                return cardholder;
            }
        }




        public static CardHolder SearchGeneralCardHolder(int CardHolderID)
        {
            CardHolder cardholder = new CardHolder();
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select cardholder.*, member.member_type as parker_name, area.area_name, parkinfo.park_id, parkinfo.ticketnum, parkinfo.paymentdate from tbl_cardholder as cardholder left join tbl_parkinfo as parkinfo on parkinfo.cardholder = cardholder.cardholder_id AND parkinfo.cardserial = cardholder.cardserial left join tbl_member as member on member.member_id = cardholder.member_type left join tbl_area as area on area.area_id = cardholder.area_id where cardholder.cardholder_id = @cardholderid group by cardholder.cardserial order by parkinfo.park_id DESC";
                cmd.Parameters.AddWithValue("@cardholderid", CardHolderID);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    cardholder.Firstname = x.GetString(x.GetOrdinal("firstname"));
                    cardholder.Lastname = x.GetString(x.GetOrdinal("lastname"));
                    cardholder.CardSerial = x.GetString(x.GetOrdinal("cardserial"));
                }

                CloseConnection();
                return cardholder;
            }
            catch (Exception ex)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Account Error", BottomText = ex.Message });
                CloseConnection();
                return cardholder;
            }
        }

        public static ParkingData SearchCardHolder(int CardHolderID)
        {
            ParkingData cardholder = new ParkingData();
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select cardholder.*, member.member_type as parker_name, area.area_name, parkinfo.park_id, parkinfo.ticketnum, parkinfo.paymentdate from tbl_cardholder as cardholder left join tbl_parkinfo as parkinfo on parkinfo.cardholder = cardholder.cardholder_id AND parkinfo.cardserial = cardholder.cardserial left join tbl_member as member on member.member_id = cardholder.member_type left join tbl_area as area on area.area_id = cardholder.area_id where cardholder.cardholder_id = @cardholderid group by cardholder.cardserial order by parkinfo.park_id DESC";
                cmd.Parameters.AddWithValue("@cardholderid", CardHolderID);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    cardholder.CardHolder_ID = x.GetInt32(x.GetOrdinal("cardholder_id"));
                    cardholder.Platenum = x.GetString(x.GetOrdinal("platenum"));
                    cardholder.ParkerType_ID = x.GetInt32(x.GetOrdinal("member_type"));
                    cardholder.ParkerType_Name = x.GetString(x.GetOrdinal("parker_name"));
                    cardholder.AreaList = getAreaList(JsonConvert.DeserializeObject<List<int>>(x.GetString(x.GetOrdinal("area_id"))));
                }

                CloseConnection();

                return cardholder;
            }
            catch (Exception ex)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Account Error", BottomText = ex.Message });
                CloseConnection();
                return cardholder;
            }
        }

        #endregion 

        //Search Card Holder Record by Card Serial
        public static string SearchCardHolderID(string CardSerial)
        {
            string cardholderid = "";
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select cardholder_id from tbl_cardholder where cardserial = @cardserial";
                cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    cardholderid = x.GetString(x.GetOrdinal("cardholder_id"));
                }
                CloseConnection();

                if (cardholderid != "")
                {
                    return cardholderid;
                }
                else
                {

                    return cardholderid;
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search User Error", BottomText = ex.Message });
                return cardholderid;
            }
        }

        //Search User Record by Card Serial
        public static string SearchUserID(string CardSerial)
        {
            string userid = "";
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select userid from tbl_user where cardserial = @cardserial";
                cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    userid = x.GetString(x.GetOrdinal("userid"));
                }
                CloseConnection();

                if (userid != "")
                {
                    return userid;
                }
                else
                {

                    return userid;
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search User Error", BottomText = ex.Message });
                return userid;
            }
        }

        public static void DisableAccount()
        {
            if (GlobalVariables.CardData != null)
            {

                if (GlobalVariables.CardData.parkingData.CardHolder_ID != 0)
                    DisableCardHolder();

                CallToast(new ToastData { type = ToastType.Success, HeaderText = "Account Disabled", BottomText = "" });
            }
        }



        public static void DisableCardHolder()
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_cardholder SET card_status = 0 where cardholder_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", GlobalVariables.CardData.parkingData.CardHolder_ID);

                    cmd.ExecuteNonQuery();
                }

                CloseConnection();
            }
            catch (MySqlException exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Disable Error", BottomText = exc.Message });
                CloseConnection();
            }
        }

        public static void ReactivateAccount()
        {
            if (GlobalVariables.CardData != null)
            {

                if (GlobalVariables.CardData.parkingData.CardHolder_ID != 0)
                    ReactivateCardHolder();

                CallToast(new ToastData { type = ToastType.Success, HeaderText = "Account Reactivated", BottomText = "" });
            }
        }

        public static void ReactivateCardHolder()
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_cardholder SET card_status = 1 where cardholder_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", GlobalVariables.CardData.parkingData.CardHolder_ID);

                    cmd.ExecuteNonQuery();
                }

                CloseConnection();
            }
            catch (MySqlException exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Disable Error", BottomText = exc.Message });
                CloseConnection();
            }
        }

        public static List<ActionHistory> getHistoryLog()
        {
            List<ActionHistory> logs = new List<ActionHistory>();
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from tbl_auditlog as auditlog where auditlog.user_id = @userid";
                cmd.Parameters.AddWithValue("@userid", "");
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (x.Read())
                {
                    ActionHistory newlog = new ActionHistory();

                    newlog.ActionHistoryID = x.GetInt64(x.GetOrdinal("id"));
                    newlog.date = x.GetDateTime(x.GetOrdinal("date"));
                    newlog.Action = x.GetString(x.GetOrdinal("function"));
                    newlog.Description = x.GetString(x.GetOrdinal("description"));
                    newlog.User_ID = x.GetInt32(x.GetOrdinal("user_id"));
                    newlog.TermName = x.GetInt32(x.GetOrdinal("term_id"));

                    logs.Add(newlog);
                }
                CloseConnection();

                return logs;
            }
            catch (Exception exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Log Error", BottomText = exc.Message });
                CloseConnection();
                return logs;
            }
        }

        #region Transactions

        public static string SearchTicketNum(string CardSerial)
        {
            string ticketnum = "";
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select parkinfo.status, parkinfo.ticketnum from tbl_parkinfo as parkinfo where (parkinfo.cardserial = @cardserial or parkinfo.ticketnum = @cardserial2) and parkinfo.status = 1 order by date_modified DESC";
                cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                cmd.Parameters.AddWithValue("@cardserial2", CardSerial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    ticketnum = x.GetString(x.GetOrdinal("ticketnum"));
                }
                CloseConnection();

                if (ticketnum != "")
                {
                    return ticketnum;
                }
                else
                {
                    CallToast(new ToastData { type = ToastType.Error, HeaderText = "SEARCH FAILED", BottomText = "Transaction Not Found" });
                    return ticketnum;
                }
            }
            catch (Exception ex)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "SEARCH FAILED", BottomText = ex.Message });
                CloseConnection();
                return ticketnum;
            }
        }





        public static bool SearchTransactionEntry(string TicketID)
        {
            try
            {
                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT parkinfo.*, parkinfo.entry_termid as termid, terminal.term_name as entryterm_name, terminal2.term_name as paymentterm_name, terminal3.term_name as exitterm_name, parkinfo.succharge as succcharge, parkinfo.oncharge as oncharge, rate.rate_id, rate.member_type, rate.initcharge as rateinit, rate.succharge as ratesucc, rate.oncharge as rateon, rate.initcharge_hour, rate.succharge_hour, area.area_id, area.area_name, terminal.term_ip, cardholder.firstname as cardholder_firstname, cardholder.lastname as cardholder_lastname, cardholder.platenum as cardholder_platenum FROM tbl_parkinfo as parkinfo left join tbl_rate rate on rate.rate_id = parkinfo.ratetype left join tbl_area area on area.area_id = parkinfo.entryarea_id left join tbl_terminal terminal on terminal.term_id = parkinfo.entry_termid left join tbl_terminal terminal2 on terminal2.term_id = parkinfo.payment_termid left join tbl_terminal terminal3 on terminal3.term_id = parkinfo.exit_termid left join tbl_cardholder as cardholder on cardholder.cardholder_id = parkinfo.cardholder where parkinfo.ticketnum = @ticketnum and parkinfo.status = 1 order by park_id";
                cmd.Parameters.AddWithValue("@ticketnum", TicketID);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    GlobalVariables.TransactionData.TransactionID = x.GetInt32(x.GetOrdinal("park_id"));
                    GlobalVariables.TransactionData.TransactionTicketNum = x.GetString(x.GetOrdinal("ticketnum"));
                    GlobalVariables.TransactionData.EntryTermID = x.GetInt32(x.GetOrdinal("entry_termid"));
                    GlobalVariables.TransactionData.PlateNumber = x.GetString(x.GetOrdinal("platenum"));
                    GlobalVariables.TransactionData.TransactionEntryDate = x.GetString(x.GetOrdinal("entrydate"));
                    GlobalVariables.TransactionData.TransactionEntryAreaID = x.GetInt32(x.GetOrdinal("entryarea_id"));
                    GlobalVariables.TransactionData.TransactionAreaName = x.GetString(x.GetOrdinal("area_name"));
                    GlobalVariables.TransactionData.Charge = x.GetFloat(x.GetOrdinal("charge"));
                    GlobalVariables.TransactionData.InitCharge_Hour = x.GetInt32(x.GetOrdinal("initcharge_hour"));
                    GlobalVariables.TransactionData.SucCharge_Hour = x.GetInt32(x.GetOrdinal("succharge_hour"));
                    GlobalVariables.TransactionData.InitCharge = x.GetFloat(x.GetOrdinal("rateinit"));
                    GlobalVariables.TransactionData.SuccCharge = x.GetFloat(x.GetOrdinal("ratesucc"));
                    GlobalVariables.TransactionData.OvernightCharge = x.GetFloat(x.GetOrdinal("rateon"));
                    GlobalVariables.TransactionData.Terminal_IP = x.GetString(x.GetOrdinal("term_ip"));
                    GlobalVariables.TransactionData.parkerstatus = (ParkerStatus)x.GetInt32(x.GetOrdinal("process"));

                    GlobalVariables.TransactionData.VatAmount = x.GetFloat(x.GetOrdinal("vatAmount"));
                    GlobalVariables.TransactionData.VatExempt = x.GetFloat(x.GetOrdinal("vatExempt"));
                    GlobalVariables.TransactionData.RateID = x.GetInt32(x.GetOrdinal("ratetype"));

                    if (!(x.IsDBNull(x.GetOrdinal("cardserial"))))
                    {
                        GlobalVariables.TransactionData.CardSerial = x.GetString(x.GetOrdinal("cardserial"));
                    }

                    if (!(x.IsDBNull(x.GetOrdinal("vehicle_image"))))
                    {
                        GlobalVariables.TransactionData.VehicleImageLoc = x.GetString(x.GetOrdinal("vehicle_image"));
                    }

                    if (!(x.IsDBNull(x.GetOrdinal("driver_image"))))
                    {
                        GlobalVariables.TransactionData.ParkerImageLoc = x.GetString(x.GetOrdinal("driver_image"));
                    }

                    if (!x.IsDBNull(x.GetOrdinal("cardholder")))
                    {
                        GlobalVariables.TransactionData.CardHolderdata.CardHolderID = x.GetInt32(x.GetOrdinal("cardholder"));
                        if (!x.IsDBNull(x.GetOrdinal("cardholder_platenum")))
                            GlobalVariables.TransactionData.PlateNumber = x.GetString(x.GetOrdinal("cardholder_platenum"));
                        if (!x.IsDBNull(x.GetOrdinal("cardholder_firstname")))
                            GlobalVariables.TransactionData.CardHolderdata.Firstname = x.GetString(x.GetOrdinal("cardholder_firstname"));
                        if (!x.IsDBNull(x.GetOrdinal("cardholder_lastname")))
                            GlobalVariables.TransactionData.CardHolderdata.Lastname = x.GetString(x.GetOrdinal("cardholder_lastname"));
                    }

                    if (x.IsDBNull(x.GetOrdinal("paymentdate")))
                    {
                        GlobalVariables.TransactionData.TransactionPaid = false;
                    }

                    else if (!(x.IsDBNull(x.GetOrdinal("paymentdate"))))
                    {
                        GlobalVariables.TransactionData.TransactionPaid = true;
                        GlobalVariables.TransactionData.TellerID = x.GetInt32(x.GetOrdinal("tellerid"));
                        GlobalVariables.TransactionData.PaymentTermID = x.GetInt32(x.GetOrdinal("payment_termid"));
                        GlobalVariables.TransactionData.TransactionPaymentAreaID = x.GetInt32(x.GetOrdinal("paymentarea_id"));
                        GlobalVariables.TransactionData.PaymentTermName = x.GetString(x.GetOrdinal("paymentterm_name"));
                        GlobalVariables.TransactionData.TransactionPaymentDate = x.GetString(x.GetOrdinal("paymentdate"));
                        GlobalVariables.TransactionData.TransactionBusinessday = x.GetString(x.GetOrdinal("businessday"));
                        GlobalVariables.TransactionData.InitCharge = x.GetDouble(x.GetOrdinal("initcharge"));
                        GlobalVariables.TransactionData.InitChargeSub = x.GetFloat(x.GetOrdinal("initcharge"));
                        GlobalVariables.TransactionData.TotalSucCharge = x.GetFloat(x.GetOrdinal("succharge"));
                        GlobalVariables.TransactionData.ReceiptNum = x.GetString(x.GetOrdinal("receiptnum"));
                        GlobalVariables.TransactionData.CashTendered = x.GetFloat(x.GetOrdinal("cash_tendered"));
                        GlobalVariables.TransactionData.creditAmount = x.GetFloat(x.GetOrdinal("creditamount"));
                        GlobalVariables.TransactionData.offlineAmount = x.GetFloat(x.GetOrdinal("offlineamount"));
                        GlobalVariables.TransactionData.cashAmount = x.GetFloat(x.GetOrdinal("cashamount"));
                        GlobalVariables.TransactionData.TotalOvernightCharge = x.GetFloat(x.GetOrdinal("oncharge"));

                        if (!(x.IsDBNull(x.GetOrdinal("settlement_id"))))
                        {
                            GlobalVariables.TransactionData.SettlementID = x.GetInt32(x.GetOrdinal("settlement_id"));
                        }
                        if (!(x.IsDBNull(x.GetOrdinal("discount_deduct"))))
                        {
                            GlobalVariables.TransactionData.isDiscount = true;
                            GlobalVariables.TransactionData.SelectedDiscountDeductID = x.GetInt32(x.GetOrdinal("discount_deduct"));
                            GlobalVariables.TransactionData.DiscountDeductionAmount = x.GetDouble(x.GetOrdinal("discount_amount"));
                        }
                        if (!(x.IsDBNull(x.GetOrdinal("voucher_deduct"))))
                        {
                            GlobalVariables.TransactionData.isVoucher = true;
                            GlobalVariables.TransactionData.SelectedVoucherDeductID = x.GetInt32(x.GetOrdinal("voucher_deduct"));
                            GlobalVariables.TransactionData.VoucherDeductionAmount = x.GetDouble(x.GetOrdinal("voucher_amount"));
                        }
                        if ((x.IsDBNull(x.GetOrdinal("discount_deduct"))) && (x.IsDBNull(x.GetOrdinal("voucher_deduct"))))
                        {
                            GlobalVariables.TransactionData.isDiscount = GlobalVariables.TransactionData.isVoucher = false;
                        }

                        //if lost card
                        if (x.GetDouble(x.GetOrdinal("lostcharge")) != 0)
                        {
                            GlobalVariables.TransactionData.isLostCharge = true;
                            GlobalVariables.TransactionData.LostCharge = x.GetFloat(x.GetOrdinal("lostcharge"));
                        }
                        else
                        {
                            GlobalVariables.TransactionData.isLostCharge = false;
                            GlobalVariables.TransactionData.LostCharge = x.GetFloat(x.GetOrdinal("lostcharge"));
                        }

                        //if member
                        if (x.GetInt32(x.GetOrdinal("settlement_id")) == 2)
                        {
                            if (!(x.IsDBNull(x.GetOrdinal("documentno"))))
                            {
                                GlobalVariables.TransactionData.CardNo = x.GetString(x.GetOrdinal("cardno"));
                                GlobalVariables.TransactionData.DocumentNo = x.GetString(x.GetOrdinal("documentno"));
                                GlobalVariables.TransactionData.gpAmount = x.GetDouble(x.GetOrdinal("gpamount"));
                            }
                        }

                        //if offline payment
                        if (!(x.IsDBNull(x.GetOrdinal("documentno"))))
                        {
                            GlobalVariables.TransactionData.DocumentNo = x.GetString(x.GetOrdinal("documentno"));
                        }
                        if (!(x.IsDBNull(x.GetOrdinal("cardno"))))
                        {
                            GlobalVariables.TransactionData.CardNo = x.GetString(x.GetOrdinal("cardno"));
                        }


                        //if adjust
                        if (x.GetInt32(x.GetOrdinal("adjust")) == 1)
                        {
                            GlobalVariables.TransactionData.isAdjust = true;
                            GlobalVariables.TransactionData.InitCharge = x.GetDouble(x.GetOrdinal("rateinit"));
                            GlobalVariables.TransactionData.SuccCharge = x.GetDouble(x.GetOrdinal("ratesucc"));
                            GlobalVariables.TransactionData.cashAmount = 0;
                            GlobalVariables.TransactionData.gpAmount = 0;
                            GlobalVariables.TransactionData.creditAmount = 0;
                            GlobalVariables.TransactionData.offlineAmount = 0;
                        }
                        else
                        {
                            GlobalVariables.TransactionData.isAdjust = false;
                        }

                    }

                    if (!(x.IsDBNull(x.GetOrdinal("exit_termid"))))
                    {
                        GlobalVariables.TransactionData.ExitTermID = x.GetInt32(x.GetOrdinal("exit_termid"));
                        GlobalVariables.TransactionData.TransactionExitDate = x.GetString(x.GetOrdinal("exitdate"));
                        GlobalVariables.TransactionData.ExitTermName = x.GetString(x.GetOrdinal("exitterm_name"));
                    }
                    if (!(x.IsDBNull(x.GetOrdinal("remarks"))))
                    {
                        GlobalVariables.TransactionData.Remarks = x.GetString(x.GetOrdinal("remarks"));
                    }


                    CloseConnection();
                    //GlobalVariables.TransactionData.TellerName = GetTellerName(MainForm, GlobalVariables.TransactionData.TellerID);


                    return true;
                }
                else
                {

                    CloseConnection();
                    GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Search Error", BottomText = "Transaction not found" });
                    return false;
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Transaction Search Error: " + ex.Message);
                CloseConnection();
                return false;
            }
        }

        public static bool CheckOvertime()
        {


            DateTime serverTime = getServerTime();

            TimeSpan duration;
            TimeSpan computedDuration;
            int durationDays;
            double durationHours;
            int durationMins;
            TimeSpan allowedDuration;

            double allowedDurationHour;
            double computedHours;
            int computedMins;
            DateTime allowedTime;

            DateTime EntryDate = DateTime.ParseExact(GlobalVariables.TransactionData.TransactionEntryDate.Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            //StartTime = DateTime.ParseExact(GlobalVariables.TransactionEntryDate.Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            DateTime PaymentDate = GlobalVariables.TransactionData.TransactionPaid ? DateTime.ParseExact(GlobalVariables.TransactionData.TransactionPaymentDate.Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) : DateTime.ParseExact(GlobalVariables.TransactionData.TransactionPaymentDate = GlobalFunctions.GetTimeNow(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            //EndTime = GlobalVariables.TransactionPaid ? DateTime.ParseExact(GlobalVariables.TransactionPaymentDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) : DateTime.ParseExact(GlobalVariables.TransactionPaymentDate = GlobalFunctions.GetTimeNow(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            duration = PaymentDate.Subtract(EntryDate);
            durationHours = duration.TotalHours;
            durationDays = duration.Days;
            durationMins = duration.Minutes;

            allowedDuration = new TimeSpan(0, GlobalFunctions.getGraceMinuteExit(MainForm, GlobalVariables.TransactionData.TransactionEntryAreaID), 0);
            allowedTime = PaymentDate.Add(allowedDuration);

            if (serverTime > allowedTime)
            {
                GlobalVariables.TransactionData.TransactionEntryDate = allowedTime.ToString("yyyy-MM-dd HH:mm:ss");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int getGraceMinuteExit(Main MainForm, int AreaID)
        {
            int GraceMinute = 0;

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "select * from tbl_area where tbl_area.area_id = @areaid";
                cmd.Parameters.AddWithValue("@areaid", AreaID);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (x.Read())
                {
                    GraceMinute = x.GetInt32(x.GetOrdinal("gracemin_exit"));
                }
                CloseConnection();

                return GraceMinute;
            }
            catch (Exception exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Grace Min Error", BottomText = exc.Message });
                CloseConnection();
                return GraceMinute;
            }
        }

        public static string ComputeTimeRemaining()
        {
            string time = "";

            try
            {
                TimeSpan duration;
                TimeSpan allowedDuration;

                DateTime StartTime;
                DateTime EndTime;


                StartTime = DateTime.ParseExact(GlobalVariables.TransactionData.TransactionPaymentDate.Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                allowedDuration = new TimeSpan(0, GlobalFunctions.getGraceMinuteExit(MainForm, GlobalVariables.TransactionData.TransactionEntryAreaID), 0);
                EndTime = StartTime.Add(allowedDuration);

                duration = EndTime.Subtract(getServerTime());

                if(duration.Days <= 0)
                {
                    if(duration.Hours > 0)
                    {
                        time = duration.Hours + " Hours " + duration.Minutes + " Minutes";
                    }
                    else
                    {
                        time = duration.Minutes + " Minutes " + duration.Seconds + " Seconds";
                    }
                }
                else
                {
                    time = duration.Days + " Days " + duration.Hours + " Hours " + duration.Minutes + " Minutes";
                }

                return time;
            }catch(Exception exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Compute Time Error", BottomText = exc.Message });
                return time;
            }
           
        }

        public static string ComputeTimeElapsed()
        {
            string time = "";

            try
            {
                TimeSpan duration;
                TimeSpan allowedDuration;

                DateTime StartTime;
                DateTime EndTime;


                StartTime = DateTime.ParseExact(GlobalVariables.TransactionData.TransactionPaymentDate.Trim(), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                allowedDuration = new TimeSpan(0, GlobalFunctions.getGraceMinuteExit(MainForm, GlobalVariables.TransactionData.TransactionEntryAreaID), 0);
                EndTime = StartTime.Add(allowedDuration);

                duration = getServerTime().Subtract(EndTime);

                if (duration.Days <= 0)
                {
                    if (duration.Hours > 0)
                    {
                        time = duration.Hours + " Hours " + duration.Minutes + " Minutes";
                    }
                    else
                    {
                        time = duration.Minutes + " Minutes " + duration.Seconds + " Seconds";
                    }
                }
                else
                {
                    time = duration.Days + " Days" + duration.Hours + " Hours " + duration.Minutes + " Minutes";
                }

                return time;
            }
            catch (Exception exc)
            {
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Compute Time Error", BottomText = exc.Message });
                return time;
            }
        }

        public static void voidEntry(Terminal term)
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_parkinfo SET exitdate = NOW(), exit_termid = @exittermid, status = 0 where park_id = @parkid";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@parkid", GlobalVariables.TransactionData.TransactionID);
                    cmd.Parameters.AddWithValue("@exittermid", term.Terminal_ID);
                    cmd.ExecuteNonQuery();
                }
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Success, HeaderText = "Card Cleared", BottomText = "" });
                CloseConnection();

                
            }
            catch (MySqlException exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Card Clear Error", BottomText = exc.Message });
                CloseConnection();
            }
        }

        public static void exitEntry(Terminal term)
        {
            try
            {
                ParkerStatus process = GlobalVariables.TransactionData.CardHolderdata.CardHolderID == 0 ? ParkerStatus.Exit : ParkerStatus.RFID_Exit;

                OpenConnection();
                string query = "UPDATE tbl_parkinfo SET exitdate = NOW(), exit_termid = @exittermid, process = @process where park_id = @parkid";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@parkid", GlobalVariables.TransactionData.TransactionID);
                    cmd.Parameters.AddWithValue("@exittermid", term.Terminal_ID);
                    cmd.Parameters.AddWithValue("@process", process);
                    cmd.ExecuteNonQuery();
                }

                GlobalFunctions.CallToast(new ToastData { type = ToastType.Success, HeaderText = "Card Cleared", BottomText = "" });
                CloseConnection();
            }
            catch (MySqlException exc)
            {
                GlobalFunctions.CallToast(new ToastData { type = ToastType.Error, HeaderText = "Card Clear Error", BottomText = exc.Message });
                CloseConnection();
            }
        }


        public static string GetTimeNow()
        {
            string StrDateTime = "";
            try
            {

                OpenConnection();

                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT NOW() AS TimeNow";
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (x.Read())
                {
                    StrDateTime = x.GetDateTime(x.GetOrdinal("TimeNow")).ToString("yyyy-MM-dd HH:mm:ss");
                }
                CloseConnection();

                return StrDateTime;
            }
            catch (MySqlException)
            { CloseConnection(); return StrDateTime; }
        }

        #endregion 

        #region Records

        public static DataTable GetAccountRecords()
        {
            DataTable dtRecord = new DataTable();

            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                //cmd.CommandText = "SELECT list.userid, cardholder.cardholder_id, CONCAT(list.firstname, ' ', list.lastname) as name, list.cardserial, (CASE WHEN user.userlevel = 1 THEN 'Admin' WHEN user.userlevel = 2 THEN 'Cashier' WHEN user.userlevel = '3' THEN 'Transportation Manager' WHEN user.userlevel = '4' THEN 'Audit' END) as userlevel, cardholder.platenum, member.member_type from (SELECT user.userid, user.firstname, user.lastname, user.cardserial FROM `tbl_user` as user where user.status = 1 UNION select cardholder.userid, cardholder.firstname, cardholder.lastname, cardholder.cardserial from tbl_cardholder as cardholder where cardholder.card_status = 1) as list left join tbl_user as user on user.userid = list.userid left join tbl_cardholder as cardholder on cardholder.cardserial = list.cardserial left join tbl_member as member on member.member_id = cardholder.member_type";
                cmd.CommandText = "SELECT cardholder.cardholder_id, cardholder.cardserial, CONCAT(cardholder.firstname, ' ', cardholder.lastname) as name, cardholder.platenum, member.member_type, cardholder.card_status from tbl_cardholder as cardholder left join tbl_member as member on member.member_id = cardholder.member_type";
                cmd.Connection = connection;
                MySqlDataAdapter sqlDataAdap = new MySqlDataAdapter(cmd);

                sqlDataAdap.Fill(dtRecord);
                GlobalVariables.RecordsTable = dtRecord;
                CloseConnection();
                return dtRecord;
            }
            catch (Exception exc)
            {
                CloseConnection();
                GlobalFunctions.CallToast(new ToastData() { type = ToastType.Error, HeaderText = "Records Read Failed", BottomText = exc.Message });
                return dtRecord;
            }
        }
        #endregion

        #region Member

        public static bool validateCardSerial_Member(string CardSerial)
        {
            try
            {
                if (CardSerial != "")
                {
                    OpenConnection();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select cardserial from tbl_memberholder where member_status = 1 and cardserial = @cardserial";
                    cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                    cmd.Connection = connection;
                    MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (x.Read())
                    {
                        CloseConnection();
                        return false;
                    }
                    CloseConnection();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Validation Error", BottomText = ex.Message });
                return false;
            }
        }

        public static string validateCardSerial_Member_Update(string CardSerial)
        {
            try
            {
                if (CardSerial != "")
                {
                    OpenConnection();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select member_id, cardserial from tbl_memberholder where member_status = 1 and cardserial = @cardserial";
                    cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                    cmd.Connection = connection;
                    MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (x.Read())
                    {
                        string serial = x.GetString(x.GetOrdinal("cardserial"));
                        CloseConnection();
                        return serial;
                    }
                    CloseConnection();
                    return "";
                }
                return "";
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Validation Error", BottomText = ex.Message });
                return "";
            }
        }

        public static bool validateMagData_Member(string MagData)
        {
            try
            {
                if (MagData != "")
                {
                    OpenConnection();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select magdata from tbl_memberholder where member_status = 1 and magdata = @magdata";
                    cmd.Parameters.AddWithValue("@magdata", MagData);
                    cmd.Connection = connection;
                    MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (x.Read())
                    {
                        CloseConnection();
                        return false;
                    }
                    CloseConnection();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Validation Error", BottomText = ex.Message });
                return false;
            }
        }

        public static string validateMagData_Member_Update(string MagData)
        {
            try
            {
                if (MagData != "")
                {
                    OpenConnection();

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "select member_id, magdata from tbl_memberholder where member_status = 1 and magdata = @magdata";
                    cmd.Parameters.AddWithValue("@magdata", MagData);
                    cmd.Connection = connection;
                    MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    if (x.Read())
                    {
                        string serial = x.GetString(x.GetOrdinal("magdata"));
                        CloseConnection();
                        return serial;
                    }
                    CloseConnection();
                    return "";
                }
                return "";
            }
            catch (Exception ex)
            {
                CloseConnection();
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Validation Error", BottomText = ex.Message });
                return "";
            }
        }

        public static bool Member_New(Member data)
        {         
            try
            {
                OpenConnection();
                string query = "INSERT INTO tbl_memberholder(cardserial, magdata, created_by) VALUES (@cardserial,@magdata,@userid);";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@cardserial", data.CardSerial);
                    cmd.Parameters.AddWithValue("@magdata", data.MagData);
                    cmd.Parameters.AddWithValue("@userid", data.CreatedBy);                    
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
                return true;

            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Create New Member Error", ex.Message);

                CloseConnection();
                return false;
            }
        }

        public static Member Member_Search_CardSerial(string CardSerial)
        {
            GlobalVariables.MemberData = new Member();
            Member data = new Member();
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * from tbl_memberholder where cardserial = @cardserial";
                cmd.Parameters.AddWithValue("@cardserial", CardSerial);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (x.Read())
                {
                    data.MemberID = x.GetInt32(x.GetOrdinal("member_id"));
                    GlobalVariables.MemberData.MemberID = x.GetInt32(x.GetOrdinal("member_id"));
                    data.CardSerial = x.GetString(x.GetOrdinal("cardserial"));
                    GlobalVariables.MemberData.CardSerial = x.GetString(x.GetOrdinal("cardserial"));
                    data.MagData = x.GetString(x.GetOrdinal("magdata"));
                    GlobalVariables.MemberData.MagData = x.GetString(x.GetOrdinal("magdata"));
                    data.DateCreated = x.GetString(x.GetOrdinal("date_created"));
                    GlobalVariables.MemberData.DateCreated = x.GetString(x.GetOrdinal("date_created"));
                    data.CreatedBy = x.GetInt32(x.GetOrdinal("created_by"));
                    GlobalVariables.MemberData.CreatedBy = x.GetInt32(x.GetOrdinal("created_by"));

                    if (!(x.IsDBNull(x.GetOrdinal("date_last_modified"))))
                    {
                        data.Last_Modified = x.GetString(x.GetOrdinal("date_last_modified"));
                        GlobalVariables.MemberData.Last_Modified = x.GetString(x.GetOrdinal("date_last_modified"));
                    }
                    else
                    {
                        data.Last_Modified = "";
                        GlobalVariables.MemberData.Last_Modified = "";
                    }

                    if (!(x.IsDBNull(x.GetOrdinal("last_modified_by"))))
                    {
                        data.Last_Modified_By = x.GetInt32(x.GetOrdinal("last_modified_by"));
                        GlobalVariables.MemberData.Last_Modified_By = x.GetInt32(x.GetOrdinal("last_modified_by"));
                    }
                    else
                    {
                        data.Last_Modified_By = 0;
                        GlobalVariables.MemberData.Last_Modified_By = 0;
                    }
                    data.Member_Status = x.GetBoolean(x.GetOrdinal("member_status"));
                    GlobalVariables.MemberData.Member_Status = x.GetBoolean(x.GetOrdinal("member_status"));

                    CloseConnection();
                    return data;
                }
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "RFID Error", BottomText = "Card [" + CardSerial + "] does not exist" });
                CloseConnection();
                return data;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error");
                CloseConnection();
                return data;
            }

        }

        public static Member Member_Search_MagData(string MagData)
        {
            Member data = new Member();
            try
            {
                OpenConnection();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * from tbl_memberholder where magdata = @magdata";
                cmd.Parameters.AddWithValue("@magdata", MagData);
                cmd.Connection = connection;
                MySqlDataReader x = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                if (x.Read())
                {

                    data.CardSerial = x.GetString(x.GetOrdinal("cardserial"));
                    data.MagData = x.GetString(x.GetOrdinal("magdata"));
                    data.DateCreated = x.GetString(x.GetOrdinal("date_created"));
                    data.CreatedBy = x.GetInt32(x.GetOrdinal("created_by"));

                    if (!(x.IsDBNull(x.GetOrdinal("date_last_modified"))))
                        data.Last_Modified = x.GetString(x.GetOrdinal("date_last_modified"));
                    else
                        data.Last_Modified = "";

                    if (!(x.IsDBNull(x.GetOrdinal("last_modified_by"))))
                        data.Last_Modified_By = x.GetInt32(x.GetOrdinal("last_modified_by"));
                    else
                        data.Last_Modified_By = 0;

                    data.Member_Status = x.GetBoolean(x.GetOrdinal("member_status"));

                    CloseConnection();
                    return data;
                }
                CallToast(new ToastData { type = ToastType.Error, HeaderText = "Swipe Error", BottomText = "[" + MagData+ "] does not exist" });
                CloseConnection();
                return data;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error");
                CloseConnection();
                return data;
            }

        }

        public static bool UpdateMemberHolder(Member data)
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_memberholder SET cardserial = @cardserial, magdata = @magdata, date_last_modified = NOW(), last_modified_by = @userid where member_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", data.MemberID);
                    cmd.Parameters.AddWithValue("@cardserial", data.CardSerial);
                    cmd.Parameters.AddWithValue("@magdata", data.MagData);
                    cmd.Parameters.AddWithValue("@userid", GlobalVariables.AdminID);
                    cmd.ExecuteNonQuery();
                }
                
                CloseConnection();
                return true;
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
                return false;
            }

        }

        public static bool DisableMemberHolder(int ID)
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_memberholder SET member_status = 0, date_last_modified = NOW(), last_modified_by = @userid where member_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@userid", GlobalVariables.AdminID);
                    cmd.ExecuteNonQuery();
                }
                CallToast(new ToastData { type = ToastType.Success, HeaderText = "Successfully Deactivated", BottomText = "" });
               
                CloseConnection();
                return true;
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
                return false;
            }

        }

        public static bool EnableMemberHolder(int ID)
        {
            try
            {
                OpenConnection();
                string query = "UPDATE tbl_memberholder SET member_status = 1, date_last_modified = NOW(), last_modified_by = @userid where member_id = @id;";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", ID);
                    cmd.Parameters.AddWithValue("@userid", GlobalVariables.AdminID);
                    cmd.ExecuteNonQuery();
                }
                CallToast(new ToastData { type = ToastType.Success, HeaderText = "Successfully Re-Activated", BottomText = "" });
                CloseConnection();
                return true;
            }
            catch (MySqlException exc)
            {
                MessageBox.Show(exc.Message);
                CloseConnection();
                return false;
            }

        }
        #endregion
        public static void CreateAuditLog(AuditLogActions action, int userid)
        {

            GlobalVariables.AuditLog = new AuditLog();
            GlobalVariables.AuditLog.auditAction = action;

            try
            {
                OpenConnection();
                string query = "INSERT INTO tbl_auditlog(date, function, description, user_id, term_id) VALUES (NOW(),@function,@action,@userid,@termid);";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@function", string.Concat(GlobalVariables.AuditLog.auditAction.ToString().Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' '));
                    cmd.Parameters.AddWithValue("@action", GlobalVariables.AuditLog.ActionString(GlobalVariables.AuditLog));
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@termid", GlobalVariables.TerminalID);
                    cmd.ExecuteNonQuery();
                }
                CloseConnection();
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("Audit Log Error", ex.Message);
                
                CloseConnection();
            }
        }
    }
}
