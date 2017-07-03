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
        private static Semaphore semaphore = new Semaphore(1, 1);

        private USB_I2C() { }

        private enum ReadWrite : byte
        {
            Read = 0,
            Write = 1,
        }

        public static byte[] ReadPort(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide, byte[] buffer)
        {
            return ReadWriteReg(deviceIndex, deviceAddress, regAddress, regAddressWide, ReadWrite.Read, buffer);
        }

        public static byte[] WritePort(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide, byte[] buffer)
        {
            return ReadWriteReg(deviceIndex, deviceAddress, regAddress, regAddressWide, ReadWrite.Write, buffer);
        }

        private static byte[] ReadWriteReg(int deviceIndex, int deviceAddress, int regAddress, bool regAddressWide,
           ReadWrite operate, byte[] buffer)
        {
            byte[] arr = new byte[buffer.Length + 8];

            arr[0] = regAddressWide ? (byte)1 : (byte)0;
            arr[1] = (byte)(regAddress / 256);
            arr[2] = (byte)0x01;//SoftHard
            arr[3] = (byte)operate;
            arr[4] = (byte)deviceAddress;
            arr[5] = (byte)(regAddress & 0xFF);
            arr[6] = (byte)buffer.Length;
            arr[7] = (byte)0;//CFKType
            buffer.CopyTo(arr, 8);

            semaphore.WaitOne();
            SerialPort _serialPort;
            byte[] readBytes = new byte[buffer.Length];
            try
            {
                string comIndex = "COM2";
                switch (deviceIndex)
                {
                    case 0:
                        comIndex = "COM1";
                        break;
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

                _serialPort = new SerialPort(comIndex, 9600, Parity.None, 8, StopBits.One);

                // Set the read/write timeouts
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;


                _serialPort.Open();
                _serialPort.Write(arr, 0, arr.Length);
                System.Threading.Thread.Sleep(20);

                if (operate == ReadWrite.Read)
                {
                    _serialPort.Read(readBytes, 0, buffer.Length);
                }
                _serialPort.Close();
                semaphore.Release();
                System.Threading.Thread.Sleep(10);
                return readBytes;
            }
            catch
            {
                semaphore.Release();
                _serialPort = null;
                readBytes = null;
                return readBytes;
            }            
        }
    }
}
