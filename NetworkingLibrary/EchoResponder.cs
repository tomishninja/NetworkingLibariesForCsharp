using System;
using System.IO;
using Windows.Networking.Sockets;

namespace NetworkingLibrary
{
    /// <summary>
    /// This object will just provide the logic to send back a 
    /// responce back to the system that sent this one
    /// </summary>
    public class EchoResponder : IUDPResponder, ITCPResponder
    {
        public async void Respond(string MessageRecived, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // Echo the request back as the response.
            using (Stream outputStream = args.Socket.OutputStream.AsStreamForWrite())
            {
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(MessageRecived);
                    await streamWriter.FlushAsync();
                }
            }
        }

        public string Respond(ref string data)
        {
            return data;
        }
    }
}
