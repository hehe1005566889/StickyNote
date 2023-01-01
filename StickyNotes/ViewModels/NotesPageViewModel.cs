using StickyNotes.Utils.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.ViewModels
{
    public sealed class NotesPageViewModel : ViewModelBase
    {
        public NotesPageViewModel()
            => App.Instance.Trans.ApplyTranslation(typeof(NotesPageViewModel), this);

        [Translable]
        public string TitleStr;
        public string Title => TitleStr;
        public bool IsOpen { get; set; } = false;
        public List<string> FilesList { get; private set; } = new List<string>();

        public void OpenMenu()
        {
            IsOpen = true;
            OnPropertyChanged(nameof(IsOpen));
            GC.Collect();
        }

        public void UpdateTickets(List<string> items)
        {
            FilesList.Clear();
            FilesList = items;
            OnPropertyChanged(nameof(FilesList));
            GC.Collect();
        }

        ~NotesPageViewModel()
        {
            FilesList.Clear();
            GC.SuppressFinalize(FilesList);
            GC.Collect();
        }
    }
}
