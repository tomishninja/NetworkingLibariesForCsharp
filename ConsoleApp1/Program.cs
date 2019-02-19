using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkingLibaryStandard;

namespace ConsoleApp1
{
    /// <summary>
    /// 
    /// </summary>
    class Program : IDisplayMessage
    {
        static void Main(string[] args)
        {
            // check the outer loop
            bool outerLoopCheck = false;
            // this is the amount of choices the user has to pick from
            int amountOfChoices = 6;
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
                    default:
                        outerLoopCheck = true;
                        break;
                }
            } while (outerLoopCheck);
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

            UDPListener listener = new UDPListener(NetworkingLibaryStandard.NetworkingLibaryStandard.DefaultPortNumber+1, this);
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
            program.RunUDPClient(hostAddress);
        }

        void RunUDPClient(string hostAddress)
        {
            UDPClient client = new UDPClient(hostAddress, this);

            client.Start();
            client.Send("Hello World");

            Console.ReadKey();

            client.Disconnect();
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

            for(int i = 0; i < 100; i++)
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

        public static object obj = new object();
        public void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            lock (obj)
            {
                Console.WriteLine(message);
            }
        }
    }
}
