using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// 
    /// </summary>
    public class UDPClient
    {
        /// <summary>
        /// A readonly integer the represents the port number the user wishes to use.
        /// This will be a matching port number to the servers this client wishes 
        /// to communciate with. This client will operate on the port number
        /// one higher.
        /// </summary>
        private readonly int portNumber = 0;

        /// <summary>
        /// A readonly primitve sting representing the host address.
        /// options avalible are "localhost", a ip4 or ip6 address, or a url
        /// </summary>
        private readonly string hostAddress = null;

        /// <summary>
        /// The main udp client object. this object provides much of the interface 
        /// between these services.
        /// </summary>
        private UdpClient Client = null;

        /// <summary>
        /// This boolean tells the system wheather or not it is connected
        /// to the overall system. 
        /// </summary>
        private bool Connected = false;

        /// <summary>
        /// The default constuctor for the UDPClient object. 
        /// This object holds 
        /// </summary>
        public UDPClient()
        {
            this.portNumber = Class1.DefaultPortNumber;
            this.hostAddress = Class1.LocalHostString;
        }

        /// <summary>
        /// A constuctor that allows the user to change the port number from default
        /// but still uses local host for the ip address
        /// </summary>
        /// <param name="portNumber">
        /// A integer that is above zero and ideally less than 655535
        /// </param>
        public UDPClient(int portNumber)
        {
            // if the argument is negivtive then send back an exception
            if (portNumber < 0)
            {
                throw new ArgumentException(
                    "invalid port number given. Value must be a postive number"
                    );
            }

            // set the values
            this.portNumber = portNumber;
            this.hostAddress = Class1.LocalHostString;
        }

        /// <summary>
        /// A constuctor that uses the default host address but does not use the
        /// default serial numbers. 
        /// </summary>
        /// <param name="hostAddress">
        /// a stirng primitive object that represents a host address
        /// this could either be "localhost" and ip address (either 4 or 6) 
        /// or a URL
        /// </param>
        public UDPClient(string hostAddress)
        {
            //TODO check that the host address is valid
            this.portNumber = Class1.DefaultPortNumber;
            this.hostAddress = hostAddress;
        }

        /// <summary>
        /// Starts this object allowing it to send messages
        /// to its client
        /// </summary>
        public void Start()
        {
            // if the main object hasn't been made yet make it
            if (this.Client == null)
            {
                Client = new UdpClient(this.portNumber+1);
            }

            // if the program hasn't already started start it
            if (!Connected)
            {
                Client.Connect(this.hostAddress, this.portNumber);
                this.Connected = true;
            }
        }

        /// <summary>
        /// Sends information to the client 
        /// </summary>
        /// <param name="message">
        /// A string representation of the message to be sent
        /// </param>
        public void Send(string message)
        {
            // Make sure the object is in the correct state before running
            if (Connected)
            {
                // transfer the string message into a byte message
                Byte[] packageContent = Encoding.ASCII.GetBytes(message);

                // send the byte array.
                this.Client.Send(packageContent, packageContent.Length);
            }
        }
        
        /// <summary>
        /// Will stop the client from operating
        /// stop the object from being able to send messages
        /// </summary>
        public void Disconnect()
        {
      
            this.Client.Close();
            this.Connected = false;
        }
    }
}
