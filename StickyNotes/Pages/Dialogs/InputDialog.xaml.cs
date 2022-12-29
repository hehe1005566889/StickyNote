using StickyNotes.Utils;
using System;

namespace StickyNotes.Common
{
    public partial class InputDialog
    {
        private readonly IInputBoxController Controller;
        public InputDialog(
            IInputBoxController box,
            string title,
            string tips
        ) {
            Controller = box;
            InitializeComponent();

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            Title = title;
            message.Text = tips;
            click.Click += Click_Click;

            GC.Collect();
            NativeImport.CollectRam();
        }

        private void Click_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Controller.OnDone(input.Text);
            Close();
            GC.Collect();
        }
    }
}
