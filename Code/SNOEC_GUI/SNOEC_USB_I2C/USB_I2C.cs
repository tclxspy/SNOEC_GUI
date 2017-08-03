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


        private static volatile Dictionary<string, SerialPort> dicMyPort = null;
        private static volatile SerialPort _serialPort;

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
                System.Threading.Thread.Sleep(20);

                if (operate == ReadWrite.Read)
                {
                    _serialPort.Read(readBytes, 0, buffer.Length);
                }
                
                _serialPort.Close();
                
                //semaphore.Release();
                System.Threading.Thread.Sleep(10);
                return readBytes;
            }
            catch(Exception ex)
            {
                //semaphore.Release();
                _serialPort.Close();
                _serialPort = null;
                readBytes = null;
                return readBytes;
            }            
        }
    }
}
