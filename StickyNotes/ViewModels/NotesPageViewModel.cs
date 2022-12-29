using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.ViewModels
{
    public sealed class NotesPageViewModel : ViewModelBase
    {
        public string Title => "便签管理";

        public List<string> FilesList { get; private set; } = new List<string>();

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
