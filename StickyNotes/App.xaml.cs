using HTBInject.Utils;
using Name3.Models;
using StickyNotes.Common;
using StickyNotes.Net;
using StickyNotes.Utils;
using StickyNotes.Utils.Common;
using StickyNotes.Views;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace StickyNotes
{
    public partial class App : Application
    {
        public static ConfigCollect AppConfig { get; set; }
        public static Client Net { get; set; }
        public static App Instance;
        private MainWindow Window { get; set; }

        public App()
        {
            Instance = this;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            using (ConfigReader reader = new ConfigReader())
            {
                var result = reader.ReadDocument($"{AssetsManger.GetFolderPath()}\\StickyNote");
                if (result == null)
                {
                    AppConfig = new ConfigCollect();
                    InitDefaultConfig(AppConfig);
                }
                GC.Collect();
            }
            NotificationIcon.Instance.LoadIcon();

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(15) };
            timer.Tick += (s, e) => NativeImport.CollectRam();
            timer.Start();

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ExceptionReport.BuildReport(e.Exception);
            ExceptionReport.DisplayExceptionMsg(e.Exception);
            NativeImport.CollectRam();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ExceptionReport.BuildReport(e.ExceptionObject as Exception);
            ExceptionReport.DisplayExceptionMsg(e.ExceptionObject as Exception);
            NativeImport.CollectRam();
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionReport.BuildReport(e.Exception);
            e.Handled = ExceptionReport.DisplayExceptionMsg(e.Exception);
            NativeImport.CollectRam();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            NotesWindows.GetController().LoadAllWindows();

            NotificationIcon.Instance.RegisterMenuItem(new MenuInfo() { Icon = "\ue78b", Name = "显示窗体" }, new Action(() =>
            {
                Dispatcher.Invoke(() => {
                    if (Window is null) Window = new MainWindow();
                    Window.Show();
                });
            }));
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e) => ExitApp();

        public void ExitApp()
        {
            NotesWindows.GetController().SaveAllWindowNames();

            NotificationIcon.Instance.KillIcon();
            NativeImport.CollectRam();
            Environment.Exit(0);
        }
        
        private void InitDefaultConfig(ConfigCollect config)
        {
            config.AddObject(new ConfigObject()
            {
                Nood = "EnableEffect", Type = "Bool", Value = "True"
            });
            config.AddObject(new ConfigObject()
            {
                Nood = "EnableNetWork", Type = "Bool", Value = "True"
            });
            config.AddObject(new ConfigObject()
            {
                Nood = "EnableAnimation", Type = "Bool", Value = "True"
            });
            GC.Collect();
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resourceName = "StickyNotes.Library." + new AssemblyName(args.Name).Name + ".dll";

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream is null)
                    return null;

                byte[] assemblyData = new byte[stream.Length];
                _ = stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        ~App() => ExitApp();
    }
}
