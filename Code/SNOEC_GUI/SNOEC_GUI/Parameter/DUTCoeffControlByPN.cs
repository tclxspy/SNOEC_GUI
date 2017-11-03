using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SNOEC_GUI
{
    public class DUTCoeffControlByPN: DataTable
    {
        private DataTable dt;

        public struct CoeffInfo
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public int Channel { get; set; }
            public byte Page { get; set; }
            public int StartAddress { get; set; }
            public int Length { get; set; }
            public byte Format { get; set; }
            public double Amplify { get; set; }
        }

        public DUTCoeffControlByPN(DataTable table)
        {
            dt = table;
        }        

        public CoeffInfo GetOneInfoFromTable(string itemName, int channel)
        {            
            string filter = "Name = " + "'" + itemName + "'";
            DataRow[] foundRows = dt.Select(filter);

            for (int row = 0; row < foundRows.Length; row++)
            {
                if (Convert.ToInt32(foundRows[row]["Channel"]) == channel)
                {
                    CoeffInfo coeffInfo = new CoeffInfo();
                    coeffInfo.Type = foundRows[row]["TYPE"].ToString();
                    coeffInfo.Name = itemName;
                    coeffInfo.Channel = channel;
                    coeffInfo.Page = Convert.ToByte(foundRows[row]["Page"]);
                    coeffInfo.StartAddress = Convert.ToInt32(foundRows[row]["StartAddress"]);
                    coeffInfo.Length = Convert.ToInt32(foundRows[row]["Length"]);
                    string buff = foundRows[row]["Format"].ToString();
                    switch (buff)
                    {                        
                        case "IEEE754":
                            coeffInfo.Format = 1;
                            break;
                        case "UInt16":
                            coeffInfo.Format = 2;
                            break;
                        case "UInt32":
                            coeffInfo.Format = 3;
                            break;
                        case "Int16":
                            coeffInfo.Format = 4;
                            break;

                    }
                    //1 ieee754;2 UInt16;3 UInt32
                    //coeffInfo.Amplify = Convert.ToDouble(foundRows[row]["AmplifyCoeff"]);

                    return coeffInfo;
                }
            }

            throw new IndexOutOfRangeException("No find " + itemName + "information, please check module table config");            
        }
    }
}
