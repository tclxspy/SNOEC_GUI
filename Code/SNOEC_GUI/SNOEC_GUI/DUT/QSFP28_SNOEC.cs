using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNOEC_GUI
{
    public class QSFP28_SNOEC
    {
        private static int DUT_USB_Port = 0;
        private static object syncRoot = new Object();//used for thread synchronization
        private const IOPort.SoftHard softHard = IOPort.SoftHard.OnEasyB_I2C;

        public QSFP28_SNOEC(int deviceIndex)
        {
            DUT_USB_Port = deviceIndex;
        }

        private QSFP28_SNOEC() { }

        public string ReadSN()
        {
            lock (syncRoot)
            {
                string SN = "";
                //EnterEngMode(0);
                SN = EEPROM_SNOEC.ReadSn(DUT_USB_Port, 0xA0, 196);
                return SN.Trim();
            }
        }

        public string ReadPn()
        {
            string pn = "";
            //Engmod(0);
            pn = EEPROM_SNOEC.ReadPn(DUT_USB_Port, 0xA0, 168);
            return pn.Trim();
        }

        public double ReadDmiTemp()
        {
            lock (syncRoot)
            {
                return EEPROM_SNOEC.readdmitemp(DUT_USB_Port, 0xA0, 22);
            }
        }

        public double ReadDmiVcc()
        {
            lock (syncRoot)
            {
                return EEPROM_SNOEC.readdmivcc(DUT_USB_Port, 0xA0, 26);
            }
        }

        public double ReadDmiBias(int channel)
        {
            lock (syncRoot)
            {
                try
                {
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
                    switch (channel)
                    {
                        case 1:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x01);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
                            break;
                        case 2:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x02);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
                            break;
                        case 3:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x04);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
                            break;
                        case 4:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0x08);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
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
                byte[] dataToWrite = { 0xFF };
                byte[] dataReadArray;
                for (int i = 0; i < 3; i++)
                {
                    IOPort.WrtieReg(DUT_USB_Port, 0xA0, 86, softHard, dataToWrite);
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
                byte[] buff = new byte[1];
                try
                {
                    switch (channel)
                    {
                        case 1:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0xFE);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
                            break;
                        case 2:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0xFD);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
                            break;
                        case 3:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0xFB);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
                            break;
                        case 4:
                            buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 86, softHard, 1);
                            buff[0] = (byte)(buff[0] | 0xF7);
                            IOPort.WrtieReg(0, 0xA0, 86, softHard, buff);
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
                byte[] dataToWrite = { 0x00 };
                byte[] dataReadArray;
                for (int i = 0; i < 3; i++)
                {
                    IOPort.WrtieReg(DUT_USB_Port, 0xA0, 86, softHard, dataToWrite);
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
                byte[] buff = IOPort.ReadReg(0, 0xA0, 6, softHard, 1);
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
                byte[] buff = IOPort.ReadReg(0, 0xA0, 6, softHard, 1);
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
                byte[] buff = IOPort.ReadReg(0, 0xA0, 7, softHard, 1);
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
                byte[] buff = IOPort.ReadReg(0, 0xA0, 7, softHard, 1);
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
    }
}
