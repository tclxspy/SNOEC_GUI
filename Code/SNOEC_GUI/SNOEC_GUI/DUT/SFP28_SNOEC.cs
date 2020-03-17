using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNOEC_GUI
{
    class SFP28_SNOEC
    {
        public static int DUT_USB_Port = 1;
        public static Company company = Company.SNOEC;
        public static IOPort.SoftHard softHard = IOPort.SoftHard.SerialPort;
        private static object syncRoot = new Object();//used for thread synchronization

        public SFP28_SNOEC()
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
                byte[] buff = new byte[1];
                buff[0] = (byte)page;
                IOPort.WriteReg(DUT_USB_Port, 0xA0, 127, softHard, buff);
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
    }
}
