using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader2
{ 
    public class CardHolder
    {
        //General Information      
        public string Firstname = "";
        public string Lastname = "";
        public string CardSerial = "";
        public DateTime DateCreated = new DateTime();

        //Parking Information
        public ParkingData parkingData = new ParkingData();

        public DateTime ValidUntil;
        public bool Status = false;
    }

    public class ParkingData
    {
        public int CardHolder_ID = 0;
        public string Platenum = "";
        public int ParkerType_ID = 0;
        public string ParkerType_Name = "";
        public List<Area> AreaList;
        public bool CardHolderStatus = false;
    }

    public class ActionHistory
    {
        public Int64 ActionHistoryID = 0;
        public DateTime date = new DateTime();
        public string Action = "";
        public string Description = "";
        public int User_ID = 0;
        public int TermName = 0;
    }

    public enum CardReaderStatus
    {
        No_Card_Detected,
        Valid_Card_Detected,
        Card_Initialized,
        Card_In_Use,
        Invalid_Card,
        Card_Expired,
        Deactivated,
        Empty_Card
    }


}
