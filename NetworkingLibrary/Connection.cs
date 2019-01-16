using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace NetworkingLibrary
{
    public class Connection
    {
        public enum State { Created, Started, Stoped }

        private State currentState = State.Created;
        public State CurrentState { get => currentState; }

        public readonly string serviceName = null;
        public readonly string hostName = null;

        readonly iDisplayMessage MessageHost = null;

        private DatagramSocket Server = null;

        private DataWriter Writer = null;

        public Connection()
        {
            this.serviceName = Class1.DefaultServiceName;
            this.hostName = Class1.LocalHostName;
        }

        public Connection(iDisplayMessage messageHost)
        {
            this.serviceName = Class1.DefaultServiceName;
            this.hostName = Class1.LocalHostName;
            this.MessageHost = messageHost;
        }

        public Connection(string serviceName, string hostName, iDisplayMessage messageHost)
        {
            this.serviceName = serviceName;
            this.hostName = hostName;
            this.MessageHost = messageHost;
        }

        public async Task StartAsync()
        {
            this.currentState = State.Started;

            HostName hostName;
            try
            {
                hostName = new HostName(this.hostName);
            }
            catch (ArgumentException exception)
            {
                NotifyUserFromAsyncThread("Error: Invalid host name." + exception.Message);
                return;
            }

            // Create the socket
            Server = new DatagramSocket();

            //TODO add Fragment options here

            // add the behaviour for the listener
            this.Server.MessageReceived += MessageReceived;

            //NotifyUserFromAsyncThread("Connecting to " + this.hostName);

            try
            {
                // Connect to the server (by default, the listener we created in the previous step).
                await this.Server.ConnectAsync(hostName, this.serviceName);
            }
            catch (Exception exception)
            {
                NotifyUserFromAsyncThread("Connect failed with error: " + exception.Message);
            }
        }

        public async void Send(string message)
        {
            if (this.currentState != State.Started)
            {
                this.NotifyUserFromAsyncThread("Please start the object before sending messages");
                return;
            }

            // if  the writer is null create a new writer
            if (this.Writer == null)
            {
                this.Writer = new DataWriter(this.Server.OutputStream);
            }

            Writer.WriteString(message);

            try
            {
                await Writer.StoreAsync();
                //this.NotifyUserFromAsyncThread("Message has been sent");

            }
            catch(Exception exception)
            {
                // If this is an unknown status it means that the error if fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                NotifyUserFromAsyncThread("Send failed with error: " + exception.Message);
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

        void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs eventArguments)
        {
            try
            {
                // Interpret the incoming datagram's entire contents as a string.
                uint stringLength = eventArguments.GetDataReader().UnconsumedBufferLength;
                string receivedMessage = eventArguments.GetDataReader().ReadString(stringLength);

                NotifyUserFromAsyncThread(
                    "Received data from remote peer: \"" +
                    receivedMessage + "\"");
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

        public void Close()
        {
            this.Server.Dispose();
            this.Writer.DetachStream();
            this.Writer.Dispose();
        }
    }
}
