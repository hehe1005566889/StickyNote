using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HTBInject.Utils
{
    public class AssetsManger
    {
        private static string configpath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\SticyNotes";

        public static string DownloadPath = configpath + @"\donwload";

        public AssetsManger()
        {
            if (!Directory.Exists(DownloadPath)) Directory.CreateDirectory(DownloadPath);
            if (!Directory.Exists(configpath)) Directory.CreateDirectory(configpath);
            try { Check(); } catch { MessageBox.Show("Error in init!Please contact FlyBirdStudio!"); throw new DirectoryNotFoundException(); }
        }

        private static ResourceManager rm;

        private void Check()
        {
            JArray array = (JArray)JObject.Parse(rm.GetString("FileFolder"))["folder"];
            foreach (JValue folder in array) if (!Directory.Exists(configpath + "\\" + (string)folder)) Directory.CreateDirectory(configpath + "\\" + (string)folder);
        }

        public static string GetFolderPath() { return configpath; }

        public enum Type
        {
            type_string,
            type_bitarry,
            type_image
        }

        public static object GetAssets(Type type,string key)
        {
            if(rm is null)
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                rm = new ResourceManager("StickyNotes.Properties.Resources", asm);
            }
            switch (type)
            {
                case Type.type_string:
                    return rm.GetString(key);
                case Type.type_bitarry:
                    var st = rm.GetStream(key);
                    byte[] buffer = new byte[st.Length];
                    st.Read(buffer, 0, buffer.Length);
                    return buffer;
                case Type.type_image:
                    return ToImageSource((Icon)rm.GetObject(key));
                default:
                    return rm.GetString(key);
            }
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
