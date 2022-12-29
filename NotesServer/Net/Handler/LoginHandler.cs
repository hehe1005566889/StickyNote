using NotesServer.Controller;
using StickyNotes.Protocol;
using System;

namespace NotesServer.Protocol.Net.Handler
{
    class LoginHandler : IHandler
    {
        private readonly LoginFactory Factory = LoginFactory.GetLoginFactory();
        public bool IsLogin;
        public LoginHandler()
        {
            GC.Collect();
        }

        public int GetReturnPacketID() => 0;

        public bool ReadPacket(PacketStream DataStream)
        {
            var name = DataStream.ReadString();

            var pass = DataStream.ReadString();

            IsLogin = Factory.Login(
                name, 
                pass
            );

            return true;
        }

        public bool WriteReturn(PacketStream ResultStream)
        {
            ResultStream.WriteBoolean(IsLogin);
            return true;
        }
    }
}
