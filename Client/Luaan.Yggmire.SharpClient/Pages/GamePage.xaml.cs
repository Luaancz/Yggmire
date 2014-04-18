using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;
using Luaan.Yggmire.SharpClient.Controls;
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
using System.Windows.Threading;

namespace Luaan.Yggmire.SharpClient.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private readonly YggmireGame game;
        private SessionObserver sessionObserver;

        public YggmireGame Game { get { return this.game; } }

        public GamePage(ISessionGrain session)
        {
            InitializeComponent();
            
            game = new YggmireGame(session);
            game.Run(surface);

            this.Loaded += GamePage_Loaded;
        }

        async void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            var observer = await SessionObserverFactory.CreateObjectReference(sessionObserver = new SessionObserver(this));

            await game.Session.RegisterObserver(observer);
        }

        class SessionObserver : ISessionObserver
        {
            GamePage page;

            public SessionObserver(GamePage page)
            {
                this.page = page;
            }

            void ISessionObserver.ShowDialog(string message)
            {
                throw new NotImplementedException();
            }

            void ISessionObserver.ShowInputDialog(int responseId, string message)
            {
                page.Dispatcher.Invoke
                (
                    () =>
                    {
                        var dialog = new InputDialog(responseId, message);

                        page.canvas.Children.Add(dialog);
                    }
                );
            }

            void ISessionObserver.Disconnected()
            {
                page.Dispatcher.Invoke(page.OnDisconnected);
            }

            void ISessionObserver.ReadyForChat()
            {
                page.Dispatcher.Invoke
                    (
                        async () =>
                        {
                            await page.chat.Init(page.Game.Session);
                        }
                    );
            }

            void ISessionObserver.SystemMessage(string message)
            {
                page.Dispatcher.Invoke
                    (
                        () =>
                        {
                            page.chat.AppendLine("System> " + message);
                        }
                    );
            }
        }

        void OnDisconnected()
        {
            game.Exit();

            var window = MainWindow.GetWindow(this) as MainWindow;
            window.Session = null;
            window.frame.Navigate(new LoginPage("You have been disconnected."));
        }

        public void SetFocus()
        {
            var scope = FocusManager.GetFocusScope(chat.tbxChat);
            FocusManager.SetFocusedElement(scope, null);
            Keyboard.ClearFocus();
            Keyboard.Focus(surface);
        }
        
        //void Network_ChatReceived(STC.ChatMessage message)
        //{
        //    if (!Dispatcher.CheckAccess())
        //    {
        //        Dispatcher.Invoke(chatReceived, message);

        //        return;
        //    }

        //    chat.AppendLine(message.Speaker + "> " + message.Message);
        //}


        private void surface_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                chat.tbxChat.Focus();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;
            Game.Session.Disconnect();
        }

    }
}
