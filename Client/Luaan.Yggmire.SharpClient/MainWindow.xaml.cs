using Luaan.Yggmire.OrleansInterfaces;
using Luaan.Yggmire.SharpClient.Pages;
using System.Windows.Input;


namespace Luaan.Yggmire.SharpClient
{
    public partial class MainWindow
    {
        public ISessionGrain Session { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Closing += MainWindow_Closing;

            var loginPage = new LoginPage();
            frame.Navigate(loginPage);
        }

        async void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Session != null)
                await Session.Disconnect();
        }
    }
}
