using HTBInject.Utils;
using StickyNotes.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StickyNotes.Utils.Common
{
    class ExceptionReport
    {
        private static readonly string filePth = $"{AssetsManger.GetFolderPath()}\\exceptions";
        public static void BuildReport(Exception e)
        {
            if (!Directory.Exists(filePth))
                _ = Directory.CreateDirectory(filePth);
            using (PacketStream stream = new PacketStream())
            {
                stream.WriteString(e.Message);
                stream.WriteString(e.Source);
                stream.WriteString(e.StackTrace);
                stream.WriteInt32(e.HResult);

                using (PacketPackage package = new PacketPackage(stream))
                    File.WriteAllBytes($"{filePth}//ExceptionReport_{DateTime.Now.ToFileTime()}.report", package.EncodePacket(114514));
            }
        }

        public static bool DisplayExceptionMsg(Exception e)
        {
            StringBuilder report = new StringBuilder();
            report.Append("Porgram Error!\n")
                  .Append("Please Report The Exception Report File To FlyBirdStudio\n")
                  .Append($"File Path {filePth}\n\n")
                  .Append("Exception Info\n")
                  .Append($"Message {e.Message}\n")
                  .Append($"Stack {e.StackTrace}\n")
                  .Append($"Time {DateTime.Now.ToFileTime()}\n\n")
                  .Append("Click Yes To ReOpen Or Continue Run The Porgram!\n\n");
            var res = MessageBox.Show(report.ToString(), "Exception", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (res == MessageBoxResult.No)
                Environment.Exit(-1);
            else
                return true;
            return false;
        }
    }
}
