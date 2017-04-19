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
    }
}
