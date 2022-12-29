namespace StickyNotes.ViewModels
{
    sealed class LoginDialogViewModel : ViewModelBase
    {
        private bool Client => App.Net is null;

        public string Title => Client ? "登陆到服务器" : "更改密码";
        public string Hint0 => Client ? "连接码" : "用户名";
        public string Hint1 => Client ? "用户名" : "旧密码";
        public string Hint2 => Client ? "密码　" : "新密码";

        public string ActionBarText { get; private set; }

        public void SetText(string txt)
        {
            ActionBarText = txt;
            OnPropertyChanged(nameof(ActionBarText));
        }

        public string ButtonText => Client ? "登陆" : "确定";
    }
}
