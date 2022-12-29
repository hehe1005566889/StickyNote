using FlyBirdCommon.Logger;
using StickyNotes.Protocol.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StickyNotes.Protocol.Net
{
    [Obsolete]
    class ServerAdapter
    {
        private Thread AsyncThread { get; }
        private List<ClientModel> Clients { get; } = new List<ClientModel>();
        private TcpListener Listener { get; }
        private bool IsAlive = false;
        private readonly Logger logger = SharedLoggers.GetNetLogger();
        //public FireWorkServer Server { get; }
        public ServerAdapter()
        {
            logger.Info("Server Start At Port {0}", 9090.ToString());
            Listener = new TcpListener(IPAddress.Any, 9090);
          //  logger.Info("Please Input The Server IP > ");
            var publicIP = "127.0.0.1";
            logger.Success("Connect Code {0}", BuildConnectCode(publicIP, 9090));

            AsyncThread = new Thread(OnListen)
            {
                IsBackground = true,
                Name = "Connect Listener Thread",
                Priority = ThreadPriority.Lowest
            };
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

        public void Start()
        {
            logger.Info("Server Begin To Listener New Requests~");
            IsAlive = true;
            Listener.Start();
            AsyncThread.Start();
            GC.Collect();
        }

        public void Stop()
        {
            IsAlive = false;
            AsyncThread.IsBackground = false;
            AsyncThread.Interrupt();
            GC.Collect();
        }

        private void OnListen()
        {
            while (IsAlive)
            {
                Socket accept = null;
                try
                {
                    accept = Listener.AcceptSocket();
                }
                catch (Exception e)
                {
                    logger.Error("Error When Listen Connect {0}", e.ToString());
                }
                if (accept == null)
                {
                    logger.Warn("An Error Empty Connection :<");
                    continue;
                }

                Clients.Add(new ClientModel(accept, this));
                GC.Collect();
            }

            logger.Info("Server Stop To Listening The New Connection At Now :)");
        }

        public void UnRegisterClient(ClientModel model)
        {
            Clients.Remove(model);
            GC.SuppressFinalize(model);
            GC.Collect();
        }

        public void Dispose()
        {
            Stop();
            Listener.Stop();
            GC.SuppressFinalize(Listener);
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
