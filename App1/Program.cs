using NetworkingLibrary;
using System;
using System.Threading.Tasks;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace App1
{
    class Program : IDisplayMessage
    {
        /// <summary>
        /// A locking object for the static message method
        /// </summary>
        private static readonly Object obj = new Object();

        static void Main(string[] args)
        {
            // checks the outer loop for when to stop the program
            bool outerLoopCheck = false;

            // this is the maxium amount of choices currently avalible
            int amountOfChoices = 3;

            // the current choice by default this is set out of range
            int choice = amountOfChoices + 1;

            do
            {
                // reset this fater each loop
                outerLoopCheck = false;

                // keep looping though code as expected
                do
                {
                    // Prompt the user for input
                    Console.WriteLine("Please select the Program you wish to run:");
                    Console.WriteLine("1: UDP Hello World");
                    Console.WriteLine("2: TCP Hello World");
                    Console.WriteLine("3: TCP Hello World With no Responce");


                    // Read the Users Input
                    string input = Console.ReadLine();
                    // as if the input was valid int save the choice as a choice

                    try
                    {
                        choice = Int16.Parse(input.Trim());
                    }
                    catch (ArgumentNullException)
                    {
                        Console.WriteLine("Please enter some text");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid Int");
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("Please Don't be smart");
                    }
                    // keep doing this if the choice isn't valid
                } while (choice > amountOfChoices && choice < 0);

                // once a choice is valid then move foward

                // now we look at what the user chose and we run that program
                switch (choice)
                {
                    case 1:
                        RunUDPHelloWorld();
                        break;
                    case 2:
                        RunTCPHelloWorld();
                        break;
                    case 3:
                        RunTCPHelloWorldWithNoResponce();
                        break;
                    default:
                        outerLoopCheck = true;
                        break;
                }
            } while (outerLoopCheck);


            Console.ReadLine();
        }

        static void RunUDPHelloWorld()
        {
            Program program = new Program();
            program.RunUDPHelloWorldTest();
        }

        static void RunTCPHelloWorldWithNoResponce()
        {
            Program program = new Program();
            program.RunTCPHelloWorldTest(false);
        }

        static void RunTCPHelloWorld()
        {
            Program program = new Program();
            program.RunTCPHelloWorldTest(true);
        }

        public void RunTCPHelloWorldTest(bool withResponce)
        {
            displayMessageStatic("Starting TCP Hello World Test:");

            NetworkingLibrary.TCPServer server;
            // start the listener socket
            if (withResponce)
            {
                server = new NetworkingLibrary.TCPServer(this, new EchoResponder());
            }
            else
            {
                server = new NetworkingLibrary.TCPServer(this);
            }
            server.Start();

            displayMessageStatic("Listener Socket Running");

            SetUpTCPConnection();

            Console.ReadLine();
        }

        public async void SetUpTCPConnection()
        {
            TCPClient client = new TCPClient(this);
            displayMessageStatic("Connection Running");

            displayMessageStatic("Sending Messages");

            // Send a request to the echo server.
            string request = "Hello, World!";
            await client.Send(request);
            
            await client.Send(request);

            await SendLotsOfData(client);
        }

        public void RunUDPHelloWorldTest()
        {
            displayMessageStatic("Starting UDP Hello World Test:");

            // start the listener socket
            ListenerSocket listener = NetworkingLibaryCore.GetListener(this);
            listener.Start();

            displayMessageStatic("Listener Socket Running");

            SetUpUDPConnection();

            Console.ReadLine();

        }

        public async void SetUpUDPConnection()
        {
            Connection connection = NetworkingLibaryCore.GetConnection(this);
            displayMessageStatic("Connection Running");
            await connection.StartAsync();

            displayMessageStatic("Sending Messages");
            connection.Send("HelloWorld");
            SendLotsOfData(connection);
        }

        public async Task SendLotsOfData(TCPClient connection)
        {
            Random random = new Random();

            int[] sixNumbers = new int[6];
            int count = 0;
            while (count < 100)
            {
                for (int i = 0; i < sixNumbers.Length; i++)
                {
                    sixNumbers[i] = random.Next(255);
                }

                await connection.Send(ConvertArrayToString(sixNumbers));

                count++;
                Console.WriteLine("Count: " + count);
            }
        }

        public void SendLotsOfData(Connection connection)
        {
            Random random = new Random();

            int[] sixNumbers = new int[6];
            int count = 0;
            while (true)
            {
                for (int i = 0; i < sixNumbers.Length; i++)
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
            for (int i = 0; i < array.Length; i++)
            {
                output += array[i];
                if (i < array.Length - 1)
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

        public void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            if (type == MessageHelper.MessageType.Data)
            {
                displayMessageStatic(message);
            }
            
        }
    }
}
