using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Security.AccessControl;

namespace SNOEC_GUI
{
    public struct FolderPath
    {
        public static string OpticalEyeDiagram;
        public static string ElecEyeDiagram;
        public static string PlariltyEyeDiagram;
        public static string LogPath;
        public static string TestDataPath;
        public static string BackupTestDataPath;

        public static void SetValue(string[] folderPath)
        {
            foreach (string path in folderPath)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }                
            }
            OpticalEyeDiagram = folderPath[0] + "\\";
            ElecEyeDiagram = folderPath[1] + "\\";
            PlariltyEyeDiagram = folderPath[2] + "\\";
            LogPath = folderPath[3];
            TestDataPath = folderPath[4];
            BackupTestDataPath = folderPath[5];
        }

        public static void ClearFolder(string folderPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            if (directoryInfo.Exists)
            {
                //先删除整个文件夹及下所有文件
                Directory.Delete(folderPath, true);
            }

            //然后在创建文件夹
            //如果不指定DirectorySecurity权限，文件夹有时会创建不成功
            DirectorySecurity securityRules = new DirectorySecurity();
            string username = Environment.UserDomainName + @"\" + Environment.UserName;
            securityRules.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));
            securityRules.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, AccessControlType.Allow));
            Directory.CreateDirectory(folderPath, securityRules);
        }
    }

    public struct FilePath
    {
        public static string ConfigXml;
        public static string TestDataXml;
        public static string RxODataXml;
        public static string TxODataXml;
        public static string LogFile;

        public static void SaveTableToExcel(DataTable table, string fileName)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(fileName);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fileStream, Encoding.Default);

            StringBuilder title = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)//栏位：自动跳到下一单元格
            {
                title.Append(table.Columns[i].ColumnName);
                title.Append("\t");
            }
            writer.WriteLine(title);

            StringBuilder content = new StringBuilder();
            foreach (DataRow row in table.Rows)//内容：自动跳到下一单元格
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    content.Append(row[i]);
                    content.Append("\t");
                }
                content.Append("\n");
            }
            writer.Write(content);

            writer.Close();
            writer.Dispose();
            fileStream.Close();
            fileStream.Dispose();
        }
    }
}
