using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using static QuickChangeProxy.SaveWrite;

namespace QuickChangeProxy
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private RegistryKey key;
        private LanValue[] lanvalue;
        private int settingValue = 0;

        //設定反映させるためのInternerSetOption関数を作る
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        public MainWindow()
        {
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "settings.config"))
            {
                lanvalue = new LanValue[4];
                for(int i = 0; i<lanvalue.Length; ++i)
                {
                    lanvalue[i] = new LanValue();
                }
                lanvalue[0].server = "example.com";
                lanvalue[0].port = "8080";
                lanvalue[0].over = true;
            }
            else
            {
                lanvalue = ReadXML();
            }

            InitializeComponent();
            ReadRegistry();
        }

        //レジストリ読み込み
        private void ReadRegistry()
        {
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\", false);
            if (key.GetValue("ProxyEnable").ToString() == "1")
            {
                labelEnable.Content = "LANにプロキシサーバーを利用する設定です。";
                if (key.GetValue("ProxyServer", "noKey").ToString() != "noKey")
                {
                    String[] server = key.GetValue("ProxyServer").ToString().Split(':');
                    labelServer.Content = "アドレス：" + server[0];
                    if (server.Length > 1)
                    {
                        labelPort.Content = "ポート：" + server[1];
                    }
                }
                else
                {
                    labelServer.Content = null;
                    labelPort.Content = null;
                }
                if (key.GetValue("ProxyOverride", "noKey").ToString() != "noKey")
                {
                    labelOverride.Content = "ローカルアドレスにはプロキシサーバーを利用しない";
                }
            }
            else
            {
                labelEnable.Content = "LANにプロキシサーバーを利用しない設定です。";
                labelServer.Content = null;
                labelPort.Content = null;
                labelOverride.Content = null;
            }
            key.Close();
        }

        //レジストリ書き込み
        
        private void WriteRegistry(int n)
        {
            string colon = ":";

            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\");
            key.SetValue("MigrateProxy", 1);
            key.SetValue("ProxyEnable", 1);

            if (lanvalue[n].port == "")
            {
                colon = "";
            }
            key.SetValue("ProxyServer", lanvalue[n].server + colon + lanvalue[n].port);

            if (lanvalue[n].over)
            {
                key.SetValue("ProxyOverride", "<local>");
            }
            key.Close();

            _SendMessage();
        }
        

        //レジストリ解除
        private void ClearRegistry()
        {
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\");
            key.SetValue("MigrateProxy", 1);
            key.SetValue("ProxyEnable", 0);
            if (key.GetValue("ProxyServer", "noKey").ToString() != "noKey")
            {
                key.DeleteValue("ProxyServer");
            }
            if (key.GetValue("ProxyOverride", "noKey").ToString() != "noKey")
            {
                key.DeleteValue("ProxyOverride");
            }
            key.Close();

            _SendMessage();
        }

        //変更状態反映
        private void _SendMessage()
        {
            bool iReturn = InternetSetOption(IntPtr.Zero, 95, IntPtr.Zero, 0); //INTERNET_OPTION_PROXY_SETTINGS_CHANGED = 95
            bool jReturn = InternetSetOption(IntPtr.Zero, 39, IntPtr.Zero, 0); //INTERNET_OPTION_SETTINGS_CHANGED = 39
            bool kReturn = InternetSetOption(IntPtr.Zero, 37, IntPtr.Zero, 0); //INTERNET_OPTION_REFRESH = 37

        }

        ///////////
        //イベント
        ///////////
        private void ButtonSetClick(object sender, RoutedEventArgs e)
        {
            WriteRegistry(settingValue);
            MessageBox.Show("設定しました！");
            ReadRegistry();
        }

        private void ButtonClearClick(object sender, RoutedEventArgs e)
        {
            ClearRegistry();
            MessageBox.Show("設定解除しました！");
            ReadRegistry();
        }

        private void ButtonReload(object sender, RoutedEventArgs e)
        {
            ReadRegistry();
        }

        private void openSetting(object sender, RoutedEventArgs e)
        {
            var optionWindow = new OptionWindow(lanvalue);
            var result = optionWindow.ShowDialog();
            if (result.HasValue)
            {
                if ((bool)result)
                {
                    radioButtonEvent(settingValue);
                }
            }
        }

        private void radioButton0_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonEvent(0);
            settingValue = 0;
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonEvent(1);
            settingValue = 1;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonEvent(2);
            settingValue = 2;
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonEvent(3);
            settingValue = 3;
        }

        private void radioButtonEvent(int n)
        {
            labelServerEdit.Content = "アドレス： " + lanvalue[n].server;
            labelPortEdit.Content = "ポート： " + lanvalue[n].port;
            if (lanvalue[n].over)
            {
                labelOverrideEdit.Content = "ローカルアドレスにはプロキシサーバーを利用しない";
            }
            else
            {
                labelOverrideEdit.Content = "";
            }
        }

        //Menu処理
        private void AboutClick(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        // タスクトレイからの実行用
        public void Notify_Set_Click(int n)
        {
            WriteRegistry(n);
            //MessageBox.Show("設定しました！");
            ReadRegistry();
        }
        public void Notify_Clear_Click()
        {
            ClearRegistry();
            //MessageBox.Show("設定解除しました！");
            ReadRegistry();
        }
    }
}
