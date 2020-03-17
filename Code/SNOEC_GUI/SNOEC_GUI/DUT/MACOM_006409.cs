using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SNOEC_GUI
{
    class MACOM_006409
    {
        private static object syncRoot;//used for thread synchronization

        public MACOM_006409()
        {
            syncRoot = new Object();//used for thread synchronization
        }

        public ushort GetVendorID()
        {
            lock (syncRoot)
            {
                return (ushort)this.ReadReg(0x0200);
            }
        }

        public bool WriteReg(ushort regAddress, ushort dataToWrite)
        {
            lock (syncRoot)
            {
                byte[] dataToReceive = new byte[4];
                byte[] dataToSend = new byte[4];

                regAddress = (ushort)((regAddress << 1) & 0xFFFE);//operate code = 0

                dataToSend[0] = (byte)(regAddress / 256);
                dataToSend[1]= (byte)(regAddress & 0xFF);

                dataToSend[2] = (byte)(dataToWrite / 256);
                dataToSend[3] = (byte)(dataToWrite & 0xFF);

                return USB_SPI.OperateData(dataToSend, dataToReceive, 4);
            }
        }

        public short ReadReg(ushort regAddress)
        {
            lock (syncRoot)
            {
                byte[] dataToReceive = new byte[2];
                byte[] dataToSend = new byte[2];

                regAddress = (ushort)((regAddress << 1) | 0x0001);//operate code = 1

                dataToSend[0] = (byte)(regAddress / 256);
                dataToSend[1] = (byte)(regAddress & 0xFF);

                bool result = USB_SPI.OperateData(dataToSend, dataToReceive, 2);
                if (result)
                {
                    return (short)(dataToReceive[0] * 256 + dataToReceive[1]);
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
