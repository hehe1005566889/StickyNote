using HandyControl.Controls;
using StickyNotes.Net;
using StickyNotes.Net.Packets.ServerBound;
using StickyNotes.ViewModels;
using System;
using System.Windows.Controls;

namespace StickyNotes.Pages.Dialogs
{
    public partial class LoginPage : Border
    {
        private bool isDebugMode = true;

        private readonly AccontPage Page;
        private readonly LoginDialogViewModel Model = new LoginDialogViewModel();
        public LoginPage(AccontPage page)
        {
            DataContext = Model;
            Page = page;
            InitializeComponent();

            if (isDebugMode)
            {
                code.Text = "H4sIAAAAAAAACouO1463jY3GID3NQjEFkUnnENdKv0qTcp8sCL/UN9DWFgD5srn5TgAAAA==";
                pass.Password = "cubevlmu";
                user.Text = "cubevlmu";
            }

            close.Click += (s, e) => Page.CloseDialog();
            GC.Collect();
        }

        private void apply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Model.SetText("开始连接~");
            App.Net = new Client();
            App.Net.ChangeCode(code.Text);//
            if (!App.Net.TryConnect())
            {
                Model.SetText(":( 连接失败~");
                var result = MessageBox.Ask("连接失败!请检查您的连接代码是否正确，或者点击重试来重新连接:>", "连接失败");
                if (result is System.Windows.MessageBoxResult.OK)
                    apply_Click(null, null);
                else
                    return;
            }
            Model.SetText(":) 连接成功~开始登陆");
            App.Net.Handler.RegisterPacket(0, new ServerBoundLoginPacket(this));
            App.Net.Login(user.Text, pass.Password);
        }

        public void OnResult(bool IsLogin)
        {
            if(IsLogin)
            {
                Model.SetText("登陆成功!~");
                Growl.SuccessGlobal("登陆到服务器成功!");
                Page.CloseDialog();
            }else
            {
                Model.SetText("登录失败,请查看账户密码:(");
                Growl.WarningGlobal("登录失败，请再查看一下你的账户密码是否正确吧~");
            }
            App.Net.Handler.Remove(0);
        }
    }
}
