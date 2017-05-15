using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ivi.Visa.Interop;
using System.Threading;
using System.Runtime.InteropServices;

namespace SNOEC_GUI
{
    public class IOPort
    { 
        [DllImport("CH375DLL.dll")]
        static extern int CH375ResetDevice(byte iIndex);// 复位CH375设备,返回句柄,出错则无效

        [DllImport("CH375DLL.dll")]
        static extern int CH375OpenDevice(byte iIndex);// 打开CH375设备,返回句柄,出错则无效

        [DllImport("CH375DLL.dll")]
        static extern int CH375GetUsbID(byte iIndex);//关闭USB设备，此函数一定要被调用。建议在关闭设备并退出应用程序后再拔出USB电缆。

        [DllImport("CH375DLL.dll")]
        static extern int CH375CloseDevice(byte iIndex);//关闭USB设备，此函数一定要被调用。建议在关闭设备并退出应用程序后再拔出USB电缆。

        [DllImport("CH375DLL.dll")]
        static extern void CH375SetTimeout(byte iIndex, ushort iwritetimeout, ushort ireadtimeout);

        [DllImport("CH375DLL.dll")]
        static extern int CH375ReadData(Int16 iIndex, // 指定USB设备序号，下同
                                       [MarshalAs(UnmanagedType.LPArray)] byte[] buff, // 指向一个足够大的缓冲区,用于保存读取的数据
                                       ref int ioLength); // 指向长度单元,输入时为准备读取的长度,返回后为实际读取的长度，最大长度为64个字节。

        [DllImport("CH375DLL.dll")]
        static extern bool CH375WriteData(Int16 iIndex, // 指定USB设备序号 
                                        [MarshalAs(UnmanagedType.LPArray)] byte[] buff, // 指向一个缓冲区,放置准备写出的数据
                                        ref int ioLength); // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度，最大长度为64个字节。

        public static bool CH375WriteData(int iIndex, byte[] iBuffer) // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度，最大长度为64个字节。
        {
            int l = iBuffer.Length;
            return CH375WriteData((byte)iIndex, iBuffer, ref l);
        }

        public static int CH375ReadData(int iIndex, byte[] iBuffer) // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度，最大长度为64个字节。
        {
            int l = iBuffer.Length;
            CH375ReadData((byte)iIndex, iBuffer, ref l);
            return l;
        }

        public static byte[] CH375ReadData(int iIndex, int len) // 指向长度单元,输入时为准备写出的长度,返回后为实际写出的长度，最大长度为64个字节。
        {
            byte[] buff = new byte[len];
            int l = buff.Length;
            CH375ReadData((byte)iIndex, buff, ref l);

            byte[] values = new byte[l];
            Array.Copy(buff, values, l);
            return values;
        }

        public static void CloseDevice(int deviceSelect)
        {
            CH375CloseDevice((byte)deviceSelect);
        }

        public enum ReadWrite : byte
        {
            Read = 0,
            Write = 1,
        }

        public enum SoftHard : byte
        {
            HARDWARE_SEQUENT = 0xA8,
            SOFTWARE_SEQUENT = 0xA9,
            HARDWARE_SINGLE = 0xA3,
            SOFTWARE_SINGLE = 0xA4,
            OnEasyB_I2C = 0x00,
        }

        public enum MDIOSoftHard : byte
        {
            HARDWARE = 0xAB,
            SOFTWARE = 0xA7,
        }

        public static bool ResetDevice(int deviceSelect)
        {
            int x = CH375ResetDevice((byte)deviceSelect);
            return x != 0;
        }

        public static bool OpenDevice(int deviceSelect)
        {
            int x = CH375ResetDevice((byte)deviceSelect);
            // Thread.Sleep(20);
            int r = CH375OpenDevice((byte)deviceSelect);
            // Thread.Sleep(10);
            return r != 0;
        }

        public static int ReadID(int driverIndex)
        {
            return IDDetect((byte)driverIndex, ReadWrite.Read, 0);
        }

