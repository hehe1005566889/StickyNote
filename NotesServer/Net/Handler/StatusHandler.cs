using StickyNotes.Protocol;
using System;
using System.IO;

namespace NotesServer.Protocol.Net.Handler
{
    class StatusHandler : IHandler
    {
        public StatusHandler()
        {
            if (!File.Exists("Status.xaml"))
                File.WriteAllText(
                    "Status.xaml",
                    "<Border xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Label Content=\"一个StickyNote服务器~\"/></Border>"
                );
            GC.Collect();
        }

        public int GetReturnPacketID() => 2;
        private string UserName;

        public bool ReadPacket(PacketStream DataStream)
        {
            UserName = DataStream.ReadString();
            return true;
        }

        public bool WriteReturn(PacketStream ResultStream)
        {
            string status = File.ReadAllText("Status.xaml");
            status = status.Replace("{#user}", UserName);

            ResultStream.WriteString(status);
            return true;
        }
    }
}
