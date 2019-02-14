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
            // 
            object outObj;
            if (CoreApplication.Properties.TryGetValue("remotePeer", out outObj))
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
                
                EchoMessage(peer, eventArguments);
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

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

                NotifyUserFromAsyncThread("Send failed with error: " + exception.Message);
            }
            
            // display message
            try
            {
                NotifyUserFromAsyncThread(  "Received data: \"" + receivedMessage + "\"");
            }
            catch (Exception exception)
            {
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

        public void Close()
        {
            this.Listener.Dispose();
        }

        /// <summary>
        /// Helper class used to identify and respond to a remote peer
        /// </summary>
        class RemotePeer
        {
            IOutputStream outputStream;
            HostName hostName;
            String port;

            public RemotePeer(IOutputStream outputStream, HostName hostName, String port)
            {
                this.outputStream = outputStream;
                this.hostName = hostName;
                this.port = port;
            }

            public bool IsMatching(HostName hostName, String port)
            {
                return (this.hostName == hostName && this.port == port);
            }

            public IOutputStream OutputStream
            {
                get { return outputStream; }
            }

            public override String ToString()
            {
                return hostName + port;
            }
        }

        /// <summary>
        /// Helper class describing a NetworkAdapter and its associated IP address
        /// </summary>
        class LocalHostItem
        {
            public string DisplayString
            {
                get;
                private set;
            }

            public HostName LocalHost
            {
                get;
                private set;
            }

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
