using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuickChangeProxy
{
    public partial class NotifyIconWrapper: Component
    {
        private MainWindow wnd;

        public NotifyIconWrapper(MainWindow wnd)
        {
            InitializeComponent();

            this.toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            this.toolStripMenuItem2.Click += toolStripMenuItem1_Click;
            this.toolStripMenuItem3.Click += toolStripMenuItem1_Click;
            this.toolStripMenuItem4.Click += toolStripMenuItem1_Click;
            this.toolStripMenuItem_Clear.Click += toolStripMenuItem_Clear_Click;
            this.toolStripMenuItem_Open.Click += toolStripMenuItem_Open_Click;
            this.toolStripMenuItem_Exit.Click += toolStripMenuItem_Exit_Click;
            this.wnd = wnd;
            Notify_Reload();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var alt = sender as System.Windows.Forms.ToolStripMenuItem;
            Console.WriteLine(alt.Name.Substring(17));
            wnd.Notify_Set_Click(int.Parse(alt.Name.Substring(17))-1);
            Notify_Reload();
        }

        private void toolStripMenuItem_Clear_Click(object sender, EventArgs e)
        {
            wnd.Notify_Clear_Click();
            Notify_Reload();
        }

        private void toolStripMenuItem_Open_Click(object sender, EventArgs e)
        {
            wnd.Show();
            wnd.WindowState = WindowState.Normal;
        }
        private void toolStripMenuItem_Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Notify_Reload()
        {
            notifyIcon1.Text = wnd.labelEnable.Content.ToString();
            if (wnd.labelServer.Content != null)
            {
                notifyIcon1.Text += "\n" + wnd.labelServer.Content.ToString();
            }
            if (wnd.labelPort.Content != null)
            {
                notifyIcon1.Text += "\n" + wnd.labelPort.Content.ToString();
            }
            if (wnd.labelOverride.Content != null)
            {
                //文字数制限がある模様。ここは表示しなくてもいいかも。
                //NotifyIcon1.Text += "\n" + wnd.labelOverride.Content.ToString();
            }
        }

        public NotifyIconWrapper(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            // ！！この関数は必要！！
        }
    }
}
