using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NetworkingLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EchoDemonstrationClientApp 
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IDisplayMessage 
    {
        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.TextBoxPortNumber.Text = NetworkingLibaryCore.DefaultServiceName;
            this.TextBoxIPAddress.Text = "10.160.";//NetworkingLibaryCore.LocalHostName;
            this.MessageToSendTexBox.Text = "Hello World";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetUpConnection();
        }

        public async void SetUpConnection()
        {
            Connection connection = NetworkingLibaryCore.GetConnection(this.TextBoxPortNumber.Text, this.TextBoxIPAddress.Text, this);
            await connection.StartAsync();
            connection.Send(MessageToSendTexBox.Text);
        }

        public async void DisplayMessage(string message)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // removes all of the null specifiers from the string
                // This is an issue when converting strings to bytes and back to strings
                //message = message.Replace("\0", "");

                // Upadate the Message Text box
                MessageRecived.Text = message;
            });
        }
    }
}
