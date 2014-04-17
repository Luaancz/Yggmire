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
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : UserControl
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

        int responseId;

        public InputDialog(int responseId, string message)
        {
            this.responseId = responseId;

            InitializeComponent();

            txtMessage.Text = message;
        }

        private async void tbxResponse_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (!string.IsNullOrEmpty(tbxResponse.Text))
                {
                    try
                    {
                        await Main.Game.Session.Respond(responseId, tbxResponse.Text);

                        Main.canvas.Children.Remove(this);
                    }
                    catch (AggregateException ae)
                    {
                        txtError.Text = string.Empty;

                        ae = ae.Flatten();
                        ae.Handle((ex) => { txtError.Text += "Error: " + ex.Message + "\r\n"; return true; });

                        return;
                    }

                    tbxResponse.Text = string.Empty;
                }

                Main.SetFocus();

                e.Handled = true;
            }
        }
    }
}
