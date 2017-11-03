using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace SNOEC_GUI
{
    public class Algorithm
    {
        public const ushort MyNaN = 9999;            

        /// <summary>
        /// using regular expressions to check format of serial number
        /// </summary>
        /// <param name="serialNumber">in serial number</param>
        /// <returns>true or false</returns>
        public static bool CheckSerialNumberFormat(string serialNumber)
        {
            Regex regex = new Regex(@"^(?![0-9]+$)(?![A-Z]+$)[0-9A-Z]{12}$", RegexOptions.IgnorePatternWhitespace);
            return regex.IsMatch(serialNumber);
            //分开来注释一下：
            //^ 匹配一行的开头位置
            //(?![0 - 9] +$) 预测该位置后面不全是数字
            //(?![A - Z] +$) 预测该位置后面不全是大写字母
            //[0 - 9A - Z] {12}
            //由12位数字或大写字母组成
            //$ 匹配行结尾位置

            //注：(? !xxxx) 是正则表达式的负向零宽断言一种形式，标识预该位置后不是xxxx字符。
        }

        public static byte[] ObjectToByteArray(object inputData, byte length, bool isLittleendian)
        {
            ArrayList array = new ArrayList();
            double value = Convert.ToDouble(inputData);
            switch (length)
            {
                case 0:
                    break;

                case 1:
                    byte temp_8bits = (byte)value;
                    array = ArrayList.Adapter(new byte[1] { temp_8bits });
                    break;

                case 2:
                    UInt16 temp_16bits = (UInt16)value;
                    array = ArrayList.Adapter(BitConverter.GetBytes(temp_16bits));                    
                    break;

                case 4:
                    UInt32 temp_32bits = (UInt32)value;
                    array = ArrayList.Adapter(BitConverter.GetBytes(temp_32bits));                    
                    break;

                default:
                    break;
            }
            
            if (isLittleendian == false)
            {
                array.Reverse();
            }
            return (byte[])array.ToArray(typeof(byte));
        }

        /// <summary>
        /// DAC是否需要进行位处理
        /// </summary>
        /// <param name="length">字节长度</param>
        /// <param name="StartBit">起始位</param>
        /// <param name="EndBit">结束位</param>
        /// <returns>True=处理;falas=不处理</returns>
        public static bool BitNeedManage(int length, int StartBit, int EndBit)
        {
            int Bitlength = EndBit - StartBit + 1;

            if (length * 8 > Bitlength)// 位数不够,拼凑而成
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 对于要写入DAC的值进行位计算
        /// </summary>
        /// <param name="writeData">要写的数值</param>
        /// <param name="readData">已经从寄存器读出的数值</param>
        /// <param name="length">字节长度</param>
        /// <param name="startBit">起始位</param>
        /// <param name="endBit">结束位</param>
        /// <param name="type_MCU">Mcu类型,1:8bit,2:16bit</param>
        /// <returns></returns>
        public static int WriteJointBitValue(int writeData, int readData, int length, int startBit, int endBit, int type_MCU = 1)
        {
            int A = 0;
            for (int i = 0; i < length * 8 * type_MCU; i++)
            {
                if (i < startBit || i > endBit)//如果是在它的位置之外,那么全部写0
                {
                    A += Convert.ToInt32(Math.Pow(2, i));
                }
            }

            int b = readData & A;//吧要写的bit写0
            int c = b + writeData * Convert.ToInt32(Math.Pow(2, startBit));
            return c;
        }

        /// <summary>
        /// 对于读出DAC的值进行位计算
        /// </summary>
        /// <param name="readData">已经从寄存器读出的数值</param>
        /// <param name="length">字节长度</param>
        /// <param name="startBit">起始位</param>
        /// <param name="endBit">结束位</param>
        /// <param name="type_MCU">MCU类型,1:8bit,2:16bit</param>
        /// <returns></returns>
        public static int ReadJointBitValue(int readData, int length, int startBit, int endBit, int type_MCU = 1)
        {
            int A = 0;
            for (int i = 0; i < length * 8 * type_MCU; i++)
            {
                if (i >= startBit && i <= endBit)//如果是在它的位置之外,那么全部写0
                {
                    A += Convert.ToInt32(Math.Pow(2, i));
                }
            }
            int b = readData & A;//吧要写的bit写0
            int c = b / Convert.ToInt32(Math.Pow(2, startBit));
            return c;
        }

        ///用最小二乘法拟合二元多次曲线
        ///</summary>
        ///<param name="arrX">已知点的x坐标集合</param>
        ///<param name="arrY">已知点的y坐标集合</param>
        ///<param name="length">已知点的个数</param>
        ///<param name="dimension">方程的最高次数</param>
        public static double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension) //二元多次线性方程拟合曲线
        {
            int n = dimension + 1;
            if (length == 2 && dimension == 2)
            {
                n = 2;
            }
            if (length <= 1)
            {
                n = 1;
            }
            //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }
            if (length == 2 && dimension == 2)
            {
                double[] temp = ComputGauss(Guass, n);
                double[] coef = new double[temp.Length + 1];
                for (byte i = 0; i < temp.Length; i++)
                {
                    coef[i] = temp[i];
                }
                return coef;
            }
            if (length <= 1)
            {
                if (dimension == 1)
                {
                    double[] temp = ComputGauss(Guass, n);
                    double[] coef = new double[temp.Length + 1];
                    for (byte i = 0; i < temp.Length; i++)
                    {
                        coef[i] = temp[i];
                    }
                    return coef;
                }
                if (dimension == 2)
                {
                    double[] temp = ComputGauss(Guass, n);
                    double[] coef = new double[temp.Length + 2];
                    for (byte i = 0; i < temp.Length; i++)
                    {
                        coef[i] = temp[i];
                    }
                    return coef;
                }

            }
            return ComputGauss(Guass, n);
        }

        private static double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }

        private static double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                //if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))//屏蔽掉arr1[i]=0的情况
                if ((arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }

        private static double SumArr(int[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }

        private static double SumArr(int[] arr1, int n1, int[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;
        }

        private static double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];

            for (i = 0; i < n; i++) x[i] = 0.0;//初始化


            for (j = 0; j < n; j++)
            {
                max = 0;

                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }



                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;

                    }
                }

                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 

                    return x;
                }


                for (i = j + 1; i < n; i++)
                {

                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);

                    }
                }


            }//结束for (j=0;j<n;j++)


            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }

                x[i] = (Guass[i, n] - s) / Guass[i, i];

            }

            return x;
        }//返回值是函数的系数

        public static double ChangeUwtoDbm(double uw)
        {
            return Getlog10(uw / 1000) * 10;
        }

        public static double ChangeDbmtoUw(double dbm)
        {
            return Math.Pow(10, dbm / 10) * 1000;
        }

        public static double[] Getlog10(double[] input)
        {
            double[] inPut = new double[input.Length];
            //inPut=input;
            for (byte i = 0; i < input.Length; i++)
            {
                inPut[i] = System.Math.Log10(input[i]);

            }
            return inPut;
        }

        public static double Getlog10(double input)
        {
            double inPut = input;
            //inPut=input;
            inPut = Math.Log10(input);
            return inPut;
        }

        public static double CalculateFromOMAtoDBM(double oma, double ER)
        {
            try
            {
                ER = Math.Pow(10, ER / 10.0);
                oma = ChangeDbmtoUw(oma);
                double dbm = oma * (ER + 1) / (2 * (ER - 1));
                dbm = ChangeUwtoDbm(dbm);
                if (double.IsInfinity(oma) || double.IsNaN(oma))
                {
                    dbm = -100000;
                }
                return dbm;
            }
            catch
            {
                return MyNaN;
            }
        }

        public static ArrayList StringtoArraylistDeletePunctuations(string inputString, char[] segregatechars)
        {
            string segregatestring = "";
            for (byte i = 0; i < segregatechars.Length; i++)
            {
                segregatestring += segregatechars[i];
            }
            if (inputString == null)
            {
                return null;
            }
            if (inputString.Length == 0)
            {
                return null;
            }
            return ArrayList.Adapter(inputString.Split(segregatechars));
        }

        public static double CalculateOMA(double PVG, double ER)
        {
            try
            {
                ER = Math.Pow(10, ER / 10.0);
                PVG = ChangeDbmtoUw(PVG);
                double oma = 2 * PVG * (ER - 1) / (ER + 1);
                oma = ChangeUwtoDbm(oma);
                if (double.IsInfinity(oma) || double.IsNaN(oma))
                {
                    oma = MyNaN;
                }
                return oma;
            }
            catch
            {
                return MyNaN;
            }
        }

        // cense test algorithm
        public static double LinearRegression(double[] x, double[] yList, out double slop, out double intercept)
        {
            double result = 0;
            double β;
            double α;
            slop = 0;
            intercept = 0;
            int n = x.Length; //个数（n）
            if (n == 0) { return 0; }
            if (n == 1)
            {
                intercept = 0;
                slop = yList[0];
                return yList[0];
            }
            double predictX = x[n - 1] + 1; //预测指标值
            //∑XiYi
            double sumXiYi = 0;
            //∑Xi
            double sumXi = 0;
            //∑Yi
            double sumYi = 0;
            //∑（Xi二次方）
            double XiSqrtSum = 0;
            for (int i = 0; i < n; i++)
            {
                sumXiYi += x[i] * yList[i];
                sumXi += x[i];
                sumYi += yList[i];
                XiSqrtSum += x[i] * x[i];
            }
            //∑Xi∑Yi
            double sumXisumYi = sumXi * sumYi;
            //（∑Xi）二次方
            double sumXiSqrt = sumXi * sumXi;
            //β= (n * ∑XiYi - ∑Xi * ∑Yi) / (n * ∑(Xi二次方) - (∑Xi)二次方)
            β = ((n * sumXiYi) - (sumXi * sumYi)) / ((n * XiSqrtSum) - sumXiSqrt);
            //α= (∑Yi / n) - β*(∑Xi / n)
            α = (sumYi / n) - β * (sumXi / n);
            //预测结果 = α+β* 预测指标值
            slop = α;
            intercept = β;
            result = α + (β * predictX);
            return Math.Round(result, 2);
        }

        public static double[] GetNegative(double[] input)
        {
            double[] inPut = new double[input.Length];
            for (byte i = 0; i < input.Length; i++)
            {
                inPut[i] = input[i] * (-1);
            }
            return inPut;
        }

        public static double SelectMaxValue(ArrayList inPutArray, out byte maxIndex)
        {
            maxIndex = 0;
            int firstitemindex = inPutArray.IndexOf(0);
            float tempMinValue = float.Parse(inPutArray[0].ToString());
            for (byte i = 0; i < inPutArray.Count; i++)
            {
                if (tempMinValue < float.Parse(inPutArray[i].ToString()))
                {
                    tempMinValue = float.Parse(inPutArray[i].ToString());
                    maxIndex = i;
                }

            }
            return double.Parse(inPutArray[maxIndex].ToString());
        }

        //将16进制字符串转化为 字节流
        public static byte[] HexStringToBytes(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
            {
                return new byte[0];
            }

            if (hexStr.StartsWith("0x"))
            {
                hexStr = hexStr.Remove(0, 2);
            }

            var count = hexStr.Length;

            if (count % 2 == 1)
            {
                throw new ArgumentException("Invalid length of bytes:" + count);
            }

            var byteCount = count / 2;
            var result = new byte[byteCount];
            for (int ii = 0; ii < byteCount; ++ii)
            {
                var tempBytes = Byte.Parse(hexStr.Substring(2 * ii, 2), System.Globalization.NumberStyles.HexNumber);
                result[ii] = tempBytes;
            }
            Array.Reverse(result);
            return result;
        }

        //将字节流转化为16进制字符串
        public static string BytesTohexString(byte[] bytes)
        {
            if (bytes == null || bytes.Count() < 1)
            {
                return string.Empty;
            }

            var count = bytes.Count();
            Array.Reverse(bytes);
            var cache = new StringBuilder();
            cache.Append("0x");
            for (int ii = 0; ii < count; ++ii)
            {
                var tempHex = Convert.ToString(bytes[ii], 16).ToUpper();
                cache.Append(tempHex.Length == 1 ? "0" + tempHex : tempHex);
            }

            return cache.ToString();
        }
    }
}
