using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Chat;
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

            //txtChatLog.ContentEnd.InsertLineBreak();
            txtChatLog.ContentEnd.InsertLineBreak();
        }
        public void AppendLine(string text)
        {
            txtChatLog.ContentEnd.InsertTextInRun(text);
            txtChatLog.ContentEnd.InsertLineBreak();
            scroll.ScrollToBottom();
        }

        public class ChatObserver : IChatObserver
        {
            ChatWindow chat;

            public ChatObserver(ChatWindow chat)
            {
                this.chat = chat;
            }

            public void UpdateChat(string name, string message)
            {
                chat.Dispatcher.Invoke(() => { chat.AppendLine(name + "> " + message); });
            }
        }

        ChatObserver observer;
        IChatObserver observerReference;

        public async Task Init(ISessionGrain session)
        {
            observer = new ChatObserver(this);
            observerReference = await ChatObserverFactory.CreateObjectReference(observer);

            try
            {
                await session.SubscribeForChat(0, observerReference);
            }
            catch (AggregateException ae)
            {
                ae = ae.Flatten();

                ae.Handle((ex) => { AppendLine("Error: " + ex.Message); return true; });

                return;
            }

            AppendLine("Welcome!");
        }

        private async void tbxChat_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (!string.IsNullOrEmpty(tbxChat.Text))
                {
                    await Main.Game.Session.SendChatMessage(0, tbxChat.Text);
                    tbxChat.Text = string.Empty;
                }

                Main.SetFocus();

                e.Handled = true;
            }
        }
    }
}
