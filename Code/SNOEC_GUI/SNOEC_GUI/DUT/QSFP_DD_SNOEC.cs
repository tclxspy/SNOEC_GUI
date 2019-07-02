using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNOEC_GUI
{
    public class QSFP_DD_SNOEC
    {
        public static int DUT_USB_Port = 1;
        public static Company company = Company.SNOEC;
        public static IOPort.SoftHard softHard = IOPort.SoftHard.SerialPort;
        private static object syncRoot = new Object();//used for thread synchronization

        public QSFP_DD_SNOEC()
        {

        }

        public enum Company : int
        {
            SNOEC = 0,
        }

        private void EnterEngMode(int bank, int page)
        {
            if (company == Company.SNOEC)
            {
                byte[] buff = new byte[6];
                buff[0] = 0x12;
                buff[1] = 0x34;
                buff[2] = 0x56;
                buff[3] = 0x78;
                buff[4] = (byte)bank;
                buff[5] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 122, softHard, buff);

                //byte[] buff = new byte[6];
                //buff[0] = 0x93;
                //buff[1] = 0x78;
                //buff[2] = 0xCC;
                //buff[3] = 0xAE;
                //buff[4] = (byte)bank;
                //buff[5] = (byte)page;
                //IOPort.WriteReg(DUT_USB_Port, 0xA0, 122, softHard, buff);
            }
        }

        public byte[] WriteReg(int deviceIndex, int deviceAddress, int page, int regAddress, byte[] dataToWrite)
        {
            EnterEngMode(0x00, page);
            return IOPort.WriteReg(deviceIndex, deviceAddress, regAddress, softHard, dataToWrite);
        }

        public byte[] ReadReg(int deviceIndex, int deviceAddress, int page, int regAddress, int length)
        {
            EnterEngMode(0x00, page);
            return IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, length);
        }

        public string ReadSN()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0); 
                string SN = "";
                SN = EEPROM_SNOEC.ReadSn(DUT_USB_Port, 0xA0, 166);
                return SN.Trim();
            }
        }

        public string ReadPN()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0);
                string pn = "";
                pn = EEPROM_SNOEC.ReadPn(DUT_USB_Port, 0xA0, 148);
                return pn.Trim();
            }
        }

        public string ReadFWRev()
        {
            lock (syncRoot)
            {
                string fwrev = "";
                EnterEngMode(0x00, 0xC2);
                fwrev = EEPROM_SNOEC.ReadFWRev(DUT_USB_Port, 0xA0, 0xB4);
                return fwrev;
            }
        }

        public string ReadVendorName()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0);
                string vendor = "";
                vendor = EEPROM_SNOEC.ReadVendorName(DUT_USB_Port, 0xA0, 129);
                return vendor.Trim();
            }
        }

        public double ReadDmiTemp()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0);
                return EEPROM_SNOEC.readdmitemp(DUT_USB_Port, 0xA0, 14);
            }
        }

        public double ReadDmiVcc()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0);
                return EEPROM_SNOEC.readdmivcc(DUT_USB_Port, 0xA0, 16);
            }
        }

        public double ReadDmiBias(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0x00, 0x11);
                    double dmibias = 0.0;
                    switch (channel)
                    {
                        case 1:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 170);
                            break;
                        case 2:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 172);
                            break;
                        case 3:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 174);
                            break;
                        case 4:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 176);
                            break;
                        case 5:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 178);
                            break;
                        case 6:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 180);
                            break;
                        case 7:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 182);
                            break;
                        case 8:
                            dmibias = EEPROM_SNOEC.readdmibias(DUT_USB_Port, 0xA0, 184);
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
                    EnterEngMode(0x00, 0x11);
                    double dmitxp = 0.0;
                    switch (channel)
                    {
                        case 1:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 154);
                            break;
                        case 2:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 156);
                            break;
                        case 3:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 158);
                            break;
                        case 4:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 160);
                            break;
                        case 5:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 162);
                            break;
                        case 6:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 164);
                            break;
                        case 7:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 166);
                            break;
                        case 8:
                            dmitxp = EEPROM_SNOEC.readdmitxp(DUT_USB_Port, 0xA0, 170);
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
                    EnterEngMode(0x00, 0x11);
                    double dmirxp = 0.0;
                    switch (channel)
                    {
                        case 1:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 186);
                            break;
                        case 2:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 188);
                            break;
                        case 3:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 190);
                            break;
                        case 4:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 192);
                            break;
                        case 5:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 194);
                            break;
                        case 6:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 196);
                            break;
                        case 7:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 198);
                            break;
                        case 8:
                            dmirxp = EEPROM_SNOEC.readdmirxp(DUT_USB_Port, 0xA0, 200);
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

        public ushort ReadAdcTemp()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0xC2);
                return EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x80);
            }
        }

        public ushort ReadAdcVcc()
        {
            lock (syncRoot)
            {
                EnterEngMode(0x00, 0xC2);
                return EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x82);
            }
        }

        public ushort ReadAdcBias(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0x00, 0xC2);
                    ushort adcbias = 0;
                    switch (channel)
                    {
                        case 1:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x94);
                            break;
                        case 2:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x96);
                            break;
                        case 3:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x98);
                            break;
                        case 4:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x9A);
                            break;
                        case 5:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x9C);
                            break;
                        case 6:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x9E);
                            break;
                        case 7:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xA0);
                            break;
                        case 8:
                            adcbias = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xA2);
                            break;
                        default:
                            break;
                    }
                    return adcbias;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }

        public ushort ReadAdcTxP(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0x00, 0xC2);
                    ushort adctxp = 0;
                    switch (channel)
                    {
                        case 1:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x84);
                            break;
                        case 2:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x86);
                            break;
                        case 3:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x88);
                            break;
                        case 4:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x8A);
                            break;
                        case 5:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x8C);
                            break;
                        case 6:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x8E);
                            break;
                        case 7:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x90);
                            break;
                        case 8:
                            adctxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0x92);
                            break;
                        default:
                            break;
                    }
                    return adctxp;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }

        public ushort ReadAdcRxP(int channel)
        {
            lock (syncRoot)
            {
                try
                {
                    EnterEngMode(0x00, 0xC2);
                    ushort adcrxp = 0;
                    switch (channel)
                    {
                        case 1:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xA4);
                            break;
                        case 2:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xA6);
                            break;
                        case 3:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xA8);
                            break;
                        case 4:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xAA);
                            break;
                        case 5:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xAC);
                            break;
                        case 6:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xAE);
                            break;
                        case 7:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xB0);
                            break;
                        case 8:
                            adcrxp = EEPROM_SNOEC.readadc(DUT_USB_Port, 0xA0, 0xB2);
                            break;
                        default:
                            break;
                    }
                    return adcrxp;
                }
                catch
                {
                    //Log.SaveLogToTxt(ex.ToString());
                    return Algorithm.MyNaN;
                }
            }
        }

        public ushort GetTxPwrHighAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x8B, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxPwrLowAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x8C, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxPwrHighWarning()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x8D, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxPwrLowWarning()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x8E, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxBiasHighAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x8F, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxBiasLowAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x90, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxBiasHighWarning()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x91, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetTxBiasLowWarning()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x92, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetRxPwrHighAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x95, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetRxPwrLowAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x96, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetRxPwrHighWarning()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x97, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public ushort GetRxPwrLowWarning()
        {
            try
            {
                EnterEngMode(0x00, 0x11);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x98, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0];
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }


        //normal:0, high:1, low:2
        public int GetInteTempAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x09, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return buff[0] & 0x03;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, high:1, low:2
        public int GetInteTempWarning()
        {
            try
            {
                EnterEngMode(0x00, 0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x09, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return (buff[0] >> 2) & 0x03;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, high:1, low:2
        public int GetInteVccAlarm()
        {
            try
            {
                EnterEngMode(0x00, 0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x09, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return (buff[0] >> 4) & 0x03;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        //normal:0, high:1, low:2
        public int GetInteVccWarning()
        {
            try
            {
                EnterEngMode(0x00, 0);
                byte[] buff = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x09, softHard, 1);
                if (buff == null)
                {
                    return Algorithm.MyNaN;
                }
                return (buff[0] >> 6) & 0x03;
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public void DataPathPwrUp_1()
        {
            try
            {
                EnterEngMode(0x00, 0x00);
                byte[] data = new byte[1] { 0x00 };
                Byte[] buff = IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x1A, softHard, data);

                EnterEngMode(0x00, 0x10);
                data = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x80, softHard, 1);

                data[0] |=  0x0F;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x80, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void DataPathPwrUp_2()
        {
            try
            {
                EnterEngMode(0x00, 0x00);
                byte[] data = new byte[1] { 0x00 };
                Byte[] buff = IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x1A, softHard, data);

                EnterEngMode(0x00, 0x10);
                data = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x80, softHard, 1);

                data[0] |= 0xF0;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x80, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void DataPathPwrDn_1()
        {
            try
            {
                EnterEngMode(0x00, 0x00);
                byte[] data = new byte[1] { 0x00 };
                Byte[] buff = IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x1A, softHard, data);

                EnterEngMode(0x00, 0x10);
                data = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x80, softHard, 1);

                data[0] &= 0xF0;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x80, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void DataPathPwrDn_2()
        {
            try
            {
                EnterEngMode(0x00, 0x00);
                byte[] data = new byte[1] { 0x00 };
                Byte[] buff = IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x1A, softHard, data);

                EnterEngMode(0x00, 0x10);
                data = IOPort.ReadReg(DUT_USB_Port, 0xA0, 0x80, softHard, 1);

                data[0] &= 0x0F;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x80, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void LowPwr()
        {
            try
            {
                EnterEngMode(0x00, 0x00);
                byte[] data = new byte[1] { 0x10 };
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x1A, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void SoftRest()
        {
            try
            {
                EnterEngMode(0x00, 0x00);
                byte[] data = new byte[1] { 0x08 };
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x1A, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void Apply_DataPathInit_Staged_0()
        {
            try
            {
                EnterEngMode(0x00, 0x10);
                byte[] data = new byte[1] { 0xFF };
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x8F, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void Apply_DataPathInit_Staged_1()
        {
            try
            {
                EnterEngMode(0x00, 0x10);
                byte[] data = new byte[1] { 0xFF };
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0xB2, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void Apply_Immediate_Staged_0()
        {
            try
            {
                EnterEngMode(0x00, 0x10);
                byte[] data = new byte[1] { 0xFF };
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0x90, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }

        public void Apply_Immediate_Staged_1()
        {
            try
            {
                EnterEngMode(0x00, 0x10);
                byte[] data = new byte[1] { 0xFF };
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 0xB3, softHard, data);
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
            }
        }
    }
}
