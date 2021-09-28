using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader2
{
    class CardReader
    {
        #region ACR122U
        public static int retCode, hContext, hCard, Protocol;
        public static bool connActive = false;
        public static bool validATS;
        public static bool autoDet;
        public static byte[] SendBuff = new byte[263];
        public static byte[] RecvBuff = new byte[263];
        public static int SendLen, RecvLen, nBytesRet, reqType, Aprotocol, dwProtocol, cbPciLength;
        public static ModWinsCard.SCARD_READERSTATE RdrState;
        public static ModWinsCard.SCARD_IO_REQUEST pioSendRequest;
        public static List<Tuple<byte[], string, string>> RecvBlock = new List<Tuple<byte[], string, string>>();

        public string errormessage = "";
        //public bool startmessage = true;
        static byte[] ATRVal = new byte[257];
        public static int ATRLen;
        public bool detect;
        public static int dwState, dwActProtocol;
        public static string Desfirename = "ACS ACR1281 1S Dual Reader PICC 0";
        //public string Desfirename = "ACS ACR1222 3S PICC Reader PICC 0";


        private static void ClearBuffers()
        {
            long indx;

            for (indx = 0; indx <= 262; indx++)
            {
                RecvBuff[indx] = 0;
                SendBuff[indx] = 0;
            }
        }

        public static void ACR122Init()
        {

            string ReaderList = "" + Convert.ToChar(0);
            int indx;
            int pcchReaders = 0;
            string rName = "";

            //Establish Context
            retCode = ModWinsCard.SCardEstablishContext(ModWinsCard.SCARD_SCOPE_USER, 0, 0, ref hContext);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                return;
            }

            // 2. List PC/SC card readers installed in the system

            retCode = ModWinsCard.SCardListReaders(hContext, null, null, ref pcchReaders);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                return;
            }

            byte[] ReadersList = new byte[pcchReaders];


            // Fill reader list
            retCode = ModWinsCard.SCardListReaders(hContext, null, ReadersList, ref pcchReaders);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                ////////
                Console.WriteLine("SCardListReaders Error: " + ModWinsCard.GetScardErrMsg(retCode));
                return;
            }
            else
            {
            }

            rName = "";
            indx = 0;

            //Convert reader buffer to string
            while (ReadersList[indx] != 0)
            {

                while (ReadersList[indx] != 0)
                {
                    rName = rName + (char)ReadersList[indx];
                    indx = indx + 1;
                }

                //Add reader name to list
                rName = "";
                indx = indx + 1;
            }
        }

        private static int SendAPDUandDisplay(int reqType)
        {

            int indx;
            string tmpStr;

            pioSendRequest.dwProtocol = Aprotocol;
            pioSendRequest.cbPciLength = 8;

            // Display Apdu In
            tmpStr = "";
            for (indx = 0; indx <= SendLen - 1; indx++)
            {
                tmpStr = tmpStr + " " + string.Format("{0:X2}", SendBuff[indx]);
            }

            retCode = ModWinsCard.SCardTransmit(hCard, ref pioSendRequest, ref SendBuff[0], SendLen, ref pioSendRequest, ref RecvBuff[0], ref RecvLen);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                return retCode;
            }

            else
            {

                tmpStr = "";
                switch (reqType)
                {

                    case 0:
                        for (indx = (RecvLen - 2); indx <= (RecvLen - 1); indx++)
                        {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        if ((tmpStr).Trim() != "90 00")
                        {
                            //err
                        }

                        break;

                    case 1:

                        for (indx = (RecvLen - 2); indx <= (RecvLen - 1); indx++)
                        {
                            tmpStr = tmpStr + string.Format("{0:X2}", RecvBuff[indx]);
                        }


                        if (tmpStr.Trim() != "90 00")
                        {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        else
                        {
                            tmpStr = "ATR : ";
                            for (indx = 0; indx <= (RecvLen - 3); indx++)
                            {
                                tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                            }
                        }

                        break;

                    case 2:

                        for (indx = 0; indx <= (RecvLen - 1); indx++)
                        {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }

                        break;

                    case 3:

                        for (indx = (RecvLen - 2); indx <= (RecvLen - 1); indx++)
                        {
                            tmpStr = tmpStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
                        }


                        if (tmpStr.Trim() == "6A 81")
                        {
                            //displayOut(4, 0, "The function is not supported.");
                            return retCode;
                        }


                        if (tmpStr.Trim() == "63 00")
                        {
                            //displayOut(4, 0, "The operation failed.");
                            return retCode;
                        }
                        validATS = true;
                        break;

                }

                //displayOut(3, 0, tmpStr.Trim());

            }
            return retCode;

        }

        bool ReadCardEnabled;
        public static string ACRReadData()
        {
            try
            {
                string tmpStr = "";
                int indx;


                validATS = false;
                ClearBuffers();
                SendBuff[0] = 0xFF;
                SendBuff[1] = 0xCA;
                SendBuff[2] = 0x00;

                SendBuff[3] = 0x00;
                SendBuff[4] = 0x00;

                SendLen = SendBuff[4] + 5;
                RecvLen = 0xFF;

                retCode = ModWinsCard.SCardConnect(hContext, Desfirename.ToString(), ModWinsCard.SCARD_SHARE_SHARED,
                                      ModWinsCard.SCARD_PROTOCOL_T0 | ModWinsCard.SCARD_PROTOCOL_T1, ref hCard, ref Protocol);

                retCode = SendAPDUandDisplay(3);

                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                {
                    return tmpStr;
                }

                // Interpret and display return values
                if (validATS)
                {
                    tmpStr = "";
                    for (indx = 0; indx <= (RecvLen - 3); indx++)
                    {
                        tmpStr = tmpStr + string.Format("{0:X2}", RecvBuff[indx]);
                    }
                }

                return tmpStr;
            }
            catch
            {
                return "";
            }
        }

        public static Boolean ACR122Pooling()
        {
            try
            {
                retCode = CardConnect(1);

                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                {
                    //////////////////
                    //if (boolClear == true)
                    //{
                    //    ClearText();
                    //}                                       
                    return false;
                }

                if (CheckCard())
                {
                    return true;
                }
                else
                {
                    return false;
                    /////////////
                    //  ClearText();
                }
            }
            catch { return false; }
        }

        private static int CardConnect(int connType)
        {
            //bool functionReturnValue = false;

            if (connActive)
            {
                retCode = ModWinsCard.SCardDisconnect(hCard, ModWinsCard.SCARD_UNPOWER_CARD);
            }

            //Connect
            retCode = ModWinsCard.SCardConnect(hContext, Desfirename.ToString(), ModWinsCard.SCARD_SHARE_SHARED, ModWinsCard.SCARD_PROTOCOL_T0 | ModWinsCard.SCARD_PROTOCOL_T1, ref hCard, ref Protocol);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                if (connType != 1)
                {
                    //displayOut(1, retCode, "");
                }
                connActive = false;
                return retCode;
            }

            else
            {
                if (connType != 1)
                {
                    //displayOut(0, 0, "Successful connection to " + cbReader.Text);
                }

                //functionReturnValue = retCode;

            }
            return retCode;
        }

        private static bool CheckCard()
        {

            bool functionReturnValue = false;

            //Variable declaration
            int ReaderLen = 0;
            long tmpWord;

            tmpWord = 32;
            ATRLen = Convert.ToInt32(tmpWord);

            retCode = ModWinsCard.SCardStatus(hCard, Desfirename, ref ReaderLen, ref dwState, ref dwActProtocol, ref ATRVal[0], ref ATRLen);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                //Call DisplayOut(1, retCode, "")
                functionReturnValue = false;
                return functionReturnValue;
            }

            else
            {
                InterpretATR();
                functionReturnValue = true;
            }
            return functionReturnValue;

        }


        private static void InterpretATR()
        {

            string RIDVal, cardName, sATRStr, lATRStr, tmpVal;
            int indx, indx2;

            //4. Interpret ATR and guess card
            // 4.1. Mifare cards using ISO 14443 Part 3 Supplemental Document
            if ((int)(ATRLen) > 14)
            {

                RIDVal = "";
                sATRStr = "";
                lATRStr = "";

                for (indx = 7; indx <= 11; indx++)
                {
                    RIDVal = RIDVal + " " + string.Format("{0:X2}", ATRVal[indx]);
                }


                for (indx = 0; indx <= 4; indx++)
                {

                    //shift bit to right
                    tmpVal = ATRVal[indx].ToString();

                    for (indx2 = 1; indx2 <= 4; indx2++)
                    {
                        tmpVal = Convert.ToString(Convert.ToInt32(tmpVal) / 2);
                    }

                    if (((indx == '1') & (tmpVal == "8")))
                    {
                        lATRStr = lATRStr + "8X";
                        sATRStr = sATRStr + "8X";
                    }

                    else
                    {
                        if (indx == 4)
                        {
                            lATRStr = lATRStr + " " + string.Format("{0:X2}", ATRVal[indx]);
                        }

                        else
                        {
                            lATRStr = lATRStr + " " + string.Format("{0:X2}", ATRVal[indx]);
                            sATRStr = sATRStr + " " + string.Format("{0:X2}", ATRVal[indx]);
                        }
                    }
                }

                cardName = "";

                // Felica and Topaz Cards
                if (ATRVal[12] == 0x03)
                {
                    if (ATRVal[13] == 0xF0)
                    {
                        switch (ATRVal[14])
                        {
                            case 0x11:
                                cardName = " FeliCa 212K";
                                break;
                            case 0x12:
                                cardName = " Felica 424K";
                                break;
                            case 0x04:
                                cardName = " Topaz";
                                break;

                        }

                    }
                }


                if (ATRVal[12] == 0x03)
                {

                    if (ATRVal[13] == 0x00)
                    {

                        switch (ATRVal[14])
                        {

                            case 0x01:
                                cardName = cardName + " Mifare Standard 1K";
                                break;
                            case 0x02:
                                cardName = cardName + " Mifare Standard 4K";
                                break;
                            case 0x03:
                                cardName = cardName + " Mifare Ultra light";
                                break;
                            case 0x04:
                                cardName = cardName + " SLE55R_XXXX";
                                break;
                            case 0x06:
                                cardName = cardName + " SR176";
                                break;
                            case 0x07:
                                cardName = cardName + " SRI X4K";
                                break;
                            case 0x08:
                                cardName = cardName + " AT88RF020";
                                break;
                            case 0x09:
                                cardName = cardName + " AT88SC0204CRF";
                                break;
                            case 0x0A:
                                cardName = cardName + " AT88SC0808CRF";
                                break;
                            case 0x0B:
                                cardName = cardName + " AT88SC1616CRF";
                                break;
                            case 0x0C:
                                cardName = cardName + " AT88SC3216CRF";
                                break;
                            case 0x0D:
                                cardName = cardName + " AT88SC6416CRF";
                                break;
                            case 0x0E:
                                cardName = cardName + " SRF55V10P";
                                break;
                            case 0xF:
                                cardName = cardName + " SRF55V02P";
                                break;
                            case 0x10:
                                cardName = cardName + " SRF55V10S";
                                break;
                            case 0x11:
                                cardName = cardName + " SRF55V02S";
                                break;
                            case 0x12:
                                cardName = cardName + " TAG IT";
                                break;
                            case 0x13:
                                cardName = cardName + " LRI512";
                                break;
                            case 0x14:
                                cardName = cardName + " ICODESLI";
                                break;
                            case 0x15:
                                cardName = cardName + " TEMPSENS";
                                break;
                            case 0x16:
                                cardName = cardName + " I.CODE1";
                                break;
                            case 0x17:
                                cardName = cardName + " PicoPass 2K";
                                break;
                            case 0x18:
                                cardName = cardName + " PicoPass 2KS";
                                break;
                            case 0x19:
                                cardName = cardName + " PicoPass 16K";
                                break;
                            case 0x1A:
                                cardName = cardName + " PicoPass 16KS";
                                break;
                            case 0x1B:
                                cardName = cardName + " PicoPass 16K(8x2)";
                                break;
                            case 0x1C:
                                cardName = cardName + " PicoPass 16KS(8x2)";
                                break;

                            case 0x1D:
                                cardName = cardName + " PicoPass 32KS(16+16)";
                                break;
                            case 0x1E:
                                cardName = cardName + " PicoPass 32KS(16+8x2)";
                                break;
                            case 0x1F:
                                cardName = cardName + " PicoPass 32KS(8x2+16)";
                                break;
                            case 0x20:
                                cardName = cardName + " PicoPass 32KS(8x2+8x2)";
                                break;
                            case 0x21:
                                cardName = cardName + " LRI64";
                                break;
                            case 0x22:
                                cardName = cardName + " I.CODE UID";
                                break;
                            case 0x23:
                                cardName = cardName + " I.CODE EPC";
                                break;
                            case 0x24:
                                cardName = cardName + " LRI12";
                                break;
                            case 0x25:
                                cardName = cardName + " LRI128";
                                break;
                            case 0x26:
                                cardName = cardName + " Mifare Mini";
                                break;

                        }
                    }

                    else
                    {

                        if (ATRVal[13] == 0xFF)
                        {

                            switch (ATRVal[14])
                            {

                                case 0x09:
                                    cardName = cardName + " Mifare Mini";
                                    break;

                            }
                        }
                    }
                    //displayOut(6, 0, cardName);
                }
            }

            //4.2. Mifare DESFire card using ISO 14443 Part 4
            if ((int)ATRLen == 11)
            {

                RIDVal = "";

                for (indx = 4; indx <= 9; indx++)
                {
                    RIDVal = RIDVal + " " + string.Format("{0:X2}", ATRVal[indx]);
                }

                if (RIDVal == " 06 75 77 81 02 80")
                {
                    //displayOut(6, 0, "Mifare DESFire");
                }
            }

            //4.3. Other cards using ISO 14443 Part 4
            if ((int)ATRLen == 17)
            {

                RIDVal = "";

                for (indx = 4; indx <= 15; indx++)
                {
                    RIDVal = RIDVal + " " + string.Format("{0:X2}", RecvBuff[indx]);
                }

                if (RIDVal == "50122345561253544E3381C3")
                {
                    //displayOut(6, 0, "ST19XRC8E");
                }
            }

            //4.4. other cards using ISO 14443 Type A or B
            lATRStr = "";
            sATRStr = "";

            if (lATRStr == "3B8X800150")
            {
                //displayOut(6, 0, "ISO 14443B ");
            }

            else
            {
                if (sATRStr == "3B8X8001")
                {
                    //displayOut(6, 0, "ISO 14443A");
                }
            }
        }

        #endregion

        #region APDU Commands


        public static string verifyCard(String Block)
        {
            string value = "";

            if (ACRReadData() != "")
            {
                value = readBlock(Block);
            }
            value = value.Split(new char[] { '\0' }, 2, StringSplitOptions.None)[0].ToString();

            return value;
        }

        public static string readBlock(String Block)
        {
            string tmpStr = "";
            int indx;

            if (authenticateBlock(Block))
            {
                ClearBuffers();
                SendBuff[0] = 0xFF; // CLA 
                SendBuff[1] = 0xB0;// INS
                SendBuff[2] = 0x00;// P1
                SendBuff[3] = (byte)int.Parse(Block);// P2 : Block No.
                SendBuff[4] = (byte)int.Parse("16");// Le

                SendLen = 5;
                RecvLen = SendBuff[4] + 2;

                retCode = SendAPDUandDisplay(2);

                if (retCode == -200)
                {
                    return "outofrangeexception";
                }

                if (retCode == -202)
                {
                    return "BytesNotAcceptable";
                }

                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                {
                    return "FailRead";
                }

                // Display data in text format
                for (indx = 0; indx <= RecvLen - 1; indx++)
                {
                    tmpStr = tmpStr + Convert.ToChar(RecvBuff[indx]);
                }
                decodeData();

                return (tmpStr);
            }
            else
            {
                return "FailAuthentication";
            }
        }

        private static bool authenticateBlock(String block)
        {
            ClearBuffers();
            SendBuff[0] = 0xFF;                         // CLA
            SendBuff[1] = 0x86;                         // INS: for stored key input
            SendBuff[2] = 0x00;                         // P1: same for all source types 
            SendBuff[3] = 0x00;                         // P2 : Memory location;  P2: for stored key input
            SendBuff[4] = 0x05;                         // P3: for stored key input
            SendBuff[5] = 0x01;                         // Byte 1: version number
            SendBuff[6] = 0x00;                         // Byte 2
            SendBuff[7] = (byte)int.Parse(block);       // Byte 3: sectore no. for stored key input
            SendBuff[8] = 0x60;                         // Byte 4 : Key A for stored key input
            SendBuff[9] = (byte)int.Parse("0");         // Byte 5 : Session key for non-volatile memory

            SendLen = 0x0A;
            RecvLen = 0x02;

            retCode = SendAPDUandDisplay(0);

            if (retCode != ModWinsCard.SCARD_S_SUCCESS)
            {
                //MessageBox.Show("FAIL Authentication!");
                return false;
            }

            return true;
        }

        public static void decodeData()
        {


            string hexStr = "";
            string tmpStr = "";
            byte[] Recvblock_tmp = new byte[16];

            for (int x = 0; x < 16; x++)
            {
                Recvblock_tmp[x] = RecvBuff[x];
            }

            for (int indx = 0; indx <= (RecvLen - 1); indx++)
            {
                hexStr = hexStr + " " + string.Format("{0:X2}", RecvBuff[indx]);
            }

            for (int indx = 0; indx <= 17 - 1; indx++)
            {
                tmpStr = tmpStr + Convert.ToChar(RecvBuff[indx]);
            }

            RecvBlock.Add(new Tuple<byte[], string, string>(Recvblock_tmp, hexStr, tmpStr));

            //ticketdata = utility.DecodeParkingSector(sector);
        }

        public static void authenticateCard(Main MainForm)
        {

            byte[] parkingKeyA = new byte[6]
            {
                (byte) 50,
                (byte) 17,
                (byte) 193,
                (byte) 46,
                (byte) 66,
                (byte) 147
            };

            int indx;
            if (authenticateBlock("10"))
            {
                ClearBuffers();
                SendBuff[0] = 0xFF;                             // CLA
                SendBuff[1] = 0x82;                             // INS
                SendBuff[2] = 0x20;                             // P1
                SendBuff[3] = 0x00;           // P2 : Starting Block No.
                SendBuff[4] = 0x06;            // P3 : Data length

                for (indx = 0; indx <= 6 - 1; indx++)
                {
                    SendBuff[indx + 5] = parkingKeyA[indx];
                }
                SendLen = SendBuff[4] + 5;
                RecvLen = 0x02;

                retCode = SendAPDUandDisplay(2);

                if (retCode != ModWinsCard.SCARD_S_SUCCESS)
                {
                }
            }
            else
            {
            }
        }


        #endregion

        public static string ReadCardData()
        {
            RecvBlock.Clear();
            string Data = "Ticket ID" + verifyCard("40") + "\n";
            string tmpStr1 = "";
            for (int indx = 0; indx <= 16 - 1; indx++)
            {

                tmpStr1 = tmpStr1 + string.Format("{0:X2}", RecvBlock[0].Item1[indx]);
            }
            Data = Convert.ToString((Convert.ToInt64(tmpStr1.Substring(14, 2), 16) << 24) + (Convert.ToInt64(tmpStr1.Substring(12, 2), 16) << 16) + (Convert.ToInt64(tmpStr1.Substring(10, 2), 16) << 8) + Convert.ToInt64(tmpStr1.Substring(8, 2), 16));




            return Data;
        }

        public static string ReadingOfCards()
        {
            //string block = ReadCardData();
            //if (block != "0")
            //{
            //    return block;
            //}
            //else
                return ACRReadData();
        }
    }
}
