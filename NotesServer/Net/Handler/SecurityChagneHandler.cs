using NotesServer.Controller;
using StickyNotes.Protocol;
using System;

namespace NotesServer.Protocol.Net.Handler
{
    class SecurityChagneHandler : IHandler
    {
        public SecurityChagneHandler()
        {
            GC.Collect();
        }

        public int GetReturnPacketID() => 1;
        private readonly LoginFactory Factory = LoginFactory.GetLoginFactory();
        private bool IsDone = false;

        public bool ReadPacket(PacketStream DataStream)
        {
            var name = DataStream.ReadString();
            var pass = DataStream.ReadString();
            var new_pass = DataStream.ReadString();

            IsDone = Factory.ChangePassWord(
                name,
                pass,
                new_pass
            );

            return true;
        }

        public bool WriteReturn(PacketStream ResultStream)
        {
            ResultStream.WriteInt32((int)ResultStatus.CHANGEPASSWORD);
            ResultStream.WriteBoolean(IsDone);
            return true;
        }
    }
}
