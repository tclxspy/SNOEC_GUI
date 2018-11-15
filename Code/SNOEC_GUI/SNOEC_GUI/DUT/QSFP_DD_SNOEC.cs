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

        private void EnterEngMode(int page)
        {
            if (company == Company.SNOEC)
            {
                byte[] buff = new byte[5];
                buff[0] = 0x12;
                buff[1] = 0x34;
                buff[2] = 0x56;
                buff[3] = 0x78;
                buff[4] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 123, softHard, buff);
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
                SN = EEPROM_SNOEC.ReadSn(DUT_USB_Port, 0xA0, 166);
                return SN.Trim();
            }
        }

        public string ReadPN()
        {
            lock (syncRoot)
            {
                EnterEngMode(0);
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
                vendor = EEPROM_SNOEC.ReadVendorName(DUT_USB_Port, 0xA0, 129);
                return vendor.Trim();
            }
        }
    }
}
