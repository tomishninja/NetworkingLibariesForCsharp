using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;


namespace NetworkingLibrary
{
    public class ListenerSocket
    {
        /// <summary>
        /// Contains all of the states this network connection can trasfer though
        /// </summary>
        public enum State { Created, Started, Listening, Stoped };

        /// <summary>
        /// provides the current state of the system as a State enum
        /// </summary>
        private State currentState = State.Created;

        /// <summary>
        /// provides the current state of the system as a State enum
        /// </summary>
        public State CurrentState { get => currentState; }

        /// <summary>
        /// 
        /// </summary>
        private DatagramSocket Listener = new DatagramSocket();

        /// <summary>
        /// List containing all available local HostName endpoints
        /// </summary>
        private List<LocalHostItem> localHostItems = new List<LocalHostItem>();

        /// <summary>
        /// A string represention of the port number that will be used
        /// </summary>
        readonly string ServiceName = null;

        /// <summary>
        /// If this object isn't null it will give this libary a method of 
        /// comminicating with the rest of the system.
        /// </summary>
        readonly IDisplayMessage MessageHost = null;

        /// <summary>
        /// the default constuctor for this object. This constuctor will assume 
        /// that there are no messages or responces that need to work and 
        /// the system will use it's default port from the main class
        /// </summary>
        public ListenerSocket()
        {
            this.ServiceName = NetworkingLibaryCore.DefaultServiceName;
        }

        /// <summary>
        /// this constuctor uses the default port from the main class
        /// but allows for messages to be sent to a object implementing 
        /// the IDisplayMessage interface
        /// </summary>
        /// <param name="messageHost">
        /// The object that implements the IDisplayMessage interface.
        /// </param>
        public ListenerSocket(IDisplayMessage messageHost)
        {
            this.MessageHost = messageHost;
            this.ServiceName = NetworkingLibaryCore.DefaultServiceName;
        }

        /// <summary>
        /// Creates a listener socket were the port number or service name
        /// can be chosen and it is capable of reciving messages though an
        /// object that implements the IDisplayMessage Class
        /// </summary>
        /// <param name="serviceName">
        /// 
        /// </param>
        /// <param name="messageHost">
        /// 
        /// </param>
        public ListenerSocket(string serviceName, IDisplayMessage messageHost)
        {
            this.ServiceName = serviceName;
            this.MessageHost = messageHost;
        }

        /// <summary>
        /// A constutor that allows for the port number or service name 
        /// to be changed without any messages or responce options.
        /// </summary>
        /// <param name="serviceName">
        /// A string object represting the Service name or port number
        /// </param>
        public ListenerSocket(string serviceName)
        {
            this.ServiceName = serviceName;
        }

        /// <summary>
        /// Starts the listener object.
        /// </summary>
        /// <exception cref="Exception">
        /// Will throw a generic exception when there are issues with the listener
        /// This method should be used within a try catch block
        /// </exception>
        public async void Start()
        {
            // Set the current state
            currentState = State.Started;

            // set lisener behaviour when accessed via a message
            Listener.MessageReceived += MessageReceived;

            // TODO Set Inbound Buffer Size

            // TODO Selected Local Host

            // Starting the listener operation
            // Note will need to change for differnt bind options this is aimed at allowing the user to bind to any address
            try
            {
                currentState = State.Listening;

                await Listener.BindServiceNameAsync(this.ServiceName);
            }
            catch (Exception exception)
            {
                NotifyUserFromAsyncThread("Connect failed with error: " + exception.Message);
            }
        }

        /// <summary>
        /// Message received listener. Once the object has started this listener will
        /// activate during each message that is recived.
        /// </summary>
        /// <param name="socket">
        /// The socket object
        /// </param>
        /// <param name="eventArguments">
        /// The datagram event information
        /// </param>
        async void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs eventArguments)
        {
            // send the echo message to the client if the information is easily avliable
            if (CoreApplication.Properties.TryGetValue("remotePeer", out object outObj))
            {
                EchoMessage((RemotePeer)outObj, eventArguments);
                return;
            }

            // We do not have an output stream yet so create one.
            try
            {
                IOutputStream outputStream = await socket.GetOutputStreamAsync(
                    eventArguments.RemoteAddress,
                    eventArguments.RemotePort);

                // It might happen that the OnMessage was invoked more than once before the GetOutputStreamAsync call
                // completed. In this case we will end up with multiple streams - just keep one of them.
                RemotePeer peer;
                lock (this)
                {
                    // get the other client or peer out of the applicaition properties if it exists.
                    // if it dosn't exist there create a new peer from avalible information and
                    // save it to the properties.
                    if (CoreApplication.Properties.TryGetValue("remotePeer", out outObj))
                    {
                        peer = (RemotePeer)outObj;
                    }
                    else
                    {
                        peer = new RemotePeer(outputStream, eventArguments.RemoteAddress, eventArguments.RemotePort);
                        CoreApplication.Properties.Add("remotePeer", peer);
                    }
                }
                
                // Send the echo message to the responder
                EchoMessage(peer, eventArguments);
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                // if there is an exception send it to the view if possible
                NotifyUserFromAsyncThread("Connect failed with error: " + exception.Message);
            }
        }



