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
        readonly IPAddress ipAddress = null;

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
        /// Defualt constuctor for the TCP object
        /// </summary>
        public TCPListener()
        {
            PortNumber = NetworkingLibaryStandard.DefaultPortNumber;
            ipAddress = IPAddress.Parse(NetworkingLibaryStandard.LocalHostString);
            this.Server = new TcpListener(this.ipAddress, this.PortNumber);
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
                while (IsListening)
                {
                    // the command will sit and wait until you can connect
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = Server.AcceptTcpClient();

                    // clean up the data value
                    data = "";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    // this object handels reading and writing
                    NetworkStream stream = client.GetStream();

                    // this feild holds the bytes recived by the packet
                    //int i;

                    // loop though all of the data recived by the client
                    /*
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data into a string (Ascii)
                        data += System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        //Console.WriteLine("Received: {0}", data);

                        // process data
                        //data = data.ToUpper();// TODO work out were that is

                        // compile more of the message
                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // send a responce
                        //this.Respond(stream, msg, data);
                    }
                    */
                    do
                    {
                        int numberOfBytesRead = stream.Read(bytes, 0, bytes.Length);
                        sb.Append(System.Text.Encoding.ASCII.GetString(bytes, 0, numberOfBytesRead));
                    } while (stream.DataAvailable);

                    data = sb.ToString();

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

        /// <summary>
        /// This method is designed to be inherited and should not be called 
        /// by the user as it is not desinged for it.
        /// 
        /// This method is designed to respond to a message from the client.
        /// by default this method will do nothing.
        /// </summary>
        /// <param name="stream">
        /// The network stream object recived from the client sending the data
        /// </param>
        /// <param name="msg">
        /// the bytes that created the message
        /// </param>
        /// <param name="data">
        /// A string represention of the message reviced
        /// </param>
        public virtual void Respond(NetworkStream stream, byte[] msg, string data)
        {
            // Send back a response.
            //stream.Write(msg, 0, msg.Length);
            //Console.WriteLine("Sent: {0}", data);
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
