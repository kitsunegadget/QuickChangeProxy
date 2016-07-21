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
using System.Windows.Shapes;
using static QuickChangeProxy.SaveWrite;

namespace QuickChangeProxy
{
    /// <summary>
    /// OptionWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class OptionWindow : Window
    {
        LanValue[] lanvalue;

        public OptionWindow(LanValue[] lanvalue)
        {
            InitializeComponent();
            this.lanvalue = lanvalue;
            ReadState(lanvalue);
        }

        private void ReadState(LanValue[] lan)
        {
            int i = 0;
            foreach(LanValue value in lan)
            {
                TextBox server = (TextBox)FindName("server" + i);
                TextBox port = (TextBox)FindName("port" + i);
                CheckBox over = (CheckBox)FindName("over" + i);

                server.Text = value.server;
                port.Text = value.port;
                over.IsChecked = value.over;
                ++i;
            }
        }

        private void WriteState(LanValue[] lan)
        {
            int i = 0;
            foreach (LanValue value in lan)
            {
                TextBox server = (TextBox)FindName("server" + i);
                TextBox port = (TextBox)FindName("port" + i);
                CheckBox over = (CheckBox)FindName("over" + i);

                value.server = server.Text;
                value.port = port.Text;
                value.over = (bool)over.IsChecked;
                ++i;
            }

            WriteXML(lan);
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            WriteState(lanvalue);
            DialogResult = true;

            Close();
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
