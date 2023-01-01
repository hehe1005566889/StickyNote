using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StickyNotes.Utils.Common
{
    public class ExceptionUI
    {
        public ExceptionUI(Exception e)
        {
            exception = e;
            BuildUI();
            GC.Collect();
        }

        private void BuildUI()
        {
            Window.Controls.Add(new Label()
            {
                Text = "Opesssee~ Program Error",
                Location = new System.Drawing.Point(10,10),
                Size = new System.Drawing.Size(300, 30)
            });
            Window.Controls.Add(new Label()
            {
                Text = "The Application Call An Exception Which Is Not Deal With",
                Location = new System.Drawing.Point(30,45),
                Size = new System.Drawing.Size(300, 30)
            });
            Window.Controls.Add(new Label()
            {
                Text = "Please Report This To FlyBird Studio To FixUp~",
                Location = new System.Drawing.Point(30,80),
                Size = new System.Drawing.Size(300, 30)
            });
            Window.Controls.Add(new Label()
            {
                Text = "Exception Info ->",
                Location = new System.Drawing.Point(30,115),
                Size = new System.Drawing.Size(300, 30)
            });
            var box = new RichTextBox()
            {
                Text = ExceptionReport.BuildExceptionMsg(exception),
                Location = new System.Drawing.Point(30, 150),
                Size = new System.Drawing.Size(Window.Width - 80, Window.Height - 300),
                ReadOnly = true,
                BorderStyle = BorderStyle.None
            };
            Window.Controls.Add(box);
            GC.SuppressFinalize(box);
            var close = new Button()
            {
                Location = new System.Drawing.Point(30, box.Location.Y + box.Height + 10),
                Size = new System.Drawing.Size(320, 50),
                Text = "Close"
            };
            Window.Controls.Add(close);
            close.Click += Close_Click;
            Window.FormClosed += Window_FormClosed;
            GC.SuppressFinalize(close);
            GC.Collect();
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e)
            => DisposeThis();

        private void Close_Click(object sender, EventArgs e)
        {
            Window.Close();
            DisposeThis();
            GC.Collect();
        }

        private void DisposeThis()
        {
            foreach (var c in Window.Controls)
                GC.SuppressFinalize(c);
            Window.Dispose();
            GC.SuppressFinalize(Window);
            GC.SuppressFinalize(exception);
            GC.SuppressFinalize(this);
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        public void Show() => Window.Show();
        ~ExceptionUI() => DisposeThis();

        private readonly Form Window = new Form() {
            Width = 400,
            Height = 700,
            FormBorderStyle = FormBorderStyle.FixedToolWindow,
            Text = "Application Exception",
            BackColor = Color.White
        };
        private readonly Exception exception;
    }
}
