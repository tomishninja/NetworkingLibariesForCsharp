using System;
using NetworkingLibrary;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace EchoDemonstrationListener
{
    class Program : IDisplayMessage
    {
        private static readonly Object obj = new Object();

        static void Main(string[] args)
        {
            Console.WriteLine("Running Listener Command Console");
            Program program = new Program();
            program.run();
        }

        public void run()
        {
            ListenerSocket listener = NetworkingLibaryCore.GetListener(this);
            listener.Start();

            Console.ReadLine();
        }

        public void DisplayMessage(string message)
        {
            lock (obj)
            {
                Console.WriteLine(message);
            }
        }
    }
}
