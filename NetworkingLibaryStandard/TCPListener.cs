using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkingLibaryStandard
{
    public class TCPListener
    {
        readonly TcpListener Server = null;

        readonly int PortNumber = 0;

        readonly IPAddress ipAddress = null;

        Thread listenerThread = null;

        private bool IsListening = false;

        public TCPListener()
        {
            PortNumber = Class1.DefaultPortNumber;
            ipAddress = IPAddress.Parse(Class1.LocalHostString);
            this.Server = new TcpListener(this.ipAddress, this.PortNumber);
        }

        public void Start()
        {
            // start the server
            this.Server.Start();

            // start a listening thread for the object
            listenerThread = new Thread(Listen);
            listenerThread.Start();

            IsListening = true;
        }

        void Listen()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            try
            {
                while (IsListening)
                {
                    // the command will sit and wait until you can connect
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = Server.AcceptTcpClient();

                    // clean up the data value
                    data = null;

                    // this object handels reading and writing
                    NetworkStream stream = client.GetStream();

                    // this feild holds the bytes recived by the packet
                    int i;

                    // loop though all of the data recived by the client
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data into a string (Ascii)
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // process data
                        data = data.ToUpper();

                        // compile more of the message
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // send a responce
                        this.Respond(stream, msg, data);
                    }

                    // close the client
                    client.Close();
                }
            }
            catch (SocketException exc)
            {
                Console.WriteLine("SocketException: {0}", exc);
            }
            finally
            {
                // stop the server after an before this method ends
                Server.Stop();
            }
        }

        void Respond(NetworkStream stream, byte[] msg, string data)
        {
            // Send back a response.
            //stream.Write(msg, 0, msg.Length);
            //Console.WriteLine("Sent: {0}", data);
        }

        public void Close()
        {
            if (Server != null)
                this.Server.Stop();

            IsListening = false;
        }

    }
}
