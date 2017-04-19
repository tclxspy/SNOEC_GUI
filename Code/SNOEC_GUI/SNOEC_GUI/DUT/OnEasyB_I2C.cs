using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SNOEC_GUI
{
    public class OnEasyB_I2C
    {
        /*--------------------------------DLL function import---------------------------------*/

        [DllImport("usb2uis.dll")]
        public static extern byte USBIO_OpenDevice();
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_CloseDevice(byte bIndex);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_ResetDevice(byte bIndex, byte byDevId);
        [DllImport("usb2uis.dll")]
        public static extern byte USBIO_OpenDeviceByNumber(StringBuilder pSerial);
        [DllImport("usb2uis.dll")]
        public static extern byte USBIO_GetMaxNumofDev();
        [DllImport("usb2uis.dll")]
        public static extern byte USBIO_GetWorkMode(byte bIndex, ref byte pbyMode);
        [DllImport("usb2uis.dll",CallingConvention = CallingConvention.StdCall)]
        public static extern byte USBIO_GetSerialNo(byte bIndex, StringBuilder pSerial);
        [DllImport("usb2uis.dll")]
        public static extern byte USBIO_GetVersion(byte bIndex, byte bType, StringBuilder pVersion);

        /*here are I2c functions*/
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cGetConfig(byte bIndex, ref byte pbyAddress, ref byte pbyRate, ref UInt32 pdwTimeout);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cSetConfig(byte bIndex, byte byAddress, byte byRate, UInt32 pdwTimeout);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cAutoGetAddress(byte bIndex, ref byte byAddress);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cRead(byte bIndex, byte byAddress, byte[] byCmd, byte byComSize, byte[] byReadData, UInt16 dReadSize);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cWrite(byte bIndex, byte byAddress, byte[] byCmd, byte byComSize, byte[] byWriteData, UInt16 dWriteSize);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cReadEEProm(byte bIndex, byte byAddress, byte byType, UInt32 dwOffset, byte[] byReadData, UInt16 dReadSize);
        [DllImport("usb2uis.dll")]
        public static extern bool USBIO_I2cWriteEEProm(byte bIndex, byte byAddress, byte byType, UInt32 dwOffset, byte[] byWriteData, UInt16 dWriteSize);
 
        public static StringBuilder serialNumber;
    }
}
