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
        public enum State { Created, Started, Listening, Stoped };

        private State currentState = State.Created;
        public State CurrentState { get => currentState; }

        private DatagramSocket Listener = new DatagramSocket();

        // List containing all available local HostName endpoints
        private List<LocalHostItem> localHostItems = new List<LocalHostItem>();

        readonly string ServiceName = null;
        readonly iDisplayMessage MessageHost = null;

        public ListenerSocket()
        {
            this.ServiceName = Class1.DefaultServiceName;
        }

        public ListenerSocket(iDisplayMessage messageHost)
        {
            this.MessageHost = messageHost;
            this.ServiceName = Class1.DefaultServiceName;
        }

        public ListenerSocket(string serviceName, iDisplayMessage messageHost)
        {
            this.ServiceName = serviceName;
            this.MessageHost = messageHost;
        }

        public ListenerSocket(string serviceName)
        {
            this.ServiceName = serviceName;
        }


        /// <summary>
        /// 
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
        /// Message received handler
        /// </summary>
        /// <param name="socket">The socket object</param>
        /// <param name="eventArguments">The datagram event information</param>
        async void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs eventArguments)
        {
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
            /*if (!peer.IsMatching(eventArguments.RemoteAddress, eventArguments.RemotePort))
            {
                // In the sample we are communicating with just one peer. To communicate with multiple peers, an
                // application should cache output streams (e.g., by using a hash map), because creating an output
                // stream for each received datagram is costly. Keep in mind though, that every cache requires logic
                // to remove old or unused elements; otherwise, the cache will turn into a memory leaking structure.
                NotifyUserFromAsyncThread(
                    String.Format(
                        "Got datagram from {0}:{1}, but already 'connected' to {2}",
                        eventArguments.RemoteAddress,
                        eventArguments.RemotePort,
                        peer));
            }*/

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
