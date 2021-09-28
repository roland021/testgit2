using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader2
{
    public class TransactionData
    {
        public int TransactionID = 0;
        public string TransactionTicketNum = "";
        public int EntryTermID = 0;
        public int PaymentTermID = 0;
        public string PaymentTermName = "";
        public int ExitTermID = 0;
        public string ExitTermName = "";
        public int TellerID = 0;
        public string TellerName = "";
        public string PlateNumber = "";
        public string ReceiptNum = "";
        public int TransactionEntryAreaID = 0;
        public int TransactionPaymentAreaID = 0;
        public string TransactionEntryDate = "";
        public string TransactionPaymentDate = "";
        public string TransactionExitDate = "";
        public string TransactionBusinessday = "";
        public string TransactionAreaName = "";

        public Boolean TransactionPaid = false;
        public int RateID = 0;
        public int SettlementID = 0;
        public int InitCharge_Hour = 0;
        public int SucCharge_Hour = 0;
        public double Charge;
        public double InitCharge;
        public double InitChargeSub;
        public double SuccCharge;
        public double TotalSucCharge;
        public bool isLostCharge;
        public double LostCharge;
        public string LostCardSerial = "";
        public double OvernightCharge;
        public double TotalOvernightCharge;
        public double VatAmount;
        public double VatExempt;
        public double cashAmount;
        public double gpAmount;
        public double lpAmount;
        public double offlineAmount;
        public double creditAmount;
        public double CashTendered;
        public int SelectedDiscountDeductID = 0;
        public int SelectedVoucherDeductID = 0;
        public double DiscountDeductionAmount;
        public double VoucherDeductionAmount;
        public string CardNo = "";
        public string DocumentNo = "";
        public string CardSerial = "";
        public string Remarks = "";
        public CardHolderData CardHolderdata = new CardHolderData();
        public ParkerStatus parkerstatus;
        public Boolean isAdjust = false;
        public bool IsOvertime = false;
        public bool isGraceMin = false;
        public Boolean isDiscount = false;
        public Boolean isVoucher = false;


        //Phase 2 Data
        public string VehicleImageLoc = "";
        public string ParkerImageLoc = "";
        public Bitmap EntryCar;
        public Bitmap EntryDriver;
        public Bitmap ExitCar;
        public Bitmap ExitDriver;
        public List<Bitmap> VehicleImages = new List<Bitmap>();
        public string Terminal_IP = "";
        public string VehicleStatus = "";
    }

    public class MemberData
    {
        public int MemberID = 0;
        public int RateID = 0;
        public int initcharge_hour = 0;
        public double initcharge;
        public double succharge;
        public double oncharge;
        public double lostcharge;

        public string MemberName = "";
    }

    public class CardHolderData
    {
        public int CardHolderID = 0;
        public int MemberTypeID = 0;
        public string CardSerial = "";
        public string Firstname = "";
        public string Lastname = "";
        public string MemberTypeName = "";
        public DateTime CardValidity;
    }

    public enum ParkerStatus
    {
        Entry = 0,
        Paid = 1,
        Exit = 2,
        Initial_Overtime = 3,
        Overtime_Paid = 4,
        Overtime_Exit = 5,
        RFID_Entry = 6,
        RFID_Paid = 7,
        RFID_Exit = 8,
        RFID_Initial_Overtime = 9,
        Overtime_RFID_Paid = 10,
        Overtime_RFID_Exit = 11
    }
}
