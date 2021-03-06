﻿using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;
using Luaan.Yggmire.SharpClient.Controls;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
    /// Interaction logic for AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        ISessionGrain session;
        AccountInformation account;

        public AccountPage(ISessionGrain session, AccountInformation account)
        {
            this.session = session;
            this.account = account;

            InitializeComponent();

            foreach (var c in account.Characters)
            {
                var ac = new AccountCharacter(session, c);
                pnlCharacters.Children.Add(ac);
            }
        }

        private async void btnCreateCharacter_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            IsEnabled = false;

            CharacterInformation character;

            try
            {
                character = await this.session.CreateCharacter();
            }
            finally
            {
                Mouse.OverrideCursor = null;
                IsEnabled = true;
            }

            var page = new GamePage(session);
            NavigationService.Navigate(page);
        }
    }
}
