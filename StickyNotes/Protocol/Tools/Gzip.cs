using System.IO;
using System.IO.Compression;

namespace StickyNotes.Protocol.Tools
{
    class Gzip
    {

        public static byte[] CompressBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    compressedzipStream.Write(bytes, 0, bytes.Length);
                }
                return ms.ToArray();
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using (MemoryStream originalStream = new MemoryStream(bytes))
            {
                using (MemoryStream decompressedStream = new MemoryStream())
                {
                    using (GZipStream decompressionStream = new GZipStream(originalStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedStream);
                    }
                    return decompressedStream.ToArray();
                }
            }
        }
    }
}
