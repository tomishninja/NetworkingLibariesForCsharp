using System;
using System.IO;

namespace NetworkingLibrary
{
    public class TCPServer
    {
        /// <summary>
        /// Calls a object that is capable of outputing messages from this object
        /// to other threads on this system.
        /// </summary>
        private readonly IDisplayMessage messageService = null;

        /// <summary>
        /// This service holds the port number that this system will be running on.
        /// </summary>
        private readonly string portNumber = "0";

        /// <summary>
        /// this object will contain the method for how this class should respond
        /// </summary>
        private readonly ITCPResponder responderObject = null;

        /// <summary>
        /// 
        /// </summary>
        private Windows.Networking.Sockets.StreamSocketListener streamSocketListener = null;

        public TCPServer(string portNumber = NetworkingLibaryCore.DefaultServiceName, IDisplayMessage messageService = null, ITCPResponder responder = null)
        {
            this.portNumber = portNumber;
            this.messageService = messageService;
            this.responderObject = responder;
        }

        /// <summary>
        /// Start the server and collect and respond to any incoming streams
        /// </summary>
        public void Start()
        {
            try
            {
                streamSocketListener = new Windows.Networking.Sockets.StreamSocketListener();

                // The ConnectionReceived event is raised when connections are received.
                streamSocketListener.ConnectionReceived += this.ConnectionReceived;

                // Start listening for incoming TCP connections on the specified port. You can specify any port that's not currently in use.
                streamSocketListener.BindServiceNameAsync(this.portNumber);

                this.Output(MessageHelper.MessageType.Status, "server is listening on port \"" + this.portNumber + "\"");
            }
            catch (Exception ex)
            {
                // Lots of scary looking code here :/
                // all that it says below is if you don't know what is going on write to buffer
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.Output(MessageHelper.MessageType.Exception, webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        /// <summary>
        /// Send a message of a certain type to the another object that is capable of communicating 
        /// with other threads on the system. This funciton is usally built for presenting information
        /// </summary>
        /// <param name="type">
        /// Determines the type of action that should be taken for this message.
        /// How these types are handeled isn't of concern to this system so long
        /// as messages are commuinicated honestly
        /// </param>
        /// <param name="message">
        /// The sting messages that the system wants to display
        /// </param>
        private void Output(MessageHelper.MessageType type, string message)
        {
            if (messageService != null)
            {
                messageService.DisplayMessage(type, message);
            }
        }

        /// <summary>
        /// A listener object that responds when ever there has been a connection established
        /// </summary>
        /// <param name="sender">
        /// 
        /// </param>
        /// <param name="args">
        /// 
        /// </param>
        private async void ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string request;
            using (var streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }

            this.Output(MessageHelper.MessageType.Data, request);

            // if responder exists then respond else don't bother
            if (responderObject != null)
            {
                using (var streamWriter = new StreamWriter(args.Socket.OutputStream.AsStreamForWrite()))
                {
                    string responce = responderObject.Respond(ref request);

                    // call the responder object and send back the responce
                    streamWriter.WriteLine(responce);

                    // output the message to the helper if it exists
                    this.Output(MessageHelper.MessageType.Status, string.Format("server acted on the response: \"{0}\" appropiatly", responce));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
            this.streamSocketListener.Dispose();

            this.Output(MessageHelper.MessageType.Status, "server closed its socket");
        }

    }
}
