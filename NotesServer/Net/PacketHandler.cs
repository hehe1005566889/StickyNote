using FlyBirdCommon.Logger;
using NotesServer.Protocol.Net.Handler;
using StickyNotes.Protocol;
using StickyNotes.Protocol.Net;
using System;
using System.Collections.Generic;

namespace NotesServer.Protocol.Net
{
    class PacketHandler : IDisposable
    {
        private readonly Logger logger = SharedLoggers.GetNetLogger();
        private static PacketHandler Instance;
        public static PacketHandler GetHandler()
        {
            if (Instance is null)
                Instance = new PacketHandler();
            return Instance;
        }

        private PacketHandler()
        {
            GC.Collect();
        }

        public byte[] HandlePacket(
            int packetID,
            PacketStream stream,
            ClientModel model
        ) {
            if (!PacketHandlers.ContainsKey(packetID))
                return SendError();

            IHandler handler = PacketHandlers[packetID];
            if (!handler.ReadPacket(stream))
            {
                ClientModel.Debug("[DEBUG] Packet Reading Error");
                return SendError();
            }

            using (PacketStream data = new PacketStream()) {
                if (!handler.WriteReturn(data))
                {
                    ClientModel.Debug("[DEBUG] Packet Writing Error");
                    return SendError();
                }

                if(packetID is 0 && handler is LoginHandler handler1)
                {
                    model.IsLogin = handler1.IsLogin;
                }

                using (PacketPackage package = new PacketPackage(data))
                {
                    return package.EncodePacket(handler.GetReturnPacketID());
                }
            }
        }

        private byte[] SendError()
        {
            using (PacketStream stream = new PacketStream())
            {
                stream.WriteInt32(404);
                return new PacketPackage(stream).EncodePacket(-2);
            }
        }

        public void Dispose()
        {
            PacketHandlers.Clear();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~PacketHandler()
        {
            Dispose();
        }

        //TODO Change To Active At Using
        //     MayBe Dynamic Load The Handlers
        //     Busy -> Keep Instance
        //     Free -> Kill Instance
        private readonly Dictionary<int, IHandler> PacketHandlers = new Dictionary<int, IHandler>()
        {
            { 0, new LoginHandler()      },
            { 2, new StatusHandler()     },
            { 3, new PublicNoteHandler() },
            { 4, new GetIndexHandler()   },
            { 6, new GetTicketHandler()  }
        };
    }
}
