using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// The server side of the network connection. 
    /// This side of the tcp is capable of listening
    /// and reacting even to responding
    /// </summary>
    public class TCPListener
    {
        /// <summary>
        /// Tcp listener object that will represent the server.
        /// This object will give this object access to its network
        /// functionality
        /// </summary>
        readonly TcpListener Server = null;

        /// <summary>
        /// The port number that this server is listening on. 
        /// </summary>
        readonly int PortNumber = 0;

        /// <summary>
        /// So far only tested using localhost.
        /// </summary>
        readonly IPAddress IpAddress = null;

        /// <summary>
        /// The data this object generated
        /// </summary>
        public String data = null;

        /// <summary>
        /// The thead that will hold the listener behaviour
        /// </summary>
        Thread listenerThread = null;

        /// <summary>
        /// This boolen determine weather or not the sytem is running and
        /// listening to client threads.
        /// </summary>
        private bool IsListening = false;

        /// <summary>
        /// A object that contains the behaviour for this element.
        /// </summary>
        private ITCPResponder Responder;

        /// <summary>
        /// Message output
        /// </summary>
        private IDisplayMessage Output;

        /// <summary>
        /// Defualt constuctor for the TCP object
        /// </summary>
        public TCPListener(int portNumber = NetworkingLibaryStandard.DefaultPortNumber, string ipAddress = NetworkingLibaryStandard.LocalHostString, ITCPResponder responder = null, IDisplayMessage output = null)
        {
            PortNumber = portNumber;
            IpAddress = IPAddress.Parse(ipAddress);
            this.Server = new TcpListener(this.IpAddress, this.PortNumber);
            Responder = responder;
            Output = output;
        }

        /// <summary>
        /// This function starts the object.
        /// Triggering this function should allow 
        /// this object to begin listening to its clients.
        /// Two listener theads may not be created using this
        /// method.
        /// </summary>
        public void Start()
        {
            if (this.IsListening == false)
            {
                // start the server
                this.Server.Start();

                Console.WriteLine("Server started");

                // set the while loop condition for the listen thread to true
                // so the loop will continue to run indefinatly. 
                IsListening = true;

                // start a listening thread for the object
                listenerThread = new Thread(Listen);
                listenerThread.Start();
            }
        }
        
        /// <summary>
        /// this function should only be run in its own thread. It is triggered
        /// by the start function. 
        /// </summary>
        private void Listen()
        {
            // Buffer for reading data
            Byte[] bytes = new Byte[256];

            try
            {
                // keep listening until the user is done with this object or until a
                // exception is thrown
                TcpClient client = Server.AcceptTcpClient();
                while (IsListening)
                {
                    // the command will sit and wait until you can connect
                    // You could also user server.AcceptSocket() here.

                    // clean up the data value
                    data = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    // this object handels reading and writing
                    NetworkStream stream = client.GetStream();

                    // this feild holds the bytes recived by the packet

                    while (stream.DataAvailable || data == "")
                    {
                        int numberOfBytesRead = stream.Read(bytes, 0, bytes.Length);
                        sb.Append(System.Text.Encoding.ASCII.GetString(bytes, 0, numberOfBytesRead));
                        data = sb.ToString();
                    }
                    
                    if (Output != null)
                    {
                        Output.DisplayMessage(MessageHelper.MessageType.Data, data);
                    }

                    if (Responder != null)
                    {
                        Responder.Respond(stream, data);
                    }
                    // close the client
                }
               client.Close();
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

        /// <summary>
        /// this function will stop this object
        /// from listening to clients on this stream.
        /// </summary>
        public void Close()
        {
            // if the sever exists stop it
            if (Server != null)
                this.Server.Stop();

            // this will stop the loop created by the Listen function
            IsListening = false;
        }

    }
}
