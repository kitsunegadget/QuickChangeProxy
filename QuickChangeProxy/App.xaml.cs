using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QuickChangeProxy
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private NotifyIconWrapper notifyIcon;
        private MainWindow startWindow;

        // 開始処理
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //ShutdownMode = ShutdownMode.OnExplicitShutdown;

            startWindow = new MainWindow();
            startWindow.Show();

            //ウィンドウイベント作成
            startWindow.StateChanged += new EventHandler(Window_StateChanged); 
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (startWindow.WindowState == WindowState.Minimized)
            {
                notifyIcon = new NotifyIconWrapper(startWindow);
                startWindow.Hide();
            }
            else if (startWindow.WindowState == WindowState.Normal)
            {
                notifyIcon.Dispose();
            }
        }

        // 終了処理
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (notifyIcon != null)
            {
                notifyIcon.Dispose();
            }
        }
    }
}
