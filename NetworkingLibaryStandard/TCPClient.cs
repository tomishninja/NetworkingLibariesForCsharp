using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkingLibaryStandard
{
    public class TCPClient
    {
        //
        readonly int PortNumber = 0;

        //
        readonly string ServerHostName = null;

        //
        private TcpClient client = null;


        private NetworkStream stream = null;

        //
        public TCPClient()
        {
            this.PortNumber = Class1.DefaultPortNumber;

            this.ServerHostName = Class1.LocalHostString;
        }

        public void Start()
        {
            if (this.client == null)
                this.client = new TcpClient(this.ServerHostName, this.PortNumber);
        }

        public void Send(string message)
        {
            if (this.client != null)
            {
                try
                {
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    stream = client.GetStream();

                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    //data = new Byte[256];

                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    //String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    //Console.WriteLine("Received: {0}", responseData);
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
            }
        }

        public void Close()
        {
            this.stream.Close();
            this.client.Close();
        }
    }
}
