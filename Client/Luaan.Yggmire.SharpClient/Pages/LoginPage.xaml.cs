using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.OrleansInterfaces.Account;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async Task HandleAccountAction(Func<ISessionGrain, Task<AccountInformation>> action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            IsEnabled = false;

            ISessionGrain session;

            try
            {
                if (!OrleansClient.IsInitialized)
                {
                    OrleansClient.Initialize();
                }

                session = SessionGrainFactory.GetGrain(Guid.NewGuid());

                await action(session);
            }
            catch (AggregateException ae)
            {
                ae = ae.Flatten();

                txtError.Text = "";
                ae.Handle((ex) => { txtError.Text += ex.Message + "\r\n"; return true; });

                return;
            }
            catch (Exception ex)
            {
                txtError.Text = ex.Message;

                return;
            }
            finally
            {
                Mouse.OverrideCursor = null;
                IsEnabled = true;
            }

            var gamePage = new GamePage(session);
            NavigationService.Navigate(gamePage);
        }
        
        private async void btnLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await
                HandleAccountAction
                    (
                        (session) =>
                        {
                            return session.Authorize(tbxUsername.Text, tbxPassword.Password);
                        }
                    );
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            await
                HandleAccountAction
                    (
                        (session) =>
                        {
                            return session.CreateAccount(tbxUsername.Text, tbxPassword.Password);
                        }
                    );
        }
    }
}
