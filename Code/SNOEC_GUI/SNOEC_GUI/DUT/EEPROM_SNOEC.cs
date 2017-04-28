using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNOEC_GUI
{
    public class EEPROM_SNOEC
    {
        private static IOPort.SoftHard softHard = QSFP28_SNOEC.softHard;

        private EEPROM_SNOEC() { }//make sure it will not be instantiated        

        public static string ReadSn(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff1 = new byte[16];
            UInt16[] buff = new UInt16[16];
            string sn = "ffffffff";
            try
            {
                if (mdiomode == 1)
                {
                    buff = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 16);
                    for (int i = 0; i < 16; i++)
                    {
                        buff1[i] = (byte)(buff[i]);
                    }
                }
                else
                {
                    buff1 = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 16);
                }
                sn = Convert.ToChar(Convert.ToInt64(buff1[0])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[1])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[2])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[3])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[4])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[5])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[6])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[7])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[8])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[9])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[10])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[11])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[12])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[13])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[14])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[15])).ToString();
                return sn.Trim();

            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN.ToString();
            }
        }

        public static string ReadPn(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff1 = new byte[16];
            UInt16[] buff = new UInt16[16];
            string pn = "ffffffff";
            try
            {
                if (mdiomode == 1)
                {
                    buff = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 16);
                    for (int i = 0; i < 16; i++)
                    {
                        buff1[i] = (byte)(buff[i]);
                    }
                }
                else
                {
                    buff1 = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 16);
                }
                pn = Convert.ToChar(Convert.ToInt64(buff1[0])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[1])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[2])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[3])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[4])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[5])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[6])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[7])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[8])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[9])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[10])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[11])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[12])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[13])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[14])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[15])).ToString();
                return pn.Trim();
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN.ToString();
            }
        }

        public static string ReadFWRev(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            string fwrev = "ff";
            UInt16[] buff1 = new UInt16[1];
            try
            {
                if (mdiomode == 1)
                {
                    buff1 = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 2);
                    buff[0] = (byte)(buff1[0]);
                    buff[1] = (byte)(buff1[1]);
                }
                else
                {

                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);

                }
                fwrev = Convert.ToString((buff[0] * 256 + buff[1]), 16);
                return fwrev.Trim();
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN.ToString();
            }
        }

        public static double readdmitemp(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            UInt16[] buffmdio = new UInt16[1];
            double temperature = 0.0;
            try
            {
                if (mdiomode == 1)
                {
                    buffmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 1);

                    buff[0] = (byte)(buffmdio[0] / 256);
                    buff[1] = (byte)(buffmdio[0] & 0xFF);
                }
                else
                {
                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);
                }
                if (buff[0] > Convert.ToByte(127))
                {
                    temperature = (buff[0] + (buff[1] / 256.0)) - 256;
                }
                else
                {
                    temperature = (buff[0] + (buff[1] / 256.0));
                }
                temperature = Math.Round(temperature, 4);
                return temperature;

            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }
        public static double readdmivcc(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            UInt16[] buffmdio = new UInt16[1];
            double vcc = 0.0;
            try
            {
                if (mdiomode == 1)
                {
                    buffmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 1);
                    vcc = buffmdio[0] / 10000.0;
                }
                else
                {
                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);
                    vcc = (buff[0] * 256 + buff[1]) / 10000.0;
                }

                vcc = Math.Round(vcc, 4);
                return vcc;

            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public static double readdmibias(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            UInt16[] buffmdio = new UInt16[1];
            double bias = 0.0;
            try
            {
                if (mdiomode == 1)
                {
                    buffmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 1);
                    bias = buffmdio[0] / 500.0;
                }
                else
                {
                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);
                    bias = (buff[0] * 256 + buff[1]) / 500.0;
                }
                bias = Math.Round(bias, 4);
                return bias;

            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

        public static double readdmitxp(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            UInt16[] buffmdio = new UInt16[1];
            double txp = 0.0;
            try
            {
                if (mdiomode == 1)
                {
                    buffmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 1);
                    txp = 10 * (Math.Log10(buffmdio[0] * (1E-4)));
                }
                else
                {
                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);
                    int data = buff[0] * 256 + buff[1];
                    if (data == 0)
                    {
                        return Algorithm.MyNaN;
                    }
                    txp = 10 * (Math.Log10((data) * (1E-4)));
                }
                txp = Math.Round(txp, 4);
                return txp;

            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }
        public static double readdmirxp(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            UInt16[] buffmdio = new UInt16[1];
            double rxp = 0.0;
            try
            {
                if (mdiomode == 1)
                {
                    buffmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 1);
                    rxp = 10 * (Math.Log10(buffmdio[0] * (1E-4)));
                }
                else
                {
                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);
                    int data = buff[0] * 256 + buff[1];
                    if (data == 0)
                    {
                        return Algorithm.MyNaN;
                    }
                    rxp = 10 * (Math.Log10((data) * (1E-4)));
                }
                rxp = Math.Round(rxp, 4);
                return rxp;

            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }

    }
}
