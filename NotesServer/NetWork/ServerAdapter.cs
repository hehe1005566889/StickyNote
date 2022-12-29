using FlyBirdCommon.Logger;
using StickyNoteCommon.Framework;
using StickyNoteCommon.Net;
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
    class ServerAdapter
    {
        public readonly PacketMapping Mapping = new PacketMapping(Assembly.GetExecutingAssembly());
        private readonly List<ClientModel> Clients = new List<ClientModel>();
        private readonly TcpListener Listener;
        private readonly Logger logger = SharedLoggers.GetNetLogger();
        private bool IsCallToDead = false;

        public ServerAdapter()
        {
            logger.Info("Server Start At Port {0}", 9090.ToString());
            Listener = new TcpListener(IPAddress.Any, 9090);
            var publicIP = "127.0.0.1";
            logger.Success("Connect Code {0}", BuildConnectCode(publicIP, 9090));

            Mapping.RegisterPackets();
            Listener.Start();
            _ = Task.Run(OnListen);
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
        
        private async Task OnListen()
        {
            Socket accept = null;
            try
            {
                accept = await Listener.AcceptSocketAsync();
                logger.Info("Server Stop To Listening The New Connection At Now :)");
                Clients.Add(new ClientModel(accept, this));
                GC.Collect();
            }
            catch (Exception e)
            {
                logger.Error("Error When Listen Connect {0}", e.ToString());
            }

            if (IsCallToDead)
                KillClientModel();
            else
                _ = Task.Run(OnListen);
        }

        private void KillClientModel()
        {
            //TODO Kill NMB
            GC.Collect();
        }

        public void UnRegisterClient(ClientModel model)
        {
            Clients.Remove(model);
            GC.SuppressFinalize(model);
            GC.Collect();
        }

        public void Dispose()
        {
            IsCallToDead = true;
            Listener.Stop();
            GC.SuppressFinalize(Listener);
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
