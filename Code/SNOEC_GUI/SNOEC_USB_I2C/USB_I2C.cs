using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace SNOEC_USB_I2C
{
    public class USB_I2C
    {
        //private static Semaphore semaphore = new Semaphore(1, 1);
        public static volatile bool i2c_soft_enable = true;  //soft: 1; hard: 0
        public static volatile bool I2C_standard_speed = false;//400k:0, 100k:1

        private USB_I2C() { }

        private enum ReadWrite : byte
        {
            Read = 0,
            Write = 1,
        }

        public static byte[] ReadPort(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide, byte[] buffer)
        {
            if (buffer.Length > 255)
            {
                return null;
            }

            if (buffer.Length <= 56)
            {
                for (int count = 0; count < 3; count++)
                {
                    byte[] buf = ReadWriteReg(deviceIndex, deviceAddress, regAddress, regAddressWide, ReadWrite.Read, buffer);
                    if (buf != null)
                    {
                        return buf;
                    }
                }
                return null;
            }

            //USB虚拟串口一次只能穿64个字节,减去8个字节的格式位，所以一次传送56个数据
            byte[] readBytes = new byte[buffer.Length];
            int cycles = buffer.Length / 56;
            int left = buffer.Length % 56;
            byte[] buff;
            int i = 0;

            for (; i < cycles; i++)
            {
                buff = new byte[56];
                for (int count = 0; count < 3; count++)
                {
                    buff = ReadWriteReg(deviceIndex, deviceAddress, regAddress + i * 56, regAddressWide, ReadWrite.Read, buff);
                    if (buff != null)
                    {
                        break ;
                    }
                }

                buff.CopyTo(readBytes, i * 56);
            }

            buff = new byte[left];
            for (int count = 0; count < 3; count++)
            {
                buff = ReadWriteReg(deviceIndex, deviceAddress, regAddress + i * 56, regAddressWide, ReadWrite.Read, buff);
                if (buff != null)
                {
                    break;
                }
            }

            buff.CopyTo(readBytes, i * 56);

            return readBytes;
        }

        //无回读
        public static byte[] WritePort(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide, byte[] buffer)
        {
            if (buffer.Length > 255)
            {
                return null;
            }
            const int subpackage = 56;//USB虚拟串口一次只能穿64个字节,减去8个字节的格式位，所以一次传送56个数据
            if (buffer.Length <= subpackage)
            {
                for (int count = 0; count < 3; count++)
                {
                    //Thread.Sleep(300);
                    Thread.Sleep(50);
                    byte[] buf = ReadWriteReg(deviceIndex, deviceAddress, regAddress, regAddressWide, ReadWrite.Write, buffer);
                    if (buf != null)
                    {
                        return buf;
                    }
                }
                return null;
            }

            //USB虚拟串口一次只能穿64个字节,减去8个字节的格式位，所以一次传送56个数据
            byte[] readBytes = new byte[buffer.Length];
            int cycles = buffer.Length / subpackage;
            int left = buffer.Length % subpackage;
            byte[] buff;
            int i = 0;

            for (; i < cycles; i++)
            {
                buff = new byte[subpackage];
                Array.Copy(buffer, i * subpackage, buff, 0, subpackage);
                for (int count = 0; count < 3; count++)
                {
                    Thread.Sleep(500);
                    byte[] buf = ReadWriteReg(deviceIndex, deviceAddress, regAddress + i * subpackage, regAddressWide, ReadWrite.Write, buff);
                    if (buf != null)
                    {
                        break;
                    }
                }
            }

            buff = new byte[left];
            Array.Copy(buffer, i * subpackage, buff, 0, left);
            for (int count = 0; count < 3; count++)
            {
                Thread.Sleep(500);
                buff = ReadWriteReg(deviceIndex, deviceAddress, regAddress + i * subpackage, regAddressWide, ReadWrite.Write, buff);
                if (buff != null)
                {
                    break;
                }
            }           

            return readBytes;
        }


        private static volatile Dictionary<string, SerialPort> dicMyPort = null;
        private static volatile SerialPort _serialPort;

        private static byte[] ReadWriteReg(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide,
           ReadWrite operate, byte[] buffer)
        {
            byte[] arr = new byte[buffer.Length + 8];

            arr[0] = regAddressWide ? (byte)1 : (byte)0;
            arr[1] = (byte)(regAddress / 256);
            arr[2] = i2c_soft_enable ? (byte)1 : (byte)0; ;//soft
            arr[3] = (byte)operate;
            arr[4] = (byte)deviceAddress;
            arr[5] = (byte)(regAddress & 0xFF);
            arr[6] = (byte)buffer.Length;
            arr[7] = I2C_standard_speed ? (byte)1 : (byte)0;////400k:0, 100k:1
            buffer.CopyTo(arr, 8);

            //USB虚拟串口一次只能穿64个字节
            if (arr.Length > 64)
            {
                return null;
            }

            //semaphore.WaitOne();


            byte[] readBytes = new byte[buffer.Length];
            try
            {
                string comIndex = "COM2";
                switch (deviceIndex)
                {
                    case 1:
                        comIndex = "COM2";
                        break;
                    case 2:
                        comIndex = "COM3";
                        break;
                    case 3:
                        comIndex = "COM4";
                        break;
                    case 4:
                        comIndex = "COM5";
                        break;
                    default:
                        comIndex = "COM2";
                        break;
                }

                if (_serialPort == null)
                {
                    _serialPort = new SerialPort(comIndex, 9600, Parity.None, 8, StopBits.One);
                    dicMyPort = new Dictionary<string, SerialPort>();
                    dicMyPort.Add(comIndex, _serialPort);
                }

                if (_serialPort != null && dicMyPort.ContainsKey(comIndex))
                {
                    _serialPort = dicMyPort[comIndex];
                }
                else
                {
                    _serialPort = new SerialPort(comIndex, 9600, Parity.None, 8, StopBits.One);
                    dicMyPort = new Dictionary<string, SerialPort>();
                    dicMyPort.Add(comIndex, _serialPort);
                }

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                if (_serialPort.IsOpen == true)
                {
                    _serialPort.Close();
                }

                try
                {
                    _serialPort.Open();
                }
                catch
                {
                    _serialPort.Close();
                    _serialPort.Open();
                }

                _serialPort.DiscardInBuffer();
                _serialPort.Write(arr, 0, arr.Length);
                System.Threading.Thread.Sleep(300);//support ADI MCU with FTDI USB to UART
                //System.Threading.Thread.Sleep(20);//support STM32L073 MCU with USB
                if (operate == ReadWrite.Read)
                {
                    _serialPort.Read(readBytes, 0, buffer.Length);
                }

                _serialPort.Close();

                //semaphore.Release();
                System.Threading.Thread.Sleep(10);
                return readBytes;
            }
            catch (Exception ex)
            {
                //semaphore.Release();
                _serialPort.Close();
                _serialPort = null;
                readBytes = null;
                return null;
            }
        }
    }
}
