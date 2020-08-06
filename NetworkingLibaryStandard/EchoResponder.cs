using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// The Echo Reponder Class is one of the most basic implentations of the
    /// I Responder Class. Its default behaviour will be to Echo back a responce
    /// when ever Respond is called by calling the Message Back function. However 
    /// Extending this class will give you access to modifiy the behaviour of the 
    /// respond method while still keeping a easy method send the message back with.
    /// </summary>
    public class EchoResponder : IResponder
    {
        /// <summary>
        /// Creates a Echo Responder object that can used as a IResponder Object
        /// This class by default will send a message back to the recipiant of choice.
        /// </summary>
        public EchoResponder() { }

        /// <summary>
        /// echos back teh message recived from the sender
        /// </summary>
        /// <param name="messageRecived"></param>
        /// <param name="client"></param>
        /// <param name="endpoint"></param>
        public virtual void Respond(string messageRecived, UdpClient client, ref IPEndPoint endpoint)
        {
            MessageBack(messageRecived, client, ref endpoint);
        }

        /// <summary>
        /// sends a message back to the client in qustion
        /// </summary>
        /// <param name="message">
        /// 
        /// </param>
        /// <param name="client">
        /// 
        /// </param>
        public void MessageBack(string message, UdpClient client, ref IPEndPoint endpoint)
        {
            // convert the data into bytes
            byte[] data = Encoding.Unicode.GetBytes(message);
            client.SendAsync(data, data.Length, endpoint);
        }
    }
}
