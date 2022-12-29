using HandyControl.Controls;
using HandyControl.Tools.Command;
using StickyNotes.Utils.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StickyNotes.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Control content;
        public Control Content
        {
            set
            {
                content = Content;
                OnPropertyChanged(nameof(Content));
            }
            get => content;
        }

        public string Icon0 => "\ue80f";
        public string Tips0 => "同步";

        public string Icon1 => "\ue71d";
        public string Tips1 => "便签管理";
        
        public string Icon2 => "\ue713";
        public string Tips2 => "程序设置";
    }

    public class Item
    {
        public string Icon { get; set; }
        public string ToolTips { get; set; }
        public MainWindowViewModel Model { set; private get; }

        private RelayCommand menuItemSelect;

        public ICommand MenuItemSelect
        {
            get
            {
                if (menuItemSelect == null)
                {
                    menuItemSelect = new RelayCommand(PerformMenuItemSelect);
                }

                return menuItemSelect;
            }
        }

        private void PerformMenuItemSelect(object commandParameter)
        {
            Console.WriteLine("qwq");
        }
    }
}
