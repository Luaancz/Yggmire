using Luaan.Yggmire.OrleansInterfaces;
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

namespace Luaan.Yggmire.SharpClient.Pages
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private readonly YggmireGame game;

        public YggmireGame Game { get { return this.game; } }

        public GamePage(ISessionGrain session)
        {
            InitializeComponent();
            
            game = new YggmireGame(session);
            game.Run(surface);
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
    }
}
