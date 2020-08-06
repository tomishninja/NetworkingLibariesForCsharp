namespace NetworkingLibaryStandard
{
    /// <summary>
    /// The interface require for the TCP client to mainatin a constant
    /// connection of messages to another system. 
    /// </summary>
    public interface IContinuousMessageHandeler
    {
        /// <summary>
        /// Depending on the input recived this method should be activated withing the TCP client
        /// when reciving commications from another device. This method will be called displaying what
        /// the user can se as well as giving the other side a automated responce. 
        /// </summary>
        /// <param name="messageRecived">
        /// The message as a string from the forigin device
        /// </param>
        /// <returns>
        /// The message from the other device
        /// </returns>
        string RespondTo(string messageRecived);
    }
}
