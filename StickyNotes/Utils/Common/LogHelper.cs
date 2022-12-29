using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace HTBInject.Utils
{
    internal class LogHelper
    {
        public LogHelper(string sender)
        {
            Sender = sender;
        }

        private string Sender;

        public enum LogType
        {
            Error,
            Success,
            Info,
            Warning,
            Default
        }

        public void SendLog(object Message, LogType type)
        {
            StringBuilder msgToSend = new StringBuilder();
            msgToSend.Append("[").Append(DateTime.Now.ToLocalTime().ToString()).Append("] ");
            switch (type)
            {
                case LogType.Default:
                    msgToSend.Append("[Default] ");
                    break;
                case LogType.Error:
                    msgToSend.Append("[Error] ");
                    break;
                case LogType.Info:
                    msgToSend.Append("[Info] ");
                    break;
                case LogType.Success:
                    msgToSend.Append("[Success] ");
                    break;
                case LogType.Warning:
                    msgToSend.Append("[Warning] ");
                    break;
            }
            msgToSend.Append(Sender).Append(" : ").Append(Message);
            LogSever.AddLog(msgToSend.ToString());
            GC.Collect();
        }

        public void SendLog(object Message)
        {
            StringBuilder msgToSend = new StringBuilder();
            msgToSend.Append("[").Append(DateTime.Now.ToLocalTime().ToString()).Append("] ");
            msgToSend.Append(Sender).Append(" : ").Append(Message);
            Console.WriteLine(msgToSend.ToString());
        }

        public void Release() => Sender = null;
    }

    class LogSever
    {
        private static List<string> logs = new List<string>();
        public static string LogsFileLocation { set; get; } = "logs.log";
        public static int SaveS { set; get; } = 5;

        public static void AddLog(string log)
        {
            Console.WriteLine(log);
            logs.Add(log);
        }

        public static void Init()
        {
            logs.Add("========================[New]Application Run");
            Thread thd = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(SaveS * 1000);
                    if (logs.Count - 1 > 0)
                    {
                        File.AppendAllLines(LogsFileLocation, logs);
                        logs.Clear();
                    }
                }
            });
            thd.Start();
        }
    }
}