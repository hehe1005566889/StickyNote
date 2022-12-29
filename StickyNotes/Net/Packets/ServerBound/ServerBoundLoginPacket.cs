using StickyNotes.Pages.Dialogs;
using StickyNotes.Protocol;
using System;

namespace StickyNotes.Net.Packets.ServerBound
{
    class ServerBoundLoginPacket : IServerBoundPacket
    {
        private LoginPage Page { get; }
        public ServerBoundLoginPacket(LoginPage page)
        {
            Page = page;
            GC.Collect();
        }

        public int GetPacketID() => 0;

        public bool ReadPacket(PacketStream DataStream)
        {
            var isLogin = DataStream.ReadBoolean();
            Console.WriteLine(isLogin);
            Page.OnResult(isLogin);
            return true;
        }
    }
}
