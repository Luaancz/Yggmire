using Luaan.Yggmire.SharpClient.Pages;
using System.Windows.Input;


namespace Luaan.Yggmire.SharpClient
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            var loginPage = new LoginPage();
            frame.Navigate(loginPage);
        }
    }
}
