using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HTBInject.Utils
{
    class SettingManger
    {
        private static readonly string basepath = AssetsManger.GetFolderPath() + "\\Assets\\Configs\\";
        private static SettingUI sui;

        public static void Init()
        {
            if (!Directory.Exists(basepath)) Directory.CreateDirectory(basepath);
            if (!File.Exists(basepath + "config.json")) File.WriteAllText(basepath + "config.json", (string)AssetsManger.GetAssets(AssetsManger.Type.type_string, "DefaultSetting"));
            sui = new SettingUI(JObject.Parse(File.ReadAllText(basepath + "config.json")),JArray.Parse((string)AssetsManger.GetAssets(AssetsManger.Type.type_string, "SettingModel")));

        }

        public static void ApplyUI(ListView lv) => sui.AppleDisplay(lv);
    }

    class SettingUI
    {
        private List<SystemSetting> stm = new List<SystemSetting>();
        private Dictionary<string,string> stu = new Dictionary<string, string>();

        class SystemSetting
        {
            public TextBlock Tb { get; set; }
            public object Control { get; set; }

            public SystemSetting(TextBlock Tb, object Ct)
            {
                this.Tb = Tb; Control = Ct;
                GC.Collect();
            }
        }

        private readonly JArray bindings;
        private readonly JObject settings;

        public SettingUI(JObject setting,JArray binding) 
        {
            settings = setting;bindings = binding;
            Build();
            GC.Collect();
        }

        private void Build()
        {
            foreach(JObject obj in bindings)
            {
                TextBlock tb = new TextBlock() { Text = (string)obj["text"],Margin = new Thickness(3,3,3,3), HorizontalAlignment = HorizontalAlignment.Center,VerticalAlignment = VerticalAlignment.Center,FontSize = 16};
                if (obj["type"].ToString().Equals("switcher"))
                {
                    stu.Add((string)obj["id"], (string)settings[(string)obj["id"]]);
                    CheckBox cb = new CheckBox() { Name = (string)obj["id"],IsChecked = bool.Parse(stu[(string)obj["id"]]) };
                    cb.Click += new RoutedEventHandler((sender, e) => 
                    {
                        string name = ((CheckBox)sender).Name;
                        if ((bool)((CheckBox)sender).IsChecked) settings[name] = "true";
                        else stu[name] = "false";
                        GC.Collect();
                    });
                    stm.Add(new SystemSetting(tb,cb));
                    GC.Collect();
                }
                else
                {
                    stu.Add((string)obj["id"], (string)settings[(string)obj["id"]]);
                    TextBox cb = new TextBox() { Name = (string)obj["id"],Text = stu[(string)obj["id"]] };
                    cb.TextChanged += new TextChangedEventHandler((sender, e) => stu[((TextBox)sender).Name] = ((TextBox)sender).Text);
                    stm.Add(new SystemSetting(tb, cb));
                    GC.Collect();
                }
            }
            GC.Collect();
        }

        public void AppleDisplay(ListView lv) 
            => lv.Dispatcher.Invoke(() => {
                lv.ItemsSource = stm;
            });

        ~SettingUI() => GC.Collect();
    }
}
