using HandyControl.Controls;
using StickyNotes.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace StickyNotes.ViewModels
{
    sealed class SettingPageViewModel : ViewModelBase
    {
        public SettingPageViewModel() 
            => App.Instance.Trans.ApplyTranslation(typeof(SettingPageViewModel), this);

        [Translable]
        public string TitleStr;
        public string Title => TitleStr;

        public List<SettingItem> Settings { get; set; } = new List<SettingItem>();

        public void InitConfigView()
        {
            var cfg = App.Instance.Config.GetCollect();
            foreach (var obj in cfg.GetAllObjects())
            {
                Settings.Add(new SettingItem()
                {
                    IsChecked = bool.Parse(obj.Value as string),
                    Title = obj.Nood,
                    TransTitle = App.Instance.Trans.GetValue(obj.Nood)
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
        public bool IsChecked { get; set; }
        public string TransTitle { get; set; }
    }
}
