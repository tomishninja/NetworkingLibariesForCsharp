using System;
using System.IO;
using System.Threading.Tasks;

namespace NetworkingLibrary
{
    public class TCPClient
    {
        /// <summary>
        /// Calls a object that is capable of outputing messages from this object
        /// to other threads on this system.
        /// </summary>
        private readonly IDisplayMessage messageService = null;

        /// <summary>
        /// Calls a objects made by the user that will create a message 
        /// for the user to work with a continious 
        /// </summary>
        private readonly ITCPResponder responder = null;

        /// <summary>
        /// This service holds the port number that this system will be running on.
        /// </summary>
        private readonly string portNumber = "0";

        /// <summary>
        /// The host name object for the os
        /// </summary>
        private readonly Windows.Networking.HostName hostName = null;

        /// <summary>
        /// The stream socket for this libary
        /// </summary>
        private Windows.Networking.Sockets.StreamSocket streamSocket = null;

        /// <summary>
        /// Used for the continuious listener version
        /// fucntion. This value is desinged to be set by another 
        /// thread to end the current listener. 
        /// </summary>
        private bool IsListening = false;

        /// <summary>
        /// Creates a TCP client object for use, 
        /// </summary>
        /// <param name="hostName">
        /// 
        /// </param>
        /// <param name="portNumber">
        /// 
        /// </param>
        /// <param name="messageService">
        /// 
        /// </param>
        /// <param name="responder">
        /// 
        /// </param>
        public TCPClient(string hostName = NetworkingLibaryCore.LocalHostName, string portNumber = NetworkingLibaryCore.DefaultServiceName, IDisplayMessage messageService = null, ITCPResponder responder = null)
        {
            this.portNumber = portNumber;
            this.hostName = new Windows.Networking.HostName(hostName);
            this.messageService = messageService;
            this.responder = responder;
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task ReciveResponce()
        {
            if (streamSocket != null)
            {
                string response;
                using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                {
                    using (StreamReader streamReader = new StreamReader(inputStream))
                    {
                        response = await streamReader.ReadLineAsync();
                    }
                }


                //this.Output(MessageHelper.MessageType.Status, string.Format("client received the response: \"{0}\" ", response));
                this.Output(MessageHelper.MessageType.Data, response);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendInternal(string message)
        {
            // Send a request
            using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
            {
                // 
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(message);
                    await streamWriter.FlushAsync();
                }
            }

            //
            this.Output(MessageHelper.MessageType.Status, string.Format("client sent the request: \"{0}\"", message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendContinuiously(string message)
        {
            // this will temperarly hold the responce string
            string responce; // may be

            // set the is listening value to true while this is going on
            IsListening = true;


            try
            {
                // this will keep running this fuction until another thead stops it
                while (IsListening)
                {
                    // Create the StreamSocket and establish a connection to the echo server.
                    streamSocket = new Windows.Networking.Sockets.StreamSocket();

                    // The server hostname that we will be establishing a connection to. In this example, the server and client are in the same process.
                    await streamSocket.ConnectAsync(hostName, this.portNumber);

                    // send the message
                    using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
                    {
                        // 
                        using (var streamWriter = new StreamWriter(outputStream))
                        {
                            await streamWriter.WriteLineAsync(message);
                            await streamWriter.FlushAsync();
                        }
                    }

                    // wait for the message
                    using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                    {
                        using (StreamReader streamReader = new StreamReader(inputStream))
                        {
                            responce = await streamReader.ReadLineAsync();
                        }
                    }

                    message = this.responder.Respond(ref responce);
                }

            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.Output(MessageHelper.MessageType.Exception, webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        /// <summary>
        /// Stops sending messages to the other device
        /// </summary>
        public void StopSendingContinuiously()
        {
            IsListening = false;
            this.Close();
        }

        /// <summary>
        /// Just sends a single message to another device
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task Send(string message)
        {
            try
            {
                // Create the StreamSocket and establish a connection to the echo server.
                streamSocket = new Windows.Networking.Sockets.StreamSocket();
                // The server hostname that we will be establishing a connection to. In this example, the server and client are in the same process.

                await streamSocket.ConnectAsync(hostName, this.portNumber);

                // send the message
                await SendInternal(message);

                this.Close();
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.Output(MessageHelper.MessageType.Exception, webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        public async Task SendAndWait(string message)
        {
            try
            {
                // Create the StreamSocket and establish a connection to the echo server.
                streamSocket = new Windows.Networking.Sockets.StreamSocket();
                // The server hostname that we will be establishing a connection to. In this example, the server and client are in the same process.

                await streamSocket.ConnectAsync(hostName, this.portNumber);

                // send the message
                await SendInternal(message);

                // wait for the message
                await ReciveResponce();

                this.Close();
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.Output(MessageHelper.MessageType.Exception, webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        private void Output(MessageHelper.MessageType type, string message)
        {
            if (this.messageService != null)
            {
                this.messageService.DisplayMessage(type, message);
            }
        }

        private void Close()
        {
            // get rid of the stream socket object and set it back to null
            // so the system can start again
            this.streamSocket.Dispose();
            this.streamSocket = null;

            this.Output(MessageHelper.MessageType.Status, "client closed its socket");
        }
    }
}
