using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NetworkingLibrary;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

/// <summary>
/// 
/// </summary>
namespace EchoDemonstration
{
    /// <summary>
    /// A echo server that will just return messages to this machine using only
    /// C# core. This is a demonstration.
    /// </summary>
    public sealed partial class MainPage : Page, IDisplayMessage
    {
        /// <summary>
        /// this is used as a lock for the messages recived from the 
        /// networking libaries so there are no colisons
        /// </summary>
        private static readonly Object obj = new Object();

        /// <summary>
        /// 
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.LocalHostDemo();
        }

        private void LocalHostDemo()
        {
            // start the listener socket
            ListenerSocket listener = NetworkingLibaryCore.GetListener(this);
            listener.Start();

            SetUpConnection();
        }

        public async void SetUpConnection()
        {
            Connection connection = NetworkingLibaryCore.GetConnection(this);
            await connection.StartAsync();
            connection.Send(MessageToSendTexBox.Text);
        }

        public async void DisplayMessage(string message)
        {
           await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
           {
               MessageRecived.Text = message;
               Console.WriteLine(message);
           });
        }
    }
}
