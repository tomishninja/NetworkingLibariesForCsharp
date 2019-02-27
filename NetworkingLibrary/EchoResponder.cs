using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace NetworkingLibrary
{
    /// <summary>
    /// This object will just provide the logic to send back a 
    /// responce back to the system that sent this one
    /// </summary>
    public class EchoResponder : IResponder
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
    }
}