        /// <summary>
        /// Echo the message back to the peer
        /// </summary>
        /// <param name="peer">The remote peer object</param>
        /// <param name="eventArguments">The received message event arguments</param>
        async void EchoMessage(RemotePeer peer, DatagramSocketMessageReceivedEventArgs eventArguments)
        {
            string receivedMessage = null;
            // Interpret the incoming datagram's entire contents as a string.

            try
            {
                // get the message
                uint stringLength = eventArguments.GetDataReader().UnconsumedBufferLength;
                receivedMessage = eventArguments.GetDataReader().ReadString(stringLength);

                // Data Writer
                IDataWriter dataWriter = new DataWriter();
                dataWriter.WriteBytes(Encoding.Unicode.GetBytes(receivedMessage));
                await peer.OutputStream.WriteAsync(dataWriter.DetachBuffer());
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                // send this expection to the view
                NotifyUserFromAsyncThread("Send failed with Exception: " + exception.Message);
            }
            
            // display message
            try
            {
                NotifyUserFromAsyncThread(  "Received data: \"" + receivedMessage + "\"");
            }
            catch (Exception exception)
            {
                // if trying to 
                SocketErrorStatus socketError = SocketError.GetStatus(exception.HResult);
                if (socketError == SocketErrorStatus.ConnectionResetByPeer)
                {
                    // This error would indicate that a previous send operation resulted in an 
                    // ICMP "Port Unreachable" message.
                    NotifyUserFromAsyncThread(
                        "Peer does not listen on the specific port. Please make sure that you run step 1 first " +
                        "or you have a server properly working on a remote server.");
                }
                else if (socketError != SocketErrorStatus.Unknown)
                {
                    NotifyUserFromAsyncThread(
                        "Error happened when receiving a datagram: " + socketError.ToString());
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Notifies the user from a non-UI thread
        /// </summary>
        /// <param name="strMessage">The message</param>
        /// <param name="type">The type of notification</param>
        private void NotifyUserFromAsyncThread(string strMessage)
        {
            if (this.MessageHost != null)
            {
                this.MessageHost.DisplayMessage(strMessage);
            }
        }

        /// <summary>
        /// Close this object so it no longer keeps using its thread or memory
        /// </summary>
        public void Close()
        {
            this.Listener.Dispose();
        }

        /// <summary>
        /// Helper class used to identify and respond to a remote peer
        /// </summary>
        class RemotePeer
        {
            /// <summary>
            /// This will contain the bytes that are being sent via
            /// the network.
            /// </summary>
            readonly IOutputStream outputStream;

            /// <summary>
            /// This is the URL the string local host or the ipaddress
            /// stored within an object that is capable of acting like a
            /// host
            /// </summary>
            readonly HostName hostName;

            /// <summary>
            /// The port number represented by a string
            /// </summary>
            readonly String port;

            /// <summary>
            /// Builds a remote peer so it can be stored in data stuctures.
            /// </summary>
            /// <param name="outputStream">
            /// represents a stream of bytes that have come from the remote peer
            /// </param>
            /// <param name="hostName">
            /// A hostName object that will represent the remote device
            /// </param>
            /// <param name="port">
            /// the port number this peer is operating on.
            /// </param>
            public RemotePeer(IOutputStream outputStream, HostName hostName, String port)
            {
                this.outputStream = outputStream;
                this.hostName = hostName;
                this.port = port;
            }

            /// <summary>
            /// This method determines if the host's name and port 
            /// are identical to the ones provided. 
            /// </summary>
            /// <param name="hostName">
            /// The host name object from the other peer
            /// </param>
            /// <param name="port">
            /// The port number as a string from the other peer
            /// </param>
            /// <returns>
            /// true if the hostname and port number are both the same.
            /// for all other cases this method will return false.
            /// </returns>
            public bool IsMatching(HostName hostName, String port)
            {
                // check these objects against each other and return the given ports.
                return (this.hostName == hostName && this.port == port);
            }

            /// <summary>
            /// Getter for the Ouptut stream
            /// </summary>
            public IOutputStream OutputStream
            {
                get { return outputStream; }
            }

            /// <summary>
            /// Returns the object as a string
            /// this will be the host name capped off with
            /// the port number deliminatied by a : character
            /// </summary>
            /// <returns>
            /// A string represention of this object
            /// </returns>
            public override String ToString()
            {
                return hostName + ":" + port;
            }
        }

        /// <summary>
        /// Helper class describing a NetworkAdapter and its associated IP address
        /// </summary>
        class LocalHostItem
        {
            /// <summary>
            /// gets a string object that descibes this object in a readable fashion
            /// </summary>
            public string DisplayString
            {
                get;
                private set;
            }

            /// <summary>
            /// gets the hostName object containing the local host
            /// </summary>
            public HostName LocalHost
            {
                get;
                private set;
            }

            /// <summary>
            /// Constuctor for the helper object. 
            /// </summary>
            /// <param name="localHostName">
            /// The host name for the local host object
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// will be thrown if the host name is null or if
            /// the hostNames IP information is null.
            /// </exception>
            public LocalHostItem(HostName localHostName)
            {
                if (localHostName == null)
                {
                    throw new ArgumentNullException("localHostName");
                }

                if (localHostName.IPInformation == null)
                {
                    throw new ArgumentException("Adapter information not found");
                }

                this.LocalHost = localHostName;
                this.DisplayString = "Address: " + localHostName.DisplayName +
                    " Adapter: " + localHostName.IPInformation.NetworkAdapter.NetworkAdapterId;
            }
        }
    }
}
