using HTBInject.Utils;
using Name3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Utils.Configration
{
    public sealed class AppSetting
    {
        public bool EnableEffect     = true;
        public bool EnableNetWork    = true;
        public bool EnableAnimation  = true;

        public AppSetting()
        {
            ReadConfig();
            GC.Collect();
        }

        private void ReadConfig()
        {
            using (ConfigReader reader = new ConfigReader())
            {
                var result = reader.ReadDocument($"{AssetsManger.GetFolderPath()}\\StickyNote");
                if (result is null)
                {
                    Collect = new ConfigCollect();
                    InitDefaultConfig(Collect);
                }
                else Collect = result;
                UpdateConfig();
                GC.Collect();
            }
            GC.Collect();
        }

        public void UpdateConfig()
        {
            EnableEffect    = bool.Parse(Collect.GetConfig(nameof(EnableEffect)).Value.Value as string);
            EnableNetWork   = bool.Parse(Collect.GetConfig(nameof(EnableNetWork)).Value.Value as string);
            EnableAnimation = bool.Parse(Collect.GetConfig(nameof(EnableAnimation)).Value.Value as string);
            GC.Collect();
        }

        private void InitDefaultConfig(ConfigCollect config)
        {
            config.AddObject(new ConfigObject()
            {
                Nood = "EnableEffect",
                Type = "Bool",
                Value = "True"
            });
            config.AddObject(new ConfigObject()
            {
                Nood = "EnableNetWork",
                Type = "Bool",
                Value = "True"
            });
            config.AddObject(new ConfigObject()
            {
                Nood = "EnableAnimation",
                Type = "Bool",
                Value = "True"
            });
            GC.Collect();
        }

        public void SaveConfig()
        {
            using (ConfigWriter writer = ConfigWriter.CreateWriter())
                writer.SaveDocument($"{AssetsManger.GetFolderPath()}\\StickyNote", Collect);
            GC.Collect();
        }

        ~AppSetting()
        {
            Collect.Dispose();
            GC.SuppressFinalize(Collect);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public ConfigCollect GetCollect() => Collect;
        private ConfigCollect Collect;
    }
}
