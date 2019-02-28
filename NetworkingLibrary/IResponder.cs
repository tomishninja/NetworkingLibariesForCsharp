namespace NetworkingLibrary
{
    /// <summary>
    /// This interface allows classes in the Networking libary to
    /// send various interactions. More still needs to be done with this class
    /// before its intergartion
    /// </summary>
    public interface IResponder
    {
        /// <summary>
        /// This method will be called to determine various 
        /// responces that can be called vai the network server.
        /// </summary>
        /// <param name="MessageRecived">
        /// The data recived from the message recived.
        /// </param>
        /// <param name="args">
        /// the arguments from the event that called this responder
        /// </param>
        void Respond(string MessageRecived, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args);
    }
}
