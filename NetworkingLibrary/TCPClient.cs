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

        public TCPClient()
        {
            this.portNumber = NetworkingLibaryCore.DefaultServiceName;
            this.hostName = new Windows.Networking.HostName(NetworkingLibaryCore.LocalHostName);
        }

        public TCPClient(IDisplayMessage messageService)
        {
            this.portNumber = NetworkingLibaryCore.DefaultServiceName;
            this.hostName = new Windows.Networking.HostName(NetworkingLibaryCore.LocalHostName);
            this.messageService = messageService;
        }

        public TCPClient(string portNumber)
        {
            this.portNumber = portNumber;
            this.hostName = new Windows.Networking.HostName(NetworkingLibaryCore.LocalHostName);
        }

        public TCPClient(string portNumber, IDisplayMessage messageService)
        {
            this.portNumber = portNumber;
            this.messageService = messageService;
            this.hostName = new Windows.Networking.HostName(NetworkingLibaryCore.LocalHostName);
        }

        public TCPClient(string portNumber, string hostname)
        {
            this.portNumber = portNumber;
            this.hostName = new Windows.Networking.HostName(NetworkingLibaryCore.LocalHostName);
        }

        public TCPClient(string portNumber, string hostName, IDisplayMessage messageService)
        {
            this.portNumber = portNumber;
            this.messageService = messageService;
            this.hostName = new Windows.Networking.HostName(NetworkingLibaryCore.LocalHostName);
        }

        public TCPClient(Windows.Networking.HostName hostname)
        {
            this.portNumber = NetworkingLibaryCore.DefaultServiceName;
            this.hostName = hostname;
        }

        public TCPClient(Windows.Networking.HostName hostname, IDisplayMessage messageService)
        {
            this.portNumber = NetworkingLibaryCore.DefaultServiceName;
            this.messageService = messageService;
            this.hostName = hostname;
        }

        public TCPClient(int portNumber, string hostname, IDisplayMessage messageService)
        {
            this.portNumber = portNumber.ToString();
            this.messageService = messageService;
            this.hostName = new Windows.Networking.HostName(hostname);
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

                this.Output(MessageHelper.MessageType.Status, string.Format("client received the response: \"{0}\" ", response));
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
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(message);
                    await streamWriter.FlushAsync();
                }
            }

            //
            this.Output(MessageHelper.MessageType.Status, string.Format("client sent the request: \"{0}\"", message));
        }

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
