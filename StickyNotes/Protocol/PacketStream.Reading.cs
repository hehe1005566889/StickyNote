using FlyBird.Platform.BML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Net
{
    partial class PacketStream
    {
        public byte ReadByte()
        {
            byte[] buffer = new byte[1];
            BasicStream.Read(buffer, 0, buffer.Length);
            return buffer[0];
        }

        public byte[] ReadBytes(int length)
        {
            byte[] buffer = new byte[length];
            BasicStream.Read(buffer, 0, buffer.Length);
            return buffer.ToArray();
        }

        public bool ReadBoolean()
        {
            byte[] buffer = new byte[1];
            BasicStream.Read(buffer, 0, buffer.Length);
            return buffer[0] == 1;
        }

        public Int32 ReadInt32()
        {
            byte[] buffer = new byte[5];
            BasicStream.Read(buffer, 0, buffer.Length);
            return SignInt.Bit5ToInt32(buffer.ToArray());
        }

        public Int64 ReadInt64()
        {
            byte[] buffer = new byte[10];
            BasicStream.Read(buffer, 0, buffer.Length);
            return SignInt.Bit10ToInt64(buffer.ToArray());
        }

        public string ReadString()
        {
            var length = ReadInt32(); 
            byte[] buffer = new byte[length];
            BasicStream.Read(buffer, 0, buffer.Length);

            var value = Encoding.UTF8.GetString(buffer);

            return value;
        }
    }
}
