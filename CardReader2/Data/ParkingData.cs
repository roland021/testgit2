using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader2
{
    public class ParkerType
    {
        public int Member_ID { get; set; }
        public string Member_Type { get; set; }
        public string Member_Code = "";
        public bool MemberStatus = false;
    }

    public class Area
    {
        public int Area_ID { get; set; }

        [JsonIgnore]
        public string Area_Name { get; set; }
        [JsonIgnore]
        public string Area_Code = "";
        [JsonIgnore]
        public bool AreaStatus = false;
    }

    public class Terminal
    {
        public int Terminal_ID { get; set; }
        public string Terminal_Name { get; set; }
        public TerminalType Terminal_Type { get; set; }        
        public Area Terminal_Area { get; set; }
        public bool Terminal_Status { get; set; }
    }

    public class Member
    {
        public int MemberID { get; set; }
        public string CardSerial { get; set; }
        public string MagData { get; set; }
        public string DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedBy_Name { get; set; }
        public string Last_Modified { get; set; }
        public int Last_Modified_By { get; set; }
        public string Last_Modified_By_Name { get; set; }
        public bool Member_Status { get; set; }
    }
    public enum TerminalType
    {
        [Description("Entry")]
        Entry,
        [Description("Payment")]
        Payment,
        [Description("Exit")]
        Exit,
        [Description("Admin")]
        Admin
    }
}
