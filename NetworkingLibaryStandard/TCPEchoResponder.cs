using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;


namespace NetworkingLibaryStandard
{
    /// <summary>
    /// By Thomas Clarke
    /// 
    /// This class is a template for a echo responder build for 
    /// demonstration and testing purposes allowing this system to determine 
    /// </summary>
    public class TCPEchoResponder : ITCPResponder, IContinuousMessageHandeler
    {
        /// <summary>
        /// A basic contructor for this object that provi
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        public void Respond(NetworkStream stream, string data)
        {
            // outputs the data to console
            Console.WriteLine("listener: {0}", data);

            // convert the sting into a byte array
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

            // Send back a response.
            stream.Write(msg, 0, msg.Length);
        }

        /// <summary>
        /// Returns the same message it recived back to the other sytem
        /// </summary>
        /// <param name="messageRecived">Assumed to be a string representing the last systems communications</param>
        /// <returns>The same parameter as the message recived</returns>
        public string RespondTo(string messageRecived)
        {
            // outputs the data to the console
            Console.WriteLine("Client: {0}", messageRecived);

            //return the parameter sent back in 
            return messageRecived;
        }
    }
}
