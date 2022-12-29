using ProjectFChat.Network.Utils;
using StickyNotes.Protocol.Tools;
using System;

namespace StickyNotes.Net
{
    partial class PacketPackage : IDisposable
    {
        private PacketStream Stream { get; set; }
        private PacketStream DataStream { get; } = new PacketStream();
        private byte[] BufferRead { get; set; }

        public PacketPackage(PacketStream stream)
        {
            Stream = stream;
            GC.Collect();
        }

        public PacketPackage(byte[] buffer)
        {
            BufferRead = buffer;//Gzip.Decompress(buffer);
            GC.Collect();
        }

        public PacketPackage()
        {
            //BufferRead = buffer;//Gzip.Decompress(buffer);
            GC.Collect();
        }

        public void Dispose()
        {
            Stream.Dispose();
            DataStream.Dispose();
            if (!(BufferRead is null))
                GC.SuppressFinalize(BufferRead);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~PacketPackage()
        {
            Dispose();
        }
    }
}
