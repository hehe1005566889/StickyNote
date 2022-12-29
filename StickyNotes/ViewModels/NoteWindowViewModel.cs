using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.ViewModels
{
    public sealed class NoteWindowViewModel : ViewModelBase
    {
        public int CaretIndex { get; set; } = 0;
        public string InputText { get; set; } = "";
        public string Title { get; set; } = "";
        
        public double Top { get; set; }
        public double Left { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public void SetLeftTop(int top, int left)
        {
            Top = top;
            OnPropertyChanged(nameof(Top));
            Left = left;
            OnPropertyChanged(nameof(Left));
            GC.Collect();
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            OnPropertyChanged(nameof(Width));
            Height = height;
            OnPropertyChanged(nameof(Height));
            GC.Collect();
        }

        public void SetText(string text)
        {
            InputText = text;
            OnPropertyChanged(nameof(InputText));
            GC.Collect();
        }

        public void SetTitle(string text)
        {
            Title = text;
            OnPropertyChanged(nameof(Title));
            GC.Collect();
        }

        public void ChangeText(string text)
        {
            InputText = text;
            OnPropertyChanged(nameof(InputText));
        }

        ~NoteWindowViewModel()
        {
            GC.SuppressFinalize(InputText);
            GC.SuppressFinalize(Width);
            GC.SuppressFinalize(Height);
            GC.SuppressFinalize(Top);
            GC.SuppressFinalize(Left);
            GC.Collect();
        }
    }
}
