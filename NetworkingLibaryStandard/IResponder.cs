using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// An optional interface that will enable the severs to
    /// Respond back to thier respective clients. This object
    /// must be placed inoto the servers constuctor inorder to
    /// be functional.
    /// </summary>
    interface IResponder
    {
        /// <summary>
        /// Allows for responces to be sent back to the client in a mannor that the application deems fit
        /// </summary>
        /// <param name="messageRecived">
        /// The messaged the server recived from the client. As a string primitive.
        /// </param>
        /// <param name="client">
        /// This is the client that originally sent the message
        /// </param>
        /// <param name="endpoint">
        /// a object holding the port number and host name of the other device
        /// </param>
        void Respond(string messageRecived, System.Net.Sockets.UdpClient client, ref IPEndPoint endpoint);
    }
}
