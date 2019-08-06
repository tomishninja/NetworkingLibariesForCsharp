using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// This object represents the client side of a 
    /// UDP connection
    /// </summary>
    public class UDPClient
    {
        /// <summary>
        /// A object that represents a networks end point
        /// This object basicly just stops packets from passing by
        /// This object usally has this object checking all UDP packets it gets
        /// so it isn't a very strict endpoint
        /// </summary>
        IPEndPoint EndPoint = null;

        /// <summary>
        /// This feild will keep the the listener running until the end unless 
        /// there are any future issues
        /// </summary>
        bool IsListenerRunning = false;

        /// <summary>
        /// This will contain the object that can recive messages from this
        /// object while it is operating
        /// </summary>
        IDisplayMessage messageSystem = null;

        /// <summary>
        /// This object allows for different methods of responces with data.
        /// </summary>
        IResponder responder = null;

        /// <summary>
        /// A lock to create a queue for messages if they are recived 
        /// in rapid succession. found in the responce function.
        /// </summary>
        private static object ResponceLock = new object();

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

        public void Close()
        {
            this.Disconnect();
        }

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
            this.portNumber = NetworkingLibaryStandard.DefaultPortNumber;
            this.hostAddress = NetworkingLibaryStandard.LocalHostString;

            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.Client = new System.Net.Sockets.UdpClient(NetworkingLibaryStandard.DefaultPortNumber);
        }

        public UDPClient(IDisplayMessage messageHelper)
        {
            this.portNumber = NetworkingLibaryStandard.DefaultPortNumber;
            this.hostAddress = NetworkingLibaryStandard.LocalHostString;
            this.messageSystem = messageHelper;
            
            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.Client = new System.Net.Sockets.UdpClient(portNumber);
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
            this.hostAddress = NetworkingLibaryStandard.LocalHostString;

            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.Client = new System.Net.Sockets.UdpClient(portNumber);
        }



        /// <summary>
        /// A constuctor that allows the user to change the port number from default
        /// but still uses local host for the ip address
        /// </summary>
        /// <param name="portNumber">
        /// A integer that is above zero and ideally less than 655535
        /// </param>
        public UDPClient(int portNumber, string hostAddress)
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
            this.hostAddress = hostAddress;

            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.Client = new System.Net.Sockets.UdpClient(portNumber);
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
            this.portNumber = NetworkingLibaryStandard.DefaultPortNumber;
            this.hostAddress = hostAddress;

            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.Client = new UdpClient(NetworkingLibaryStandard.DefaultPortNumber);
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
        /// <param name="messageHelper">
        /// This is a object that inherits IDisplayMessage.
        /// </param>
        public UDPClient(string hostAddress, IDisplayMessage messageHelper)
        {
            //TODO check that the host address is valid
            this.portNumber = NetworkingLibaryStandard.DefaultPortNumber;
            this.hostAddress = hostAddress;

            // set up a way for this system to contact the outside
            this.messageSystem = messageHelper;

            // Set up a end point that dosn't exclued any possible end points
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            // set up the end point client
            this.Client = new UdpClient(NetworkingLibaryStandard.DefaultPortNumber);
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
                Client = new UdpClient(this.portNumber);
            }

            // if the program hasn't already started start it
            if (!Connected)
            {
                Client.Connect(this.hostAddress, this.portNumber);
                this.Connected = true;
            }

            // set up the listener funcionality
            // start a listening thread for the object
            IsListenerRunning = true;
            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        /// <summary>
        /// triggered from the start funciton and run in it's own thread.
        /// </summary>
        private void Listen()
        {
            // keep looping over the listener code until the IsConnnected veriable
            // changes or a exception is triggered
            try
            {
                while (this.IsListenerRunning)
                {
                    Byte[] receiveBytes = this.Client.Receive(ref this.EndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    // if there is a message outlet send the data there
                    if (messageSystem != null)
                    {
                        messageSystem.DisplayMessage(MessageHelper.MessageType.Data, returnData);
                    }

                    // will activiate the responder if nessarcary 
                    //Responce(returnData, ref this.EndPoint);
                }
            }
            catch (Exception ex)
            {
                if (messageSystem != null)
                {
                    this.messageSystem.DisplayMessage(MessageHelper.MessageType.Exception, "Exception: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// this method is triggered by the listen function. it's main job is to saftly call
        /// the respond mehtod if it exists.
        /// </summary>
        /// <param name="dataRecived">
        /// this is a string represention of the 
        /// </param>
        /// <param name="endpoint">
        /// Details about the other side of the coversation
        /// </param>
        private void Responce(string dataRecived, ref IPEndPoint endpoint)
        {
            //create a client to send the data back to
            //System.Net.Sockets.UdpClient client = new System.Net.Sockets.UdpClient(endpoint.Port, endpoint.AddressFamily);

            if (responder != null)
            {
                // create a lock on this so there are no collisons or erros
                lock (ResponceLock)
                {
                    // call the responder method to send the new information
                    if (responder != null)
                    {
                        responder.Respond(dataRecived, this.Client, ref endpoint);
                    }
                }
            }
            else
            {
                // Code to echo back a responce
                byte[] data = Encoding.Unicode.GetBytes(dataRecived);
                this.Client.SendAsync(data, data.Length, endpoint);
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

            this.IsListenerRunning = false;

            if (Client != null)
            {
                this.Client.Close();
            }
        }
    }
}
