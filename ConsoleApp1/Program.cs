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
    class Program : IDisplayMessage
    {
        static void Main(string[] args)
        {
            JustListenUDP();
            //UDPDemoToms();
            //MicrosoftUDPClientDemo();
            //TcpDemo();
        }

        static void JustListenUDP()
        {
            Program program = new Program();
            program.StartListening();
        }

        void StartListening()
        {
            UDPListener listener = new UDPListener(this);
            listener.Start();

            Console.ReadKey();

            listener.Stop();
        }

        static void UDPDemoToms()
        {
            Program program = new Program();
            program.RunUDPDemoToms();
        }

        void RunUDPDemoToms()
        {
            UDPClient client = new UDPClient();

            UDPListener listener = new UDPListener(this);
            listener.Start();

            client.Start();
            client.Send("Hello World");

            Console.ReadKey();

            client.Disconnect();
            listener.Stop();
        }

        static void TcpDemo()
        {
            //TCPListener listener = new TCPListener();
            TCPClient client = new TCPClient("10.160.99.76");

            //listener.Start();

            client.Start();

            client.Send("Hello World");

            /*for(int i = 0; i < 100; i++)
            {
                client.Send(getRandomCoords());
            }*/
            

            Console.ReadKey();

            //client.Close();
            //listener.Close();
        }

        static string getRandomCoords()
        {
            Random random = new Random();
            string output = "";
            
            for(int i = 0; i < 6; i++)
            {
                if (i != -1)
                {
                    output += ", ";
                }

                output += random.Next(360);
            }

            return output;
        }

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
