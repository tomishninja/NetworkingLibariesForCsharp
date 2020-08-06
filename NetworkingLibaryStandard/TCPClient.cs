using System;
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
        /// A behaviour class for reply to messages from this object
        /// </summary>
        private IContinuousMessageHandeler Responder = null;

        /// <summary>
        /// This boolean determine weather or not the sytem is running and
        /// listening to client threads.
        /// </summary>
        private bool isListening = false;

        /// <summary>
        /// Tells the other thread when things are done 
        /// </summary>
        private bool listeningHasFinished = true;

        /// <summary>
        /// The following thread allows controls the continuious sending and listing of data
        /// </summary>
        private Thread continuiousSenderAndListenerThread = null;

        /// <summary>
        /// Default constuctor this will set the Port Number 
        /// and the Server Host Name to the default ones 
        /// previouly listed.
        /// </summary>
        public TCPClient()
        {
            this.PortNumber = NetworkingLibaryStandard.DefaultPortNumber;
            this.ServerHostName = NetworkingLibaryStandard.LocalHostString;
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
        /// <param>
        /// 
        /// </param>
        public TCPClient(int portNumber = NetworkingLibaryStandard.DefaultPortNumber, string hostName = NetworkingLibaryStandard.LocalHostString, IContinuousMessageHandeler responder = null)
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

            this.Responder = responder;
        }

        /// <summary>
        /// Constuctor for this object keeps the 
        /// </summary>
        /// <param name="hostName">
        /// A string primtive that represents the host's name
        /// This will either be an ipAddress (IP4 tested) or the
        /// string "localhost" will also work.
        /// </param>
        public TCPClient(string hostName, int portNumber = NetworkingLibaryStandard.DefaultPortNumber, IContinuousMessageHandeler responder = null)
        {
            // set the values
            this.PortNumber = portNumber;
            this.ServerHostName = hostName;
            Responder = responder;
        }

        /// <summary>
        /// Starts the network stream if none currently exists
        /// </summary>
        public void Start(bool continuious = false, string firstMessage = null, bool wait = false)
        {
            // if the client does not exist create it or else do nothing. 
            if (this.client == null && !wait)
                this.client = new TcpClient(this.ServerHostName, this.PortNumber);
            else if (wait)
                this.client = waitForClient();

            // check the values and start the expected behaviour acordingly
            if (continuious == true && firstMessage == null)
            {
                // Don't let people send a null for a message
                throw new ArgumentException("The first message sent may not be null");
            }
            else if (continuious == true)
            {
                // start the listener thread running a continuious read and write between this and the other machine
                continuiousSenderAndListenerThread = new Thread(() => SendAndListenContinuously(firstMessage));
                continuiousSenderAndListenerThread.Start();
            }
            else if (firstMessage != null)
            {
                // just send a single message
                this.Send(firstMessage);
            }
            // if the message is null don't do anything because there was no message to send to the other device
        }

        public TcpClient waitForClient()
        {
            // loop until a listener on this port and server becomes avliable
            do
            {
                try
                {
                    return new TcpClient(this.ServerHostName, this.PortNumber);
                }
                catch (SocketException)
                {
                    // just keep going if it is this type of exception stop for others because something is wrong with another part of the system if they trigger
                    Thread.Sleep(400);
                }
            } while (true);
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
            // if the client is null we don't want to do anything thoughout this method
            if (this.client != null)
            {
                // Try to send this data while being aware that there can be lots of exceptions
                try
                {
                    // convert the string into a byte array
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    // get the network stream
                    NetworkStream stream = client.GetStream();

                    // and send the message to the other device on this stream
                    stream.Write(data, 0, data.Length);
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
        /// 
        /// </summary>
        /// <param name="firstMessage"></param>
        public void SendAndListenContinuously(string firstMessage)
        {
            // TODO: work out if the responder part of this "if" is nessarcary, you could just keep sending the first message in some stituations
            if (this.client != null && Responder != null)
            {
                // a byte array to hold the messages to be sent
                Byte[] bytes = new byte[256];

                // this verible will hold the next message to send to the other client
                string msg = firstMessage;

                // since were about to start sending a information to another client we need to place data here
                isListening = true;

                // 
                listeningHasFinished = false;

                // if any network errors happen display some info on why and break out of this thread 
                try
                {
                    // until another thread tells this thread to stop working
                    while (isListening)
                    {
                        // convert the current message into bypes
                        bytes = System.Text.Encoding.ASCII.GetBytes(msg);

                        // Get the network stream
                        NetworkStream stream = client.GetStream();

                        // send the string data
                        stream.Write(bytes, 0, bytes.Length);

                        // clean up the data value
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        // loop though all of the data recived by the client while there is still more data to read
                        do
                        {
                            // Note that this function is blocking work out how much data we can read it one sweep
                            int numberOfBytesRead = stream.Read(bytes, 0, bytes.Length);
                            // then append this data to a string builder to that we can read later
                            sb.Append(System.Text.Encoding.ASCII.GetString(bytes, 0, numberOfBytesRead));
                        } while (stream.DataAvailable);

                        // convert the string that was built into the data string
                        // get the next message to send ready for use for the next itteration
                        if (Responder != null)
                            msg = Responder.RespondTo(sb.ToString());
                    }
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine("Client disconconnected");
                }
                listeningHasFinished = true;
            }
            else
            {
                // let the user know that this program isn't quite complete in its current state because of something they managed to pull off
                throw new ArgumentException("Send and listen continiously was entered in the incorrect state. " +
                    "This system shoudl be started and have a responder class during  its initizization. " +
                    "One of these conditions should have been met before starting this program.");
            }
        }

        /// <summary>
        /// Stop the listening thread. This will not disconnect the system. 
        /// Expect issues if you call this method
        /// </summary>
        public void StopListening()
        {
            isListening = false;
        }

        /// <summary>
        /// returns the result when regarding the state of listening. 
        /// This is used to tell other threads if the connection is still running
        /// </summary>
        /// <returns>Bool true if this app is no longer listening or else it will be true</returns>
        public bool ListeningHasFinished()
        {
            return listeningHasFinished;
        }

        /// <summary>
        /// Stops the client from running saving memory
        /// </summary>
        public void Close()
        {
            this.StopListening();

            // the below loop will give the program approximately 1 second or less to naturaly come to an end until this program will close off the access to the client.
            int counter = 0;
            while (!listeningHasFinished && counter < 60)
            {
                counter++;
                Thread.Sleep(32);
            }

            this.client.Close();
            this.client = null;
        }
    }
}