        public static int IDDetect(int driverSelect, ReadWrite readWrtie, byte idInput)
        {
            CH375SetTimeout((byte)driverSelect, 1000, 1000);

            byte[] buff = new byte[5];
            buff[0] = buff[1] = 0;
            buff[2] = 0xB3;
            buff[3] = (byte)readWrtie;
            buff[4] = idInput;

            CH375WriteData(driverSelect, buff);

            byte[] readBuff = new byte[1];
            CH375ReadData(driverSelect, readBuff);

            return readBuff[0];
        }

        public enum CFKType : byte
        {
            _400K = 0,
            _200K = 1,
            _100K = 2,
            _50K = 3,
            _25K = 4,
        }

        public enum MDIOCFKType : byte
        {
            _4M = 0,
            _2M = 1,
            _1M = 2,
            _500K = 3,
            _250K = 4,
            _125K = 5,
            _625K = 6,//62.5K
        }

        static Semaphore semaphore = new Semaphore(1, 1);

        public static byte[] ReadWriteReg(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide, SoftHard softHard,
            ReadWrite operate, CFKType cfk, byte[] buffer)
        {
            semaphore.WaitOne();
            OpenDevice(deviceIndex);
            CH375SetTimeout((byte)deviceIndex, 100, 100);

            byte[] arr = new byte[buffer.Length + 8];

            arr[0] = regAddressWide ? (byte)1 : (byte)0;
            arr[1] = (byte)(regAddress / 256);
            arr[2] = (byte)softHard;
            arr[3] = (byte)operate;
            arr[4] = (byte)deviceAddress;
            arr[5] = (byte)(regAddress & 0xFF);
            arr[6] = (byte)buffer.Length;
            arr[7] = (byte)cfk;
            buffer.CopyTo(arr, 8);
            bool b = CH375WriteData(deviceIndex, arr);
            System.Threading.Thread.Sleep(50);
            byte[] arrRead = CH375ReadData(deviceIndex, buffer.Length);
            CloseDevice(deviceIndex);
            semaphore.Release();
            //  System.Threading.Thread.Sleep(50);
            //  System.Threading.Thread.Sleep(100);
            return arrRead;


        }

        public static byte[] ReadWriteMDIO(int deviceIndex, int deviceAddress, int phycialAddress, int regAddress, bool regAddressWide, MDIOSoftHard softHard,
            ReadWrite operate, MDIOCFKType cfk, byte[] buffer)
        {


            //  log.AdapterLogString(3, ex.Message);
            semaphore.WaitOne();
            CH375ResetDevice((byte)deviceIndex);
            OpenDevice(deviceIndex);
            CH375SetTimeout((byte)deviceIndex, 100, 100);

            byte[] arr = new byte[buffer.Length + 10];
            arr[0] = 0;
            arr[1] = 0;
            arr[2] = (byte)softHard;
            arr[3] = (byte)operate;
            arr[4] = (byte)cfk;
            arr[5] = (byte)phycialAddress;
            arr[6] = (byte)deviceAddress;
            arr[7] = (byte)(regAddress / 256);
            arr[8] = (byte)(regAddress & 0xFF);
            arr[9] = (byte)(buffer.Length / 2);
            buffer.CopyTo(arr, 10);
            bool b = CH375WriteData(deviceIndex, arr);
            System.Threading.Thread.Sleep(50);
            if ((byte)operate == 1)
            {
                int k = 0;
                do
                {
                    System.Threading.Thread.Sleep(50);
                    byte[] ACK = CH375ReadData(deviceIndex, buffer.Length);
                    if (ACK[0] == 0xAA && ACK[1] == 0xAA)
                    {
                        break;
                    }
                    k++;
                } while (k < 6);
            }

            byte[] arrRead = CH375ReadData(deviceIndex, buffer.Length);
            System.Threading.Thread.Sleep(100);
            CloseDevice(deviceIndex);
            semaphore.Release();
            //System.Threading.Thread.Sleep(50);
            return arrRead;


        }

