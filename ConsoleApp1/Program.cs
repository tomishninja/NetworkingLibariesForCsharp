using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkingLibaryStandard;
using LSLFramework;

namespace ConsoleApp1
{
    /// <summary>
    /// 
    /// </summary>
    class Program : NetworkingLibaryStandard.IDisplayMessage, LSLFramework.IDisplayMessage
    {

        bool endFunction = true;

        static void Main(string[] args)
        {
            // check the outer loop
            bool outerLoopCheck = false;
            // this is the amount of choices the user has to pick from
            int amountOfChoices = 8;
            // This is the selection the user made of the system. its set out of bounds by default
            int choice = amountOfChoices + 1;
            do
            {
                // make sure that this value is reset at the start of each itteration
                outerLoopCheck = false;

                // keep looping though this code until it works as expected
                do
                {
                    // Prompt the user for input
                    Console.WriteLine("Please select the Program you wish to run:");
                    Console.WriteLine("1: Listen UDP Version");
                    Console.WriteLine("2: UDP Demo Toms");
                    Console.WriteLine("3: Microsoft UDP Demo Downloaded");
                    Console.WriteLine("4: TCP Demo");
                    Console.WriteLine("5: Run UDP Server");
                    Console.WriteLine("6: Run UDP Client");
                    Console.WriteLine("7: Send Random Data Via UDP Client");
                    Console.WriteLine("8: LSL Recive Floats Test");

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

                } while (choice > amountOfChoices && choice < 0);

                // to get out of that loop the choice must have been valid

                // now we look at what the user chose and we run that program
                switch (choice)
                {
                    case 1:
                        JustListenUDP();
                        break;
                    case 2:
                        UDPDemoToms();
                        break;
                    case 3:
                        MicrosoftUDPClientDemo();
                        break;
                    case 4:
                        TcpDemo();
                        break;
                    case 5:
                        RunUDPServerStatic();
                        break;
                    case 6:
                        RunUDPClientStatic();
                        break;
                    case 7:
                        SendRandomNumbersStatic();
                        break;
                    case 8:
                        StartLSLInletDemo();
                        break;
                    default:
                        outerLoopCheck = true;
                        break;
                }
            } while (outerLoopCheck);
        }

        public static void StartLSLInletDemo()
        {
            Console.WriteLine("Starting LSL Demo");

            Program program = new Program();

            LSLInletFloats lslinlet = new LSLInletFloats("type", "RandCoord", 8, program);
            lslinlet.Start();

            Console.ReadKey();

            lslinlet.Stop();
        }

        /// <summary>
        /// Listens to UDP packets from all addresses
        /// </summary>
        static void JustListenUDP()
        {
            Program program = new Program();
            program.StartListening();
        }

