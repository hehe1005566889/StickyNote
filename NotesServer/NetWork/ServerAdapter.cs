using FlyBirdCommon.Logger;
using StickyNoteCommon.Framework;
using StickyNoteCommon.Net;
using StickyNoteCommon.Net.API;
using StickyNoteCommon.Net.Basic;
using StickyNotes.Protocol;
using StickyNotes.Protocol.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NotesServer.NetWork
{
    [AppService("serveradapter", Mode = ServiceStartUpMode.Auto)]
    sealed class ServerAdapter : IServer 
    {
        public ServerAdapter()
        {
            ServerNetWork = new ServerUtil(Assembly.GetExecutingAssembly(), ProtocolMode.TCP, this, 9090);
            logger.Success("Connect Code {0}", BuildConnectCode("127.0.0.1", 9090));
            ServerNetWork.Start();
            GC.Collect();
        }

        private string BuildConnectCode(string ip, int port)
        {
            using (PacketStream stream = new PacketStream())
            {
                stream.WriteInt32(port);
                stream.WriteString(ip);

                var orign = Convert.ToBase64String(stream.GetBytes()).Replace("A", "[_+_=]");
                return Convert.ToBase64String(Gzip.CompressBytes(Encoding.UTF8.GetBytes(orign)));
            }
        }

        public void PreConnect(ref IPacketManager manger)
        {
            manger = new ClientModel();
            Collect.Add(manger);
            GC.Collect();
        }

        public void OnStop() => GC.Collect();
    
        public void OnActive()
        {
            var info = ServerNetWork.GetInfo();
            logger.Success("Server Started Listening At Port {0}", info.Port);
            GC.SuppressFinalize(info);
            GC.Collect();
        }

        public void OnDead()
        {
            logger.Warn("Server Stop Listening, Client Will Not Able To Connect");
            GC.Collect();
        }

        private readonly ServerUtil ServerNetWork;
        private readonly Logger logger = SharedLoggers.GetNetLogger();
        private readonly PacketManagerList Collect = new PacketManagerList();
    }
}
