using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SNOEC_GUI
{
    public class QSFP28_SNOEC
    {
        public static int DUT_USB_Port = 1;        
        public static Company company = Company.Nopasswords;
        public static IOPort.SoftHard softHard = IOPort.SoftHard.SerialPort;
        private static object syncRoot = new Object();//used for thread synchronization
        protected DUTCoeffControlByPN dataTable_DUTCoeffControlByPN;

        public enum Company : int
        {
            SNOEC = 0,
            Inno = 1,
            Generic = 2,
            Luxshare = 3,
            Nopasswords = 4,
            Nopasswords_Nopage = 5
        }

        public QSFP28_SNOEC(DUTCoeffControlByPN tabe)
        {
            this.dataTable_DUTCoeffControlByPN = tabe;
        }

        public QSFP28_SNOEC()
        {
        }

        private void EnterEngMode(int page)
        {
            if (company == Company.Inno)
            {
                byte[] buff = new byte[5];
                buff[0] = 0xca;
                buff[1] = 0x2d;
                buff[2] = 0x81;
                buff[3] = 0x5f;
                buff[4] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);
            }
            else if (company == Company.SNOEC)
            {
                //byte[] buff = new byte[5];
                //buff[0] = 0xdf;
                //buff[1] = 0x5e;
                //buff[2] = 0x75;
                //buff[3] = 0xcd;
                //buff[4] = (byte)page;
                //IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);
                byte[] buff = new byte[5];
                buff[0] = 0x12;
                buff[1] = 0x34;
                buff[2] = 0x56;
                buff[3] = 0x78;
                buff[4] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);

                //byte[] buff = new byte[5];
                //buff[0] = 0x80;
                //buff[1] = 0x00;
                //buff[2] = 0x46;
                //buff[3] = 0x49;
                //buff[4] = (byte)page;
                //IOPort.WriteReg(DUT_USB_Port, 0xA2, 123, softHard, buff);

                //byte[] buff = new byte[1];//sip
                //buff[0] = (byte)page;
                //IOPort.WriteReg(DUT_USB_Port, 0xA0, 127, softHard, buff);
            }
            else if(company == Company.Generic)
            {
                byte[] buff = new byte[5];
                buff[0] = 0x00;
                buff[1] = 0x00;
                buff[2] = 0x10;
                buff[3] = 0x11;
                buff[4] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);
            }
            else if (company == Company.Luxshare)
            {
                //byte[] buff = new byte[5];//100G
                //buff[0] = 0xA6;
                //buff[1] = 0x06;
                //buff[2] = 0x4C;
                //buff[3] = 0x6E;
                //buff[4] = (byte)page;
                //IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);

                //byte[] buff = new byte[5];//56G
                //buff[0] = 0xE0;
                //buff[1] = 0x55;
                //buff[2] = 0x3E;
                //buff[3] = 0x8A;
                //buff[4] = (byte)page;
                //IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);

                byte[] buff = new byte[5];//56G
                buff[0] = 0x4C;
                buff[1] = 0x4F;
                buff[2] = 0x45;
                buff[3] = 0x54;
                buff[4] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);
            }
            else if (company == Company.Nopasswords)
            {
                byte[] buff = new byte[1];
                buff[0] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 127, softHard, buff);
            }
            else if (company == Company.Nopasswords_Nopage)
            {
                //no code
            }
        }

        public byte[] WriteReg(int deviceIndex, int deviceAddress, int page, int regAddress, byte[] dataToWrite)
        {
            EnterEngMode(page);
            return IOPort.WriteReg(deviceIndex, deviceAddress, regAddress, softHard, dataToWrite);
        }

        public byte[] ReadReg(int deviceIndex, int deviceAddress, int page, int regAddress, int length)
        {
            EnterEngMode(page);
            return IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, length);
        }

        public string ReadSN()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                string SN = "";
                SN = EEPROM_SNOEC.ReadSn(DUT_USB_Port, 0xA0, 196);
                return SN.Trim();
            }
        }

        public string ReadPn()
        {
            try
            {
                lock (syncRoot)
                {
                    EnterEngMode(0);
                    string pn = "";
                    pn = EEPROM_SNOEC.ReadPn(DUT_USB_Port, 0xA0, 168);
                    return pn.Trim();
                }
            }
            catch
            {
                MessageBox.Show("No link.");
                return "999";
            }
        }

        public string ReadFWRev()
        {
            lock (syncRoot)
            {
                string fwrev = "";
                //EnterEngMode(4);
                //fwrev = EEPROM_SNOEC.ReadFWRev(DUT_USB_Port, 0xA0, 128);
                return fwrev;
            }
        }

        public string ReadVendorName()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                string vendor = "";
                vendor = EEPROM_SNOEC.ReadVendorName(DUT_USB_Port, 0xA0, 148);
                return vendor.Trim();
            }
        }

        public double ReadDmiTemp()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                return EEPROM_SNOEC.readdmitemp(DUT_USB_Port, 0xA0, 22);
            }
        }

        public double ReadDmiVcc()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                return EEPROM_SNOEC.readdmivcc(DUT_USB_Port, 0xA0, 26);
            }
        }

        public double ReadDmiBias(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0);
                    double dmibias = 0.0;
                    switch (channel)
                    {
                        case 1:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 42);
                            break;
                        case 2:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 44);
                            break;
                        case 3:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 46);
                            break;
                        case 4:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 48);
                            break;
                        default:
                            break;
                    }
                    return dmibias;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }

        public double ReadDmiTxP(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0);
                    double dmitxp = 0.0;
                    switch (channel)
                    {
                        case 1:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 50);
                            break;
                        case 2:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 52);
                            break;
                        case 3:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 54);
                            break;
                        case 4:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 56);
                            break;
                        default:
                            break;
                    }
                    return dmitxp;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }

        public double ReadDmiRxP(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0);
                    double dmirxp = 0.0;
                    switch (channel)
                    {
                        case 1:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 34);
                            break;
                        case 2:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 36);
                            break;
                        case 3:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 38);
                            break;
                        case 4:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 40);
                            break;
                        default:
                            break;
                    }
                    return dmirxp;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }

        public bool SetSoftTxDis(int channel)
        {
            lock (syncRoot)
            {
                byte[] buff = new byte[1];
                try
                {
                    EnterEngMode(0);
                    switch (channel)
                    {
                        case 1:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x01);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        case 2:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x02);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        case 3:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x04);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        case 4:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x08);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        default:
                            break;
                    }
                    return true;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return false;
                }
            }
        }

        public bool SetSoftTxDis()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                byte[] dataToWrite = { 0xFF };
                byte[] dataReadArray;
                for (int i = 0; i < 3; i++)
                {
                    IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, dataToWrite);
                    dataReadArray = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                    if (dataReadArray[0] == 0xFF)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool TxChannelEnable(int channel)
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                byte[] buff = new byte[1];
                try
                {
                    switch (channel)
                    {
                        case 1:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] & 0xFE);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        case 2:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] & 0xFD);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        case 3:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] & 0xFB);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        case 4:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] & 0xF7);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, buff);
                            break;
                        default:
                            break;
                    }
                    return true;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return false;
                }
            }
        }

        public bool TxAllChannelEnable()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
                byte[] dataToWrite = { 0x00 };
                byte[] dataReadArray;
                for (int i = 0; i < 3; i++)
                {
                    IOPort.WriteReg(DUT_USB_Port, 0xA0, 86, softHard, dataToWrite);
                    dataReadArray = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                    if (dataReadArray[0] == 0x00)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public byte[] GetTxChEnStatus()
        {
            lock (syncRoot)
            {
                byte[] buff = new byte[1];
                try
                {
                    EnterEngMode(0);
                    buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                    return buff;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return null;
                }
            }
        }

        //normal:0, low:1, high:2
        public int GetInteTempAlarm()
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 6, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0] >> 6;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteTempWarning()
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 6, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return (buff[0] >> 4) & 3;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteVccAlarm()
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 7, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0] >> 6;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteVccWarning()
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 7, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return (buff[0] >> 4) & 3;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteRxPowerAlarm(int channel)
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 9, softHard, 2);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }

                switch (channel)
                {
                    case 1:
                        return buff[0] >> 6;

                    case 2:
                        return (buff[0] >> 2) & 3;

                    case 3:
                        return buff[1] >> 6;

                    case 4:
                        return (buff[1] >> 2) & 3;

                    default:
                        return Algorithm.MyNaN;
                }             
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteRxPowerWarning(int channel)
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 9, softHard, 2);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }

                switch (channel)
                {
                    case 1:
                        return (buff[0] >> 4) & 3;

                    case 2:
                        return buff[0] & 3;

                    case 3:
                        return (buff[1] >> 4) & 3;

                    case 4:
                        return buff[1] & 3;

                    default:
                        return Algorithm.MyNaN;
                }
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteTxPowerAlarm(int channel)
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 13, softHard, 2);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }

                switch (channel)
                {
                    case 1:
                        return buff[0] >> 6;

                    case 2:
                        return (buff[0] >> 2) & 3;

                    case 3:
                        return buff[1] >> 6;

                    case 4:
                        return (buff[1] >> 2) & 3;

                    default:
                        return Algorithm.MyNaN;
                }
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteTxPowerWarning(int channel)
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 13, softHard, 2);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }

                switch (channel)
                {
                    case 1:
                        return (buff[0] >> 4) & 3;

                    case 2:
                        return buff[0] & 3;

                    case 3:
                        return (buff[1] >> 4) & 3;

                    case 4:
                        return buff[1] & 3;

                    default:
                        return Algorithm.MyNaN;
                }
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteTxBiasAlarm(int channel)
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 11, softHard, 2);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }

                switch (channel)
                {
                    case 1:
                        return buff[0] >> 6;

                    case 2:
                        return (buff[0] >> 2) & 3;

                    case 3:
                        return buff[1] >> 6;

                    case 4:
                        return (buff[1] >> 2) & 3;

                    default:
                        return Algorithm.MyNaN;
                }
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, low:1, high:2
        public int GetInteTxBiasWarning(int channel)
        {
            try
            {
                EnterEngMode(0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 11, softHard, 2);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }

                switch (channel)
                {
                    case 1:
                        return (buff[0] >> 4) & 3;

                    case 2:
                        return buff[0] & 3;

                    case 3:
                        return (buff[1] >> 4) & 3;

                    case 4:
                        return buff[1] & 3;

                    default:
                        return Algorithm.MyNaN;
                }
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public bool PSM4_SetIbias(int channel, byte value)
        {
            lock (syncRoot)
            {
                byte[] buff = new byte[1];
                buff[0] = value;
                try
                {
                    //EnterEngMode(0);
                    switch (channel)
                    {
                        case 1:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 0, softHard, buff);
                            break;
                        case 2:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 1, softHard, buff);
                            break;
                        case 3:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 2, softHard, buff);
                            break;
                        case 4:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 3, softHard, buff);
                            break;
                        case 5:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 4, softHard, buff);
                            break;
                        default:
                            break;
                    }
                    return true;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return false;
                }
            }
        }

        public byte[] PSM4_GetIbias(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    byte[] buff = new byte[1];
                    //EnterEngMode(0);
                    switch (channel)
                    {
                        case 6:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 5, softHard, 1);
                            break;
                        default:
                            break;
                    }
                    return buff;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return null;
                }
            }
        }

        public bool PSM4_SetHeator(int channel, UInt16 value)
        {
            lock (syncRoot)
            {
                byte[] buff_L = new byte[1];
                buff_L[0] = (byte)(value % 256);
                byte[] buff_H = new byte[1];
                buff_H[0] = (byte)(value / 256);
                try
                {
                    //EnterEngMode(0);
                    switch (channel)
                    {
                        case 1:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 6, softHard, buff_L);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 7, softHard, buff_H);
                            break;
                        case 2:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 8, softHard, buff_L);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 9, softHard, buff_H);
                            break;
                        case 3:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 10, softHard, buff_L);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 11, softHard, buff_H);
                            break;
                        case 4:
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 12, softHard, buff_L);
                            IOPort.WriteReg(DUT_USB_Port, 0xA0, 13, softHard, buff_H);
                            break;
                        default:
                            break;
                    }
                    return true;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return false;
                }
            }
        }

        public byte[] PSM4_GetHeator(int channel)
        {
            lock (syncRoot)
            {
                byte[] buff = new byte[2];
                byte[] buff_L = new byte[1];
                byte[] buff_H = new byte[1];
                try
                {
                    //EnterEngMode(0);
                    switch (channel)
                    {
                        case 1:
                            buff_L = IOPort.ReadReg(DUT_USB_Port, 0xA0, 14, softHard, 1);
                            buff_H = IOPort.ReadReg(DUT_USB_Port, 0xA0, 15, softHard, 1);
                            break;
                        case 2:
                            buff_L = IOPort.ReadReg(DUT_USB_Port, 0xA0, 16, softHard, 1);
                            buff_H = IOPort.ReadReg(DUT_USB_Port, 0xA0, 17, softHard, 1);
                            break;
                        case 3:
                            buff_L = IOPort.ReadReg(DUT_USB_Port, 0xA0, 18, softHard, 1);
                            buff_H = IOPort.ReadReg(DUT_USB_Port, 0xA0, 19, softHard, 1);
                            break;
                        case 4:
                            buff_L = IOPort.ReadReg(DUT_USB_Port, 0xA0, 20, softHard, 1);
                            buff_H = IOPort.ReadReg(DUT_USB_Port, 0xA0, 21, softHard, 1);
                            break;
                        default:
                            break;
                    }
                    buff[0] = buff_L[0];
                    buff[1] = buff_H[0];
                    return buff;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return null;
                }
            }
        }

        public enum Coeff : int
        {
            DmiTempSlope,
            DmiVccSlope,
            DmiTxpowerSlope,
            DmiRxpowerSlope,
            DmiTxBiasSlope,
            DmiTempOffset,
            DmiVccOffset,
            DmiTxpowerOffset,
            DmiRxpowerOffset,
            DmiTxBiasOffset,
            DmiTempShift,
            DmiVccShift,
            DmiTxpowerShift,
            DmiRxpowerShift,
            DmiTxBiasShift,
        }

        public string GetCoeff(Coeff coeff, int channel)
        {
            lock (syncRoot)
            {
                string coeffName = coeff.ToString();
                try
                {
                    DUTCoeffControlByPN.CoeffInfo coeffInfo = dataTable_DUTCoeffControlByPN.GetOneInfoFromTable(coeffName, channel);

                    EnterEngMode(coeffInfo.Page);
                    string value = EEPROM_SNOEC.ReadCoef(DUT_USB_Port, 0xA0, coeffInfo.StartAddress, coeffInfo.Format);

                    //Log.SaveLogToTxt("Get " + coeffName + " is " + value);
                    return value;

                }
                catch
                {
                    //Log.SaveLogToTxt("Failed to get value of " + coeffName);
                    return Algorithm.MyNaN.ToString();
                }
            }
        }

        public enum NameOfADC : int
        {
            TemperatureAdc,
            VccAdc,
            TxPowerAdc,
            RxPowerAdc,
            TxBiasAdc,
        }

        public ushort ReadADC(NameOfADC enumName, int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    string name = enumName.ToString();
                    DUTCoeffControlByPN.CoeffInfo coeffInfo = dataTable_DUTCoeffControlByPN.GetOneInfoFromTable(name, channel);

                    EnterEngMode(coeffInfo.Page);
                    UInt16 valueADC = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, coeffInfo.StartAddress);
                    //Log.SaveLogToTxt("Current TXPOWERADC is " + valueADC);
                    return valueADC;

                }
                catch (Exception ex)
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }
    }
}
