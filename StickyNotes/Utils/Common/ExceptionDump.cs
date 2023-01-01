using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Utils.Common
{
    public sealed class ExceptionDump
    {
        public string filePath;

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public ExceptionDump(string filePath)
        {
            if (!File.Exists(filePath))
                File.Create(filePath).Close();
            this.filePath = filePath;
            GC.Collect();
        }

        public string Read(string section, string key, string def)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, filePath);
            return temp.ToString();
        }

        public bool Write(string section, string key, string value)
        {
            long OpStation = WritePrivateProfileString(section, key, value, filePath);
            if (OpStation == 0)
            {
                return false;
            }
            return true;
        }

        public static void Log(string text)
        {
            string[] nowRes = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Split(new char[] { ' ' });
            string logFile = @"Log\" + nowRes[0] + ".log";
            if (!Directory.Exists("Log"))
                Directory.CreateDirectory("Log");
            File.AppendAllText(logFile, "[" + nowRes[1] + "] " + text + "\r\n");
            GC.Collect();
        }
    }
}
