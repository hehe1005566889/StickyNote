using ProjectFChat.Network.Utils;
using StickyNotes.Protocol.Tools;
using System;
using System.IO;
using System.Text;

namespace StickyNotes.Net
{
    partial class PacketPackage
    {
        private int PacketID { get; set; }
        public void ReadPacket()
        {
            Stream = new PacketStream(BufferRead);
            GC.Collect();
        }

        public void ReadPacket(PacketStream stream)
        {
            Stream = stream;
            GC.Collect();
        }

        public int GetPacketID()
        {
            return PacketID;
        }

        public PacketStream DecodeStream()
        {
            PacketID = Stream.ReadInt32();
            int length = Stream.ReadInt32();
            byte[] Content = Gzip.Decompress(Stream.ReadBytes(length));

            int passlength = Stream.ReadInt32();
            byte[] PassBuffer = Stream.ReadBytes(passlength);

            Helper.encryptKey = Encoding.UTF8.GetString(PassBuffer);
            byte[] Buffer = Helper.DecryptDll(Content);

            return new PacketStream(Buffer);
        }
    }
}
