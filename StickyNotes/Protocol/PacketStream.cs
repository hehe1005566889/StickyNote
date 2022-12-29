using System;
using System.IO;

namespace StickyNotes.Net
{
    public partial class PacketStream : IDisposable
    {
        private Stream BasicStream { get; }

        public PacketStream()
        {
            BasicStream = new MemoryStream();
            GC.Collect();
        }

        public PacketStream(byte[] bytes)
        {
            BasicStream = new MemoryStream(bytes);
            GC.Collect();
        }

        public PacketStream(Stream stream)
        {
            BasicStream = stream;
            GC.Collect();
        }

        public byte[] GetBytes()
        {
            BasicStream.Position = 0;
            byte[] buffer = new byte[BasicStream.Length];
            for (int totalBytesCopied = 0; totalBytesCopied < BasicStream.Length;)
            {
                totalBytesCopied += BasicStream.Read(buffer, totalBytesCopied, Convert.ToInt32(BasicStream.Length) - totalBytesCopied);
            }
            return buffer;
        }

        public void Dispose()
        {
            BasicStream.Close();
            BasicStream.Dispose();
            GC.SuppressFinalize(BasicStream);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        ~PacketStream()
        {
            Dispose();
        }
    }
}
