using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkingLibaryStandard
{
    /// <summary>
    /// This class contains various elements to help this libary 
    /// communicate to other parts of the system as it runs.
    /// </summary>
    public class MessageHelper
    {
        /// <summary>
        /// A value decribing different types of messages sent from this libary
        /// that could be used for general purpose.
        /// </summary>
        public enum MessageType { Data, Exception, Error };
    }
}
