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
        private static readonly Object objA = new Object();

        static void Main(string[] args)
        {
            // checks the outer loop for when to stop the program
            bool outerLoopCheck = false;

            // this is the maxium amount of choices currently avalible
            int amountOfChoices = 5;

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
                    Console.WriteLine("4: New TCP low volume message test");
                    Console.WriteLine("5: New TCP moderate volume message test");


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
                    case 4:
                        SendHelloWorldBackAndForth();
                        break;
                    case 5:
                        SendALotBackAndForth();
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

        static void SendHelloWorldBackAndForth()
        {
            Program program = new Program();
            program.RunHelloWorldEchoServer("Hello World!");
        }

        static void SendALotBackAndForth()
        {
            Program program = new Program();
            program.RunHelloWorldEchoServer(ARandomString);
        }

        public void RunHelloWorldEchoServer(string msg)
        {
            displayMessageStatic("Starting");

            NetworkingLibrary.TCPServer server = new NetworkingLibrary.TCPServer(messageService: this, responder: new EchoResponder());

            NetworkingLibrary.TCPClient client = new NetworkingLibrary.TCPClient(messageService: this, responder: new EchoResponder());

            server.Start();

            // keep sending messages constantly
            client.SendContinuiously(msg);

            DateTime end = DateTime.Now;
            end.AddMinutes(1);
            while (DateTime.Now < end) { }
        }

        public void RunTCPHelloWorldTest(bool withResponce)
        {
            displayMessageStatic("Starting TCP Hello World Test:");

            NetworkingLibrary.TCPServer server;

            // start the listener socket
            if (withResponce)
            {
                server = new NetworkingLibrary.TCPServer(messageService: this, responder: new EchoResponder());
            }
            else
            {
                server = new NetworkingLibrary.TCPServer(messageService: this);
            }

            server.Start();

            displayMessageStatic("Listener Socket Running");

            SetUpTCPConnection();

            Console.ReadLine();
        }

        public async void SetUpTCPConnection()
        {
            TCPClient client = new TCPClient(messageService: this);
            displayMessageStatic("Connection Running");

            displayMessageStatic("Sending Messages");

            // Send a request to the echo server.
            string request = "Hello, World!";
            await client.Send(request);

            //await client.Send(request);

            //await SendLotsOfData(client);
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
            lock (objA)
            {
                if (type == MessageHelper.MessageType.Data)
                {
                    displayMessageStatic("Data: " + message);
                }
                else if (type == MessageHelper.MessageType.Status)
                {
                    displayMessageStatic("Status: " + message);
                }
                else if (type == MessageHelper.MessageType.Exception)
                {
                    displayMessageStatic("Exception: " + message);
                }
                else if (type == MessageHelper.MessageType.Error)
                {
                    displayMessageStatic("Error: " + message);
                }

            }

        }

        public const string ARandomString = "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";
    }
}