        public static byte[] ReadWriteMDIO(int deviceIndex, int deviceAddress, int phycialAddress, int regAddress, bool regAddressWide, MDIOSoftHard softHard,
           ReadWrite operate, MDIOCFKType cfk, UInt32 buffer)
        {
            //semaphore.WaitOne();
            OpenDevice(deviceIndex);
            CH375SetTimeout((byte)deviceIndex, 100, 100);

            byte[] arr = new byte[2 + 10];
            arr[0] = 0;
            arr[1] = 0;
            arr[2] = (byte)softHard;
            arr[3] = (byte)operate;
            arr[4] = (byte)cfk;
            arr[5] = (byte)phycialAddress;
            arr[6] = (byte)deviceAddress;
            arr[7] = (byte)(regAddress / 256);
            arr[8] = (byte)(regAddress & 0xFF);
            arr[9] = (byte)(1);
            arr[8] = (byte)(regAddress & 0xFF);
            arr[9] = (byte)(1);

            bool b = CH375WriteData(deviceIndex, arr);
            System.Threading.Thread.Sleep(100);
            byte[] arrRead = CH375ReadData(deviceIndex, 1);
            CloseDevice(deviceIndex);
            semaphore.Release();
            System.Threading.Thread.Sleep(100);

            return arrRead;
        }

        public static UInt16[] ReadMDIO(int deviceIndex, int deviceAddress, int phycialAddress, int regAddress, MDIOSoftHard softHard, int readLength)
        {            
            UInt16[] returndata = new UInt16[readLength];

            byte[] readdata = new byte[readLength * 2];
            readdata = ReadWriteMDIO(deviceIndex, deviceAddress, phycialAddress, regAddress, false, softHard, ReadWrite.Read,
                (MDIOCFKType)2, new byte[readLength * 2]);

            int j = 0;
            for (int i = 0; i < readdata.Length; i = i + 2)
            {
                returndata[j] = (UInt16)((readdata[i] * 256) + readdata[i + 1]);
                j++;
            }
            
            return returndata;
        }

        public static byte[] WriteMDIO(int deviceIndex, int deviceAddress, int phycialAddress, int regAddress, MDIOSoftHard softHard, UInt16[] dataToWrite)
        {
            byte[] writedata = new byte[dataToWrite.Length * 2];
            int j = 0;
            for (int i = 0; i < dataToWrite.Length; i++)
            {
                writedata[j] = (byte)(dataToWrite[i] / 256);
                writedata[j + 1] = (byte)(dataToWrite[i] & 0xFF);
                j = j + 2;
            }
            return ReadWriteMDIO(deviceIndex, deviceAddress, phycialAddress, regAddress, false, softHard, ReadWrite.Write, (MDIOCFKType)2, writedata);
        }

        public static bool WritePort(int id, int deviceIndex, int Port, int DDR)
        {
            OpenDevice(deviceIndex);
            CH375SetTimeout((byte)deviceIndex, 100, 100);
            byte[] buffer = new byte[6];
            buffer[0] = (byte)id;
            buffer[1] = 0;
            buffer[2] = 0xA2;
            buffer[3] = 1;
            buffer[4] = (byte)Port;
            buffer[5] = (byte)DDR;

            CH375WriteData(deviceIndex, buffer);
            Thread.Sleep(100);
            byte[] buf = CH375ReadData(deviceIndex, 1);
            CloseDevice(0);
            if (buf.Length < 1) return false;
            return buf[0] == 0xAA;
        }

        public static byte[] ReadPort(int id, int deviceIndex, int Port, int DDR)
        {
            OpenDevice(deviceIndex);
            CH375SetTimeout((byte)deviceIndex, 100, 100);
            byte[] buffer = new byte[6];
            buffer[0] = (byte)id;
            buffer[1] = 0;
            buffer[2] = 0xA2;
            buffer[3] = 0;
            buffer[4] = (byte)Port;
            buffer[5] = (byte)DDR;
            CH375WriteData(deviceIndex, buffer);
            Thread.Sleep(100);
            byte[] buf = CH375ReadData(deviceIndex, 1);
            CloseDevice(0);
            return buf;
        }

        public const uint tmptime = 200 + 200 * 256 * 256;
        public static byte Frequency = 4;

