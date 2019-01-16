using System;
using NetworkingLibrary;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace App1
{
    class Program : iDisplayMessage
    {
        private static readonly Object obj = new Object();
        
        static void Main(string[] args)
        {
            Program program = new Program();
            program.RunHelloWorldTest();

        }

        public void RunHelloWorldTest()
        {
            displayMessageStatic("Starting:");

            // start the listener socket
            ListenerSocket listener = Class1.GetListener(this);
            listener.Start();


            //displayMessageStatic("Listener Socket Running");

            //SetUpConnection();

            Console.ReadLine();
            
        }

        public async void SetUpConnection()
        {
            Connection connection = Class1.GetConnection(this);
            await connection.StartAsync();
            displayMessageStatic("Connection Running");

            displayMessageStatic("Sending Messages");
            connection.Send("HelloWorld");
            SendLotsOfData(connection);
        }

        public void SendLotsOfData(Connection connection)
        {
            Random random = new Random();

            int[] sixNumbers = new int[6];
            int count = 0;
            while (true)
            {
                for(int i = 0; i < sixNumbers.Length; i++)
                {
                    sixNumbers[i] = random.Next(255);
                }

                connection.Send(ConvertArrayToString(sixNumbers));
                count++;
            }
        }

        static string ConvertArrayToString(int[] array)
        {
            string output = "";
            for(int i = 0; i < array.Length; i++)
            {
                output += array[i];
                if (i < array.Length-1)
                {
                    output += ",";
                }
            }
            return output;
        }

        static void displayMessageStatic(string message)
        {
            lock (obj)
            {
                Console.WriteLine(message);
            }
        }

        public void DisplayMessage(string message)
        {
            displayMessageStatic(message);
        }
    }
}