        /// <summary>
        /// 
        /// </summary>
        void StartListening()
        {
            UDPListener listener = new UDPListener(this);
            listener.Start();

            Console.ReadKey();

            listener.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        static void UDPDemoToms()
        {

            Program program = new Program();
            program.RunUDPDemoToms();
        }

        /// <summary>
        /// 
        /// </summary>
        void RunUDPDemoToms()
        {
            UDPClient client = new UDPClient(this);

            UDPListener listener = new UDPListener(NetworkingLibaryStandard.NetworkingLibaryStandard.DefaultPortNumber + 1, this);
            listener.Start();

            client.Start();
            client.Send("Hello World");

            Console.ReadKey();

            client.Disconnect();
            listener.Stop();
        }

        static void RunUDPServerStatic()
        {
            Program program = new Program();
            program.RunUDPServer();
        }

        void RunUDPServer()
        {
            UDPListener listener = new UDPListener(NetworkingLibaryStandard.NetworkingLibaryStandard.DefaultPortNumber, this);
            listener.Start();

            Console.WriteLine("Press any Key to stop");
            Console.ReadKey();

            listener.Stop();
            Console.WriteLine("They System Has stoped");
        }

        static void RunUDPClientStatic()
        {
            Console.WriteLine("Please enter the IP address you wish to contact");
            string hostAddress = Console.ReadLine();

            Program program = new Program();
            program.RunUDPClient(hostAddress, false);
        }

        void RunUDPClient(string hostAddress, bool sendRandomData)
        {
            UDPClient client = new UDPClient(hostAddress, this);

            client.Start();
            client.Send("Hello World");

            if (sendRandomData)
            {
                int waitTimeMilliseconds = 100;

                // call a generater in a different thread to send lots of numbers
                // at specified intervals
                Thread randomNumbersThread = new Thread(() =>
                    SendRandomNumbers(ref client, waitTimeMilliseconds));
                randomNumbersThread.Start();

                //if the user presses the key start trying to stop sending data
                Console.ReadKey();
                Console.WriteLine("Preparing to stop sending data");

                // stop the loop
                this.endFunction = false;

                // wait for a while to make sure your not stoping the thread some were important
                Thread.Sleep(waitTimeMilliseconds * 3); // 3 times the wait time should be more than enough
            }

            // Disconnect the client
            client.Disconnect();
            Console.WriteLine("Program Stoped Sending Data");
        }

        static void SendRandomNumbersStatic()
        {
            Console.WriteLine("Please enter the IP address you wish to contact");
            string hostAddress = Console.ReadLine();

            Program program = new Program();
            program.RunUDPClient(hostAddress, true);
        }

        /// <summary>
        /// This program will send random numbers to a forign device
        /// using UDP packets. it will act as a client
        /// </summary>
        /// <param name="client">
        /// A UDPObject Acting as the client to send data from
        /// </param>
        /// <param name="millisecondsBetweenIterations">
        /// The time between sending data packets
        /// </param>
        void SendRandomNumbers(ref UDPClient client, int millisecondsBetweenIterations)
        {
            // a object to generate the random data to be sent
            Random randomGenerator = new Random();

            // set veriable to true so the app runs
            this.endFunction = true;
            while (this.endFunction)
            {
                // this will be the data to send to the client
                string outputString = "";

                // this loop will build the string to send to the other client
                for (int i = 0; i < 6; i++)
                {
                    outputString += randomGenerator.Next(256);

                    // add a deliminator to the items
                    if (i < 5)
                    {
                        outputString += ",";
                    }
                }


                Thread.Sleep(millisecondsBetweenIterations);

                //
                client.Send(outputString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void TcpDemo()
        {
            TCPListener listener = new TCPListener();
            TCPClient client = new TCPClient("127.0.0.1");

            listener.Start();

            client.Start();

            client.Send("Hello World");

            for (int i = 0; i < 100; i++)
            {
                client.Send(getRandomCoords());
            }

            Console.ReadKey();

            client.Close();
            listener.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static string getRandomCoords()
        {
            Random random = new Random();
            string output = "";

            for (int i = 0; i < 6; i++)
            {
                if (i != -1)
                {
                    output += ", ";
                }

                output += random.Next(360);
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        static void MicrosoftUDPClientDemo()
        {
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                udpClient.Connect("www.contoso.com", 11000);

                // Sends a message to the host to which you have connected.
                Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");

                udpClient.Send(sendBytes, sendBytes.Length);

                // Sends a message to a different host using optional hostname and port parameters.
                UdpClient udpClientB = new UdpClient();
                udpClientB.Send(sendBytes, sendBytes.Length, "www.google.com", 11000);

                //IPEndPoint object will allow us to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClientB.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);

                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());

                udpClient.Close();
                udpClientB.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Display a message from the network standard libary
        public static object obj = new object();
        public void DisplayMessage(NetworkingLibaryStandard.MessageHelper.MessageType type, string message)
        {
            lock (obj)
            {
                message = message.Replace("\0", "");
                Console.WriteLine(message);
            }
        }

        // Display a message from the LSL framework
        public void DisplayMessage(string message)
        {
            lock (obj)
            {
                Console.WriteLine(message);
            }
        }

    }
}