        public static byte[] ReadReg(int deviceIndex, int deviceAddress, int regAddress, SoftHard softHard, int readLength)
        {
            if(softHard == SoftHard.OnEasyB_I2C)
            {
                Thread.Sleep(30);
                OnEasyB_I2C.USBIO_OpenDeviceByNumber(OnEasyB_I2C.serialNumber);

                OnEasyB_I2C.USBIO_I2cSetConfig((byte)deviceIndex, (byte)deviceAddress, Frequency, tmptime);

                byte[] cmd = BitConverter.GetBytes(regAddress);
                byte[] readData = new byte[readLength];
                bool result = OnEasyB_I2C.USBIO_I2cRead((byte)deviceIndex, (byte)deviceAddress, cmd, (byte)(cmd.Length/4), readData,(ushort) readLength);

                OnEasyB_I2C.USBIO_CloseDevice((byte)deviceIndex);
                Thread.Sleep(20);
                if (result == false)
                {
                    return null;
                }
                return readData;
            }
            return ReadWriteReg(deviceIndex, deviceAddress, regAddress, false, softHard, ReadWrite.Read,
            (CFKType)0, new byte[readLength]);
        }

        public static byte[] WriteReg(int deviceIndex, int deviceAddress, int regAddress, SoftHard softHard, byte[] dataToWrite)
        {
            if (softHard == SoftHard.OnEasyB_I2C)
            {
                Thread.Sleep(30);
                OnEasyB_I2C.USBIO_OpenDeviceByNumber(OnEasyB_I2C.serialNumber);
                OnEasyB_I2C.USBIO_I2cSetConfig((byte)deviceIndex, (byte)deviceAddress, Frequency, tmptime);

                byte[] cmd = BitConverter.GetBytes((byte)regAddress);
                bool result = OnEasyB_I2C.USBIO_I2cWrite((byte)deviceIndex, (byte)deviceAddress, cmd, (byte)(cmd.Length/2), dataToWrite, (ushort)dataToWrite.Length);
                
                byte[] readData = new byte[dataToWrite.Length];
                result = OnEasyB_I2C.USBIO_I2cRead((byte)deviceIndex, (byte)deviceAddress, cmd, (byte)(cmd.Length / 4), readData, (ushort)readData.Length);
                
                OnEasyB_I2C.USBIO_CloseDevice((byte)deviceIndex);
                Thread.Sleep(20);

                if (result == false)
                {
                    return null;
                }
                return readData;
            }
            return ReadWriteReg(deviceIndex, deviceAddress, regAddress, false, softHard, ReadWrite.Write,
                (CFKType)0, dataToWrite);
        }


        private static volatile IOPort instance = null;
        private static object syncRoot_GPIB = new Object();
        private static object syncRoot_RS232 = new Object();
        private static volatile Dictionary<string, IMessage> dicMyDmm = null;
        private Ivi.Visa.Interop.ResourceManager rm; // VIsa  GPIB
        private Ivi.Visa.Interop.FormattedIO488 myDmm; // VIsa  GPIB

        private IOPort() { }

        public static IOPort GetIOPort()
        {
            if (instance == null)
            {
                lock (syncRoot_GPIB)
                {
                    if (instance == null)
                    {
                        instance = new IOPort();
                    }
                }
            }
            return instance;
        }

        public enum Type: int
        {
            GPIB = 0,
            USB = 1,
            NIUSB = 2,
            RJ45 = 3,
            RS232 = 4
        }

        public enum Status : int
        {
            Pass = 0,
            Failed = 1,
        }

        public dynamic ReadIEEEBlock(Type type, string ioaddr, string str_Write)
        {
            lock (syncRoot_GPIB)
            {
                this.WriteString(type, ioaddr, str_Write);
                return myDmm.ReadIEEEBlock(IEEEBinaryType.BinaryType_UI1);
            }
        }

        public bool WriteString(Type type, string ioaddr, string str_Write)
        {
            return this.WriteReadString(type, ioaddr, str_Write, ReadWrite.Write) != Status.Failed.ToString();
        }

        public string ReadString(Type type, string ioaddr, int count = 0)
        {
            return this.WriteReadString(type, ioaddr, null, ReadWrite.Read, count);
        }

