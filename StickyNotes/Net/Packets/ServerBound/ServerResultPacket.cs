using HandyControl.Controls;
using StickyNotes.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Net.Packets.ServerBound
{
    class ServerResultPacket : IServerBoundPacket
    {
        public ServerResultPacket() => GC.Collect();
        public int GetPacketID() => 1;

        public bool ReadPacket(PacketStream DataStream)
        {
            var status = DataStream.ReadInt32();
            var result = DataStream.ReadBoolean();

            switch (status)
            {
                case 5:
                    if (result)
                        Growl.SuccessGlobal("更改密码成功~");
                    else
                        Growl.SuccessGlobal("更改密码失败:(");
                    break;
                case 6:
                    if (result)
                        Growl.SuccessGlobal("发布便签成功~");
                    else
                        Growl.ErrorGlobal("发布便签失败:(");
                    break;
            }

            return true;
        }
    }
}
