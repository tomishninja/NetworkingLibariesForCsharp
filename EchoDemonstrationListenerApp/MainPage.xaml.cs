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

namespace EchoDemonstrationListenerApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, IDisplayMessage
    {
        public MainPage()
        {
            this.InitializeComponent();

            ListenerSocket listener = NetworkingLibaryCore.GetListener(this);
            listener.Start();
        }

        public async void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.StatusListBox.Items.Insert(0, new ListBoxItem().Content = message);

                if (StatusListBox.Items.Count > 1000)
                {
                    for(int i = 0; i < 100; i++)
                    {
                        StatusListBox.Items.RemoveAt(900);
                    }
                }
            });
        }
    }
}
