using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.ViewModels
{
    public sealed class AcccountPageViewModel : ViewModelBase
    {
        public bool IsOpen { get; set; }

        public void OpenMenu()
        {
            IsOpen = true;
            OnPropertyChanged(nameof(IsOpen));
            GC.Collect();
        }
    }
}
