using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader2
{
    public class AuditLog
    {
        public AuditLogActions auditAction;
        public string Action = "";
        public string UserID = GlobalVariables.AdminID.ToString();
        public string Username = GlobalVariables.AdminName.ToString();
        public string AdminId = GlobalVariables.AdminID.ToString();
        public string AdminName = GlobalVariables.AdminName;
        public string TransactionNumber = GlobalVariables.TransactionData.TransactionTicketNum;
        public string TerminalName = GlobalVariables.TerminalName;
        public string TerminalID = GlobalVariables.TerminalID.ToString();

        public string ActionString(AuditLog auditlog)
        {
            switch (auditlog.auditAction)
            {
                case AuditLogActions.CashierLogin:
                    return "Cashier [" + auditlog.UserID + "]" + auditlog.Username + " Logged in at Terminal [" + auditlog.TerminalID + "] " + auditlog.TerminalName;
                case AuditLogActions.CreateEntry:
                    return "Entry Transaction Created for Park ID # [" + auditlog.TransactionNumber + "]";
                case AuditLogActions.SearchTransaction:
                    return "Transaction Record Number [" + auditlog.TransactionNumber + "] Searched by [" + auditlog.UserID + "] " + auditlog.Username;
                case AuditLogActions.PaidTransaction:
                    return "Transaction Record Number [" + auditlog.TransactionNumber + "] Paid by [" + auditlog.UserID + "] " + auditlog.Username;
                case AuditLogActions.AdminLogin:
                    return "Admin [" + auditlog.AdminId + "] " + auditlog.AdminName + " Logged in at Terminal [" + auditlog.TerminalID + "] " + auditlog.TerminalName;
                case AuditLogActions.ReconfiguredSettings:
                    return "Reconfigured Terminal for [" + auditlog.TerminalID + "] " + auditlog.TerminalID + " Settings Detected by [" + auditlog.AdminId + "] " + auditlog.AdminName;
                case AuditLogActions.VoidedTransaction:
                    return "Voided Record with Transaction Number: [" + auditlog.TransactionNumber + "] by Authority of [" + auditlog.AdminId + "] " + auditlog.AdminName;
                case AuditLogActions.AdjustedTransaction:
                    return "Adjusted Record for Transaction Number: [" + auditlog.TransactionNumber + "] by Authority of [" + auditlog.AdminId + "] " + auditlog.AdminName;
                case AuditLogActions.XReading:
                    return "X Reading printed by [" + auditlog.AdminId + "] " + auditlog.AdminName;
                case AuditLogActions.ZReading:
                    return "Z Reading printed by [" + auditlog.AdminId + "] " + auditlog.AdminName;
                case AuditLogActions.AdminLogout:
                    return "Admin [" + auditlog.AdminId + "] " + auditlog.AdminName + " Logged out at Terminal [" + auditlog.TerminalID + "] " + auditlog.TerminalName;
                case AuditLogActions.CashierLogout:
                    return "Cashier [" + auditlog.UserID + "]" + auditlog.Username + " Logged out at Terminal [" + auditlog.TerminalID + "] " + auditlog.TerminalName;
                default:
                    return "";
            }
        }

        public static string ActionPrefix(AuditLogActions action)
        {
            switch (action)
            {
                case AuditLogActions.CashierLogin:
                    return "Cashier Logged In";
                case AuditLogActions.CreateEntry:
                    return "Entry Transaction Created";
                case AuditLogActions.SearchTransaction:
                    return "Transaction Record Searched";
                case AuditLogActions.PaidTransaction:
                    return "Transaction Record Paid";
                case AuditLogActions.AdminLogin:
                    return "Admin Logged in";
                case AuditLogActions.ReconfiguredSettings:
                    return "Reconfigured Terminal Settings Detected";
                case AuditLogActions.ReprintTransaction:
                    return "Transaction Reprint";
                case AuditLogActions.AdminLogout:
                    return "Admin Logged out";
                case AuditLogActions.CashierLogout:
                    return "Cashier Logged out";
                default:
                    return "";
            }
        }
    }

    public enum AuditLogActions
    {
        CashierLogin,
        CreateEntry,
        SearchTransaction,
        PaidTransaction,
        AdminLogin,
        ReconfiguredSettings,
        VoidedTransaction,
        AdjustedTransaction,
        ReprintTransaction,
        XReading,
        ZReading,
        AdminLogout,
        CashierLogout
    }
}
