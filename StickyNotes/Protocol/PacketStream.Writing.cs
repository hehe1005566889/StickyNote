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
        public void WriteByte(byte array)
        {
            byte[] buffer = new byte[1];
            buffer[0] = array;
            BasicStream.Write(buffer, 0, buffer.Length);
        }

        public void WriteBoolean(bool boolean)
        {
            byte[] ToWrite;
            if (boolean)
            {
                ToWrite = new byte[] { 1 };
                WriteBytes(ToWrite);
            }
            else
            {
                ToWrite = new byte[] { 0 };
                WriteBytes(ToWrite);
            }
        }

        public void WriteInt32(Int32 value)
        {
            byte[] encode = SignInt.Int32ToBit5(value);
            BasicStream.Write(encode, 0, encode.Length);
        }

        public void WriteInt64(Int64 value)
        {
            byte[] encode = SignInt.Int64ToBit10(value);
            BasicStream.Write(encode, 0, encode.Length);
        }

        public void WriteBytes(byte[] array)
        {
            BasicStream.Write(array, 0, array.Length);
        }

        public void WriteString(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            WriteInt32(bytes.Length);
            BasicStream.Write(bytes, 0, bytes.Length);
        }
    }
}
