using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader2
{
    class GlobalVariables
    {

        #region Terminal Config

        static int _TerminalID;
        public static int TerminalID
        {
            get { return _TerminalID; }
            set { _TerminalID = value; }
        }

        static string _TerminalName;
        public static string TerminalName
        {
            get { return _TerminalName; }
            set { _TerminalName = value; }
        }
        static string _ServerUsername;
        public static string ServerUsername
        {
            get { return _ServerUsername; }
            set { _ServerUsername = value; }
        }

        static string _ServerPassword;
        public static string ServerPassword
        {
            get { return _ServerPassword; }
            set { _ServerPassword = value; }
        }

        static string _ServerDatabaseName;
        public static string ServerDatabaseName
        {
            get { return _ServerDatabaseName; }
            set { _ServerDatabaseName = value; }
        }

        static string _Server;
        public static string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }

        static string _ServerPort;
        public static string ServerPort
        {
            get { return _ServerPort; }
            set { _ServerPort = value; }
        }

        static int _AdminID;
        public static int AdminID
        {
            get { return _AdminID; }
            set { _AdminID = value; }
        }

        static string _AdminUsername;
        public static string AdminUsername
        {
            get { return _AdminUsername; }
            set { _AdminUsername = value; }
        }

        static string _AdminPassword;
        public static string AdminPassword
        {
            get { return _AdminPassword; }
            set { _AdminPassword = value; }
        }

        static string _AdminName;
        public static string AdminName
        {
            get { return _AdminName; }
            set { _AdminName = value; }
        }

        static int _AdminType;
        public static int AdminType
        {
            get { return _AdminType; }
            set { _AdminType = value; }
        }

        static string _ftp_IP;
        public static string FTP_IP
        {
            get { return _ftp_IP; }
            set { _ftp_IP = value; }
        }

        static string _grandWing_ImageLoc;
        public static string GrandWing_ImageLoc
        {
            get { return _grandWing_ImageLoc; }
            set { _grandWing_ImageLoc = value; }
        }


        static SystemView _systemView;
        public static SystemView SystemView
        {
            get { return _systemView; }
            set { _systemView = value; }
        }

        public static AuditLog _auditLog;
        public static AuditLog AuditLog
        {
            get { return _auditLog; }
            set { _auditLog = value; }
        }

        #endregion

        #region Account 

        static CardHolder _CardData;
        public static CardHolder CardData
        {
            get { return _CardData; }
            set { _CardData = value; }
        }

        static TransactionData _TransactionData;
        public static TransactionData TransactionData
        {
            get { return _TransactionData; }
            set { _TransactionData = value; }
        }

        static List<Area> _AreaList;
        public static List<Area> AreaList
        {
            get { return _AreaList; }
            set { _AreaList = value; }
        }

        static List<ParkerType> _ParkerTypeList;
        public static List<ParkerType> ParkerTypeList
        {
            get { return _ParkerTypeList; }
            set { _ParkerTypeList = value; }
        }

        #endregion

        #region Records

        static DataTable _recordsTable;
        public static DataTable RecordsTable
        {
            get { return _recordsTable; }
            set { _recordsTable = value; }
        }

        static List<Terminal> _TerminalList;
        public static List<Terminal> TerminalList
        {
            get { return _TerminalList; }
            set { _TerminalList = value; }
        }


        #endregion

        #region Member

        static Member _MemberData;
        public static Member MemberData
        {
            get { return _MemberData; }
            set { _MemberData = value; }
        }

        #endregion
    }

    public class ListBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class ToastData
    {
        public ToastType type;
        public string HeaderText = "";
        public string BottomText = "";
    }

    public enum ToastType
    {
        Success,
        Caution,
        Error
    }

    public enum SystemView
    {
        AdminLogin,
        DisplayAccount,
        NewAccount,
        UpdateAccount,
        MemberMain,
        MemberNew,
        MemberUpdate,
        ClearData
    }


}
