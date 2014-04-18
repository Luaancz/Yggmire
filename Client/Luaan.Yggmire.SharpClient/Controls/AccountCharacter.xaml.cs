using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;
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
    /// Interaction logic for AccountCharacter.xaml
    /// </summary>
    public partial class AccountCharacter : UserControl
    {
        ISessionGrain session;

        CharacterInformation character;

        public AccountCharacter(ISessionGrain session, CharacterInformation character)
        {
            this.session = session;
            this.character = character;

            InitializeComponent();

            this.txtCharacterName.Text = character.Name;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            IsEnabled = false;

            try
            {
                var tmp = await this.session.SelectCharacter(character.Name);
            }
            finally
            {
                Mouse.OverrideCursor = null;
                IsEnabled = true;
            }

            var page = new GamePage(session);
            (MainWindow.GetWindow(this) as MainWindow).frame.NavigationService.Navigate(page);
        }
    }
}
