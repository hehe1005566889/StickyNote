using FStore.NetWork;
using ProjectFChat.Network.Utils;
using StickyNotes.Protocol.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Net
{
    partial class PacketPackage
    {
        public byte[] EncodePacket(int PacketID)
        {
            int[] abc = password.RandomX(3);
            int[] xp = password.RandomX(4, 120);
            byte[] PassBuffer = password.GetCode(abc[0], abc[1], abc[2], xp);

            Helper.encryptKey = Encoding.UTF8.GetString(PassBuffer);
            byte[] Buffer = Stream.GetBytes();

            DataStream.WriteInt32(PacketID);

            byte[] EncodeBuffer = Gzip.CompressBytes(Helper.EncryptDll(Buffer));
            DataStream.WriteInt32(EncodeBuffer.Length);
            DataStream.WriteBytes(EncodeBuffer);

            DataStream.WriteInt32(PassBuffer.Length);
            DataStream.WriteBytes(PassBuffer);

            //return Gzip.CompressBytes(DataStream.GetBytes());
            return DataStream.GetBytes();
        }
    }
}
