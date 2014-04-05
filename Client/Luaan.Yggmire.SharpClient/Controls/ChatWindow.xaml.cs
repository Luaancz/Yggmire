using Luaan.Yggmire.SharpClient.Pages;
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

namespace Luaan.Yggmire.SharpClient.Controls
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : UserControl
    {
        GamePage parentFrame;

        GamePage Main
        {
            get
            {
                if (parentFrame == null)
                    parentFrame = (MainWindow.GetWindow(this) as MainWindow).frame.Content as GamePage;

                return parentFrame;
            }
        }

        public ChatWindow()
        {
            InitializeComponent();

            txtChatLog.ContentEnd.InsertLineBreak();
            txtChatLog.ContentEnd.InsertLineBreak();
        }
        public void AppendLine(string text)
        {
            txtChatLog.ContentEnd.InsertTextInRun(text);
            txtChatLog.ContentEnd.InsertLineBreak();
            scroll.ScrollToBottom();
        }

        private void tbxChat_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (!string.IsNullOrEmpty(tbxChat.Text))
                {
                    AppendLine(tbxChat.Text);

                    // Main.Game.Network.SendChat(tbxChat.Text);
                    tbxChat.Text = string.Empty;
                }

                Main.SetFocus();

                e.Handled = true;
            }
        }
    }
}
