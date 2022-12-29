using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.ViewModels
{
    sealed class SettingPageViewModel : ViewModelBase
    {
        public string Title => "设置页面";
        public List<SettingItem> Settings { get; set; } = new List<SettingItem>();

        public void InitConfigView()
        {
            var cfg = App.AppConfig;
            foreach (var obj in cfg.GetAllObjects())
            {
                Settings.Add(new SettingItem()
                {
                    IsToggle = bool.Parse(obj.Value as string),
                    Title = obj.Nood
                });
            }
            GC.Collect();
        }

        ~SettingPageViewModel()
        {
            Settings.Clear();
            GC.SuppressFinalize(Settings);
            GC.Collect();
        }
    }

    public class SettingItem
    {
        public string Title { get; set; }
        public bool IsToggle { get; set; }
    }
}
