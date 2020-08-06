namespace NetworkingLibaryStandard
{
    public interface ITCPResponder
    {
        /// <summary>
        /// This method is designed to be inherited and should not be called 
        /// by the user as it is not desinged for it.
        /// 
        /// This method is designed to respond to a message from the client.
        /// by default this method will do nothing.
        /// </summary>
        /// <param name="stream">
        /// The network stream object recived from the client sending the data
        /// </param>
        /// <param name="msg">
        /// the bytes that created the message
        /// </param>
        /// <param name="data">
        /// A string represention of the message reviced
        /// </param>
        void Respond(System.Net.Sockets.NetworkStream stream, string data);
    }
}