        private string WriteReadString(Type type, string ioaddr, string str_Write, ReadWrite operation, int count = 0)
        {
            string buf = Status.Failed.ToString();
            switch (type)
            {                    
                case Type.GPIB:
                    buf = this.WriteReadString_GPIB(ioaddr, str_Write, operation, count);
                    break;

                case Type.USB:
                    break;

                case Type.NIUSB:
                    buf = this.WriteReadString_NIUSB(ioaddr, str_Write, operation, count);
                    break;

                case Type.RJ45:// 网口
                    break;

                case Type.RS232:// 网口
                    buf = this.WriteReadString_RS232(str_Write, operation, count);
                    break;
            }
            return buf;                
        }

        private string WriteReadString_GPIB(string ioaddr, string str_Write, ReadWrite operation, int count = 0)
        {
            string buf = Status.Failed.ToString();
            try
            {
                lock (syncRoot_GPIB)
                {
                    if (rm == null || myDmm == null || myDmm.IO == null)
                    {
                        rm = new Ivi.Visa.Interop.ResourceManager(); //Open up a new resource manager
                        myDmm = new Ivi.Visa.Interop.FormattedIO488(); //Open a new Formatted IO 488 session 
                        myDmm.IO = (IMessage)rm.Open(ioaddr, AccessMode.NO_LOCK, 5000, ""); //Open up a handle to the DMM with a 2 second timeout
                        myDmm.IO.Timeout = 5000; //You can also set your timeout by doing this command, sets to 3 seconds
                                                 //myDmm.IO.Clear(); //Send a device clear first to stop any measurements in process    
                                                 //myDmm.WriteString("*RST", true);  
                        dicMyDmm = new Dictionary<string, IMessage>();
                        dicMyDmm.Add(ioaddr, myDmm.IO);
                    }

                    if (myDmm.IO != null && dicMyDmm.ContainsKey(ioaddr))
                    {
                        myDmm.IO = dicMyDmm[ioaddr];
                    }
                    else
                    {
                        Thread.Sleep(100);
                        myDmm.IO = (IMessage)rm.Open(ioaddr, AccessMode.NO_LOCK, 5000, ""); //Open up a handle to the DMM with a 2 second timeout
                        myDmm.IO.Timeout = 5000; //You can also set your timeout by doing this command, sets to 3 seconds
                        dicMyDmm.Add(ioaddr, myDmm.IO);
                    }


                    //if (myDmm.IO != null && myDmm.IO.ResourceName != ioaddr + "::INSTR")
                    //{
                    //    Thread.Sleep(100);
                    //    myDmm.IO = (IMessage)rm.Open(ioaddr, AccessMode.NO_LOCK, 5000, ""); //Open up a handle to the DMM with a 2 second timeout
                    //    myDmm.IO.Timeout = 5000; //You can also set your timeout by doing this command, sets to 3 seconds
                    //    Thread.Sleep(100);
                    //}
                    //myDmm.IO.Clear(); //Send a device clear first to stop any measurements in process    
                    //myDmm.WriteString("*RST", true);

                    if (operation == ReadWrite.Write)
                    { 
                        myDmm.WriteString(str_Write, true);
                        buf = Status.Pass.ToString();
                    }
                    else if (count == 0)
                    {
                        buf = myDmm.ReadString();
                        //myDmm.IO.Clear(); //Send a device clear first to stop any measurements in process 
                    }
                    else
                    {
                        byte[] arr = new byte[count];
                        arr = myDmm.IO.Read(count);
                        buf = System.Text.Encoding.Default.GetString(arr);
                    }
                    Thread.Sleep(50);
                    return buf;
                }
            }
            catch
            {
                //Log.SaveLogToTxt(ex.Message + "\r\n" + "failed to operate GPIB");
                return buf;
            }
        }

        private string WriteReadString_NIUSB(string ioaddr, string str_Write, ReadWrite operation, int count = 0)
        {
            return this.WriteReadString_GPIB(ioaddr, str_Write, operation, count);
        }

        private string WriteReadString_RS232(string str_Write, ReadWrite operation, int count = 0)
        {
            //need code
            lock (syncRoot_RS232)
            {
                //connect
                //writestring  or readstring
            }
            return null;
        }
    }
}
