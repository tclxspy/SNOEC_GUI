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
            string sn = Algorithm.MyNaN.ToString();
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
            string pn = Algorithm.MyNaN.ToString();
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
            byte[] buff = new byte[4];
            string fwrev = Algorithm.MyNaN.ToString();
            UInt16[] buff1 = new UInt16[1];
            try
            {
                if (mdiomode == 1)
                {
                    buff1 = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 4);
                    buff[0] = (byte)(buff1[0]);
                    buff[1] = (byte)(buff1[1]);
                    buff[2] = (byte)(buff1[2]);
                    buff[3] = (byte)(buff1[3]);
                }
                else
                {

                    buff = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 4);

                }
                fwrev = Convert.ToString((buff[0] << 24) |( buff[1] << 16) |(buff[2] << 8) | buff[3], 16).ToUpper();
                return fwrev.Trim();
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN.ToString();
            }
        }

        public static string ReadVendorName(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff1 = new byte[16];
            UInt16[] buff = new UInt16[16];
            string vendor = Algorithm.MyNaN.ToString();
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
                vendor = Convert.ToChar(Convert.ToInt64(buff1[0])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[1])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[2])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[3])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[4])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[5])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[6])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[7])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[8])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[9])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[10])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[11])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[12])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[13])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[14])).ToString() + Convert.ToChar(Convert.ToInt64(buff1[15])).ToString();
                return vendor.Trim();

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

        public static string ReadCoef(int deviceIndex, int deviceAddress, int regAddress, byte format, int phycialAdress = 0, int mdiomode = 0)
        {//1 ieee754;2 UInt16;3 UInt32;4 Int16
            string strcoef = "";
            try
            {

                switch (format)
                {
                    case 1:
                        float fcoef = ieeetofloat(deviceIndex, deviceAddress, regAddress, phycialAdress, mdiomode);
                        strcoef = fcoef.ToString();
                        break;
                    case 2:
                        UInt16 u16coef = bytetou16(deviceIndex, deviceAddress, regAddress, phycialAdress, mdiomode);
                        strcoef = u16coef.ToString();
                        break;
                    case 3:
                        UInt32 u32coef = bytetou32(deviceIndex, deviceAddress, regAddress, phycialAdress, mdiomode);
                        strcoef = u32coef.ToString();
                        break;
                    case 4:
                        Int16 i16coef = bytetoi16(deviceIndex, deviceAddress, regAddress, phycialAdress, mdiomode);
                        strcoef = i16coef.ToString();
                        break;
                    default:
                        strcoef = "";
                        break;
                }

                return strcoef;
            }
            catch (Exception ex)
            {

                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN.ToString();
            }
        }

        public static float ieeetofloat(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            float fcoef;
            byte[] bcoef = new byte[4];

            UInt16[] bcoefmdio = new UInt16[4];
            if (mdiomode == 1)
            {
                bcoefmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 4);
                for (int i = 0; i < 4; i++)
                {
                    bcoef[i] = (byte)(bcoefmdio[i]);
                }
            }
            else
            {
                bcoef = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 4);

            }
            System.Threading.Thread.Sleep(200);
            bcoef.Reverse();
            fcoef = BitConverter.ToSingle(bcoef, 0);
            return fcoef;
        }

        public static UInt16 bytetou16(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] bcoef = new byte[2];
            UInt16[] bcoefmdio = new UInt16[2];
            if (mdiomode == 1)
            {
                bcoefmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 2);
                for (int i = 0; i < 2; i++)
                {
                    bcoef[i] = (byte)(bcoefmdio[i]);
                }
            }
            else
            {
                bcoef = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);

            }

            System.Threading.Thread.Sleep(200);

            UInt16 U16coef = (UInt16)((bcoef[1] << 8) + bcoef[0]);
            return U16coef;

        }

        public static UInt32 bytetou32(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] bcoef = new byte[4];
            UInt16[] bcoefmdio = new UInt16[4];
            if (mdiomode == 1)
            {
                bcoefmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 4);
                for (int i = 0; i < 4; i++)
                {
                    bcoef[i] = (byte)(bcoefmdio[i]);
                }
            }
            else
            {
                bcoef = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 4);
            }
            UInt32 U32coef = (UInt32)((bcoef[0] << 24) + (bcoef[1] << 16) + (bcoef[2] << 8) + bcoef[3]);
            System.Threading.Thread.Sleep(200);
            return U32coef;
        }

        public static Int16 bytetoi16(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] bcoef = new byte[2];
            UInt16[] bcoefmdio = new UInt16[2];
            if (mdiomode == 1)
            {
                bcoefmdio = IOPort.ReadMDIO(deviceIndex, deviceAddress, phycialAdress, regAddress, IOPort.MDIOSoftHard.SOFTWARE, 2);
                for (int i = 0; i < 2; i++)
                {
                    bcoef[i] = (byte)(bcoefmdio[i]);
                }
            }
            else
            {
                bcoef = IOPort.ReadReg(deviceIndex, deviceAddress, regAddress, softHard, 2);

            }

            System.Threading.Thread.Sleep(200);

            Int16 i16coef = (Int16)((bcoef[1] << 8) + bcoef[0]);
            return i16coef;

        }

        //read adc
        public static UInt16 readadc(int deviceIndex, int deviceAddress, int regAddress, int phycialAdress = 0, int mdiomode = 0)
        {
            byte[] buff = new byte[2];
            UInt16 adc = 0;
            UInt16[] buff1 = new UInt16[2];
            try
            {
                for (int i = 0; i < 4; i++)
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
                    if (buff[0] != 0)
                        break;

                }

                adc = (UInt16)((buff[1]) * 256 + buff[0]);
                return adc;

            }
            catch (Exception ex)
            {
                //Log.SaveLogToTxt(ex.Message);
                return Algorithm.MyNaN;
            }
        }
    }
}
