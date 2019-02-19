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
        /// <summary>
        /// Shows the current state of the Connection object
        /// Created: the object isn't null but hasn't been started or stoped yet;
        /// Started: the object has started and can send a message
        /// </summary>
        public enum State { Created, Started, Stoped }

        /// <summary>
        /// This object lets this object communicate its
        /// current state
        /// </summary>
        private State currentState = State.Created;
        /// <summary>
        /// Returns the current state of this object
        /// </summary>
        public State CurrentState { get => currentState; }

        /// <summary>
        /// A readonly string holding the port number in question
        /// </summary>
        public readonly string serviceName = null;

        /// <summary>
        /// the host name of the listening socket that this is connected to
        /// </summary>
        public readonly string hostName = null;

        /// <summary>
        /// This object contains a object that implemetents the 
        /// IDisplayMessage.This allows the sytem to parse the 
        /// algorithms. 
        /// </summary>
        readonly IDisplayMessage MessageHost = null;

        /// <summary>
        /// This is the networking object that this handels all
        /// of the indivual networking commands.
        /// </summary>
        private DatagramSocket Server = null;

        /// <summary>
        /// This object allows for an easy transfer of data between streams.
        /// </summary>
        private DataWriter Writer = null;

        /// <summary>
        /// base constuctor this will save the service name and the host
        /// name to their default values as held in the NetowrkingLibaryCore Class.
        /// This option will have no options for messages to be sent to the view.
        /// </summary>
        public Connection()
        {
            this.serviceName = NetworkingLibaryCore.DefaultServiceName;
            this.hostName = NetworkingLibaryCore.LocalHostName;
        }

        /// <summary>
        /// base constuctor this will save the service name and the host
        /// name to their default values as held in the NetowrkingLibaryCore Class.
        /// This constuctor allows messages to be sent to the view. If this funcitonality
        /// isn't disired either set the message host to null or remove it as a argument.
        /// </summary>
        /// <param name="messageHost">
        /// 
        /// </param>
        public Connection(IDisplayMessage messageHost)
        {
            this.serviceName = NetworkingLibaryCore.DefaultServiceName;
            this.hostName = NetworkingLibaryCore.LocalHostName;
            this.MessageHost = messageHost;
        }

        /// <summary>
        /// This constuctor allows for eveything in the class to be set.
        /// </summary>
        /// <param name="serviceName">
        /// A string primitive that allows the port number to be set within this object.
        /// </param>
        /// <param name="hostName">
        /// A host name object that could either be a URL a ipaddress or the string "localhost"
        /// </param>
        /// <param name="messageHost">
        /// If this object isn't null it will allow the ouput from this
        /// object to be sent to it.
        /// </param>
        public Connection(string serviceName, string hostName, IDisplayMessage messageHost)
        {
            this.serviceName = serviceName;
            this.hostName = hostName;
            this.MessageHost = messageHost;
        }

        /// <summary>
        /// Starts the object in a asyncronisis thread. 
        /// </summary>
        /// <returns>
        /// Nothing of note. A Asyncornis applicaition. Output will be send to 
        /// Message host if it is set
        /// </returns>
        public async Task StartAsync()
        {
            // set the current state of the object to started
            this.currentState = State.Started;

            // turn the hostname from a string to a host name object
            // if this fails notify the user and stop the method
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
                // if an exception happens notifiy the user.
                NotifyUserFromAsyncThread("Connect failed with error: " + exception.Message);
            }
        }

        /// <summary>
        /// Sends data to the other peer or server.
        /// This object must be started inorder for this
        /// operation to work
        /// </summary>
        /// <param name="message">
        /// The data that should be sent to the server/peer
        /// </param>
        public async void Send(string message)
        {
            // if the system hasn't started yet don't run this method.
            if (this.currentState != State.Started)
            {
                // Notify the user and return from this method
                this.NotifyUserFromAsyncThread("Please start the object before sending messages");
                return;
            }

            // if  the writer is null create a new writer
            if (this.Writer == null)
            {
                this.Writer = new DataWriter(this.Server.OutputStream);
            }

            // 
            Writer.WriteString(message);

            // Send the message via the writer object
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

                // if it is not a SocketError notify the user why this happened.
                NotifyUserFromAsyncThread("Send failed with error: " + exception.Message);
            }
        }

        /// <summary>
        /// Notifies the user from a non-UI thread
        /// </summary>
        /// <param name="strMessage">
        /// The message as a string object
        /// </param>
        /// <param name="type">
        /// The type of notification
        /// </param>
        private void NotifyUserFromAsyncThread(string strMessage)
        {
            if (this.MessageHost != null)
            {
                this.MessageHost.DisplayMessage(strMessage);
            }
        }

        /// <summary>
        /// This method acts as a listener and will react whenever the socket recives a message.
        /// </summary>
        /// <param name="socket">
        /// the datagram socket that the message came from and is held in. 
        /// </param>
        /// <param name="eventArguments">
        /// All of the various event level arguments
        /// </param>
        void MessageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs eventArguments)
        {
            try
            {
                // Interpret the incoming datagram's entire contents as a string.
                uint stringLength = eventArguments.GetDataReader().UnconsumedBufferLength;
                string receivedMessage = eventArguments.GetDataReader().ReadString(stringLength);
                receivedMessage = receivedMessage.Replace("\0", "");

                // transmit the data back to the main thread.
                NotifyUserFromAsyncThread(
                    "Received data from remote peer: \"" +
                    receivedMessage + "\"");
            }
            catch (Exception exception)
            {
                // Work out what the exception is. if it is a exception that is being
                // produced from what part of the system and return an appropriate exception.
                SocketErrorStatus socketError = SocketError.GetStatus(exception.HResult);
                if (socketError == SocketErrorStatus.ConnectionResetByPeer)
                {
                    // This error would indicate that a previous send operation resulted in an 
                    // ICMP "Port Unreachable" message.
                    // Notify the user about this error so they can fix it on there end
                    NotifyUserFromAsyncThread(
                        "Peer does not listen on the specific port. Please make sure that you run step 1 first " +
                        "or you have a server properly working on a remote server.");
                }
                else if (socketError != SocketErrorStatus.Unknown)
                {
                    // Notify the user about the strange error
                    NotifyUserFromAsyncThread(
                        "Error happened when receiving a datagram: " + socketError.ToString());
                }
                else
                {
                    // something unexpected happend throw send the exception to the method clalling this class.
                    throw;
                }
            }
        }

        /// <summary>
        /// Close the object and stop all ongoing streams.
        /// </summary>
        public void Close()
        {
            //close all of the persistant objects
            this.Server.Dispose();
            this.Writer.DetachStream();
            this.Writer.Dispose();
        }
    }
}
