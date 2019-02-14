using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// This function creates a standard TCP connection and is capable 
    /// of sending strings to the disired host.
    /// </summary>
    public class TCPClient
    {
        /// <summary>
        /// A read only int that is set on the creation of this object.
        /// Representing the port number that TCP connection needs to use
        /// </summary>
        readonly int PortNumber = 0;

        /// <summary>
        /// A read only string that will be turned into a host name once the
        /// start function has been triggered. 
        /// </summary>
        readonly string ServerHostName = null;

        /// <summary>
        /// The TCP client object that is created after the start method has been triggered.
        /// This veriable will be overwriten everytime the start function has been triggered.
        /// </summary>
        private TcpClient client = null;

        /// <summary>
        /// Default constuctor this will set the Port Number 
        /// and the Server Host Name to the default ones 
        /// previouly listed.
        /// </summary>
        public TCPClient()
        {
            this.PortNumber = Class1.DefaultPortNumber;

            this.ServerHostName = Class1.LocalHostString;
        }

        /// <summary>
        /// Creates this object allowing 
        /// </summary>
        /// <param name="portNumber">
        /// a int primitive that will represent the port number of this object
        /// </param>
        /// <param name="hostName">
        /// A string primtive that represents the host's name
        /// This will either be an ipAddress (IP4 tested) or the
        /// string "localhost" will also work.
        /// </param>
        public TCPClient(int portNumber, string hostName)
        {
            // validate the parameters
            // TODO: validate host name
            if (portNumber < 0 || hostName == null)
            {
                throw new ArgumentException("one or more arguments have invalid values");
            }

            // set the values
            this.PortNumber = portNumber;
            this.ServerHostName = hostName;
        }

        /// <summary>
        /// Constuctor for this object keeps the 
        /// </summary>
        /// <param name="hostName">
        /// A string primtive that represents the host's name
        /// This will either be an ipAddress (IP4 tested) or the
        /// string "localhost" will also work.
        /// </param>
        public TCPClient(string hostName)
        {
            //TODO: validate the host name

            // set the values
            this.PortNumber = Class1.DefaultPortNumber;
            this.ServerHostName = hostName;
        }

        /// <summary>
        /// Starts the network stream if none currently exists
        /// </summary>
        public void Start()
        {
            // if the client does not exist create it or else do nothing. 
            if (this.client == null)
                this.client = new TcpClient(this.ServerHostName, this.PortNumber);
        }

        /// <summary>
        /// This function sends a message to a client that is at the 
        /// Server host name listening on the specified port number.
        /// </summary>
        /// <param name="message">
        /// The message to be sent to the sever
        /// </param>
        public void Send(string message)
        {
            if (this.client != null)
            {
                try
                {
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    NetworkStream stream = client.GetStream();

                    stream.Write(data, 0, data.Length);

                    Console.WriteLine("Sent: {0}", message);

                    // TODO: stuff to be removed
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

        /// <summary>
        /// Stops the client from running saving memory
        /// </summary>
        public void Close()
        {
            this.client.Close();
            this.client = null;
        }
    }
}
