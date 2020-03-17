using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//STEP 1: 
//   Add the DLL as a reference to your project through "Project" -> "Add Reference"
//   menu item within Visual Studio
using MCP2210;     //<---- Need to include this namespace

namespace SNOEC_GUI
{
    class USB_SPI
    {
        private static MCP2210.DevIO UsbSpi;
        private static bool isConnected = false;           // Connection status variable for MCP2210
        private USB_SPI() { }

        private static bool Connect()
        {
            if (isConnected == false)
            {
                //Variables
                const uint MCP2210_VID = 0x04D8;   // VID for Microchip Technology Inc.
                const uint MCP2210_PID = 0x00DE;   // PID for MCP2210   

                //STEP 2:
                //	Make an instance of the MCP2210.DevIO class by calling 
                //	the class constructor with the device VID and PID.
                UsbSpi = new DevIO(MCP2210_VID, MCP2210_PID);

                //	we choose to check the connection status.
                isConnected = UsbSpi.Settings.GetConnectionStatus();
            }

            return isConnected;
        }   
        
        private static bool Config(ushort bytenum)
        {
            if (Connect())
            {
                //  Set up the SPI transfer settings first
                int rslt = UsbSpi.Settings.SetAllSpiSettings(MCP2210.DllConstants.CURRENT_SETTINGS_ONLY, 6000000, 0xFFFF, 0xFFEF, 0, 0, 0, bytenum, 0);
                if (rslt != 0)
                {
                    return false;
                }
                else
                    return true;
            }

            return false;
        }

        //bytenum: bytes of senddata
        public static bool OperateData(byte[] dataToSend, byte[] dataToReceive, ushort bytenum)
        {
            if (Config(bytenum))
            {
                // Create array for received data and data to send and place data in array             
                //byte[] dataToReceive = new byte[16];
                //byte[] dataToSend = new byte[16];
                //dataToSend[0] = 0x40;   //Command for writing to the MCP23S08 
                //dataToSend[1] = 0x00;   //Address of the IODIR register
                //dataToSend[2] = 0x00;   //Value to write (set all to digital outputs)

                // Transfer SPI data
                int rslt = UsbSpi.Functions.TxferSpiData(dataToSend, dataToReceive);
                if (rslt < 0)
                {
                    //Error occured -- display error                    
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
