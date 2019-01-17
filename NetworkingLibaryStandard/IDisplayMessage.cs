using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkingLibaryStandard
{
    public interface IDisplayMessage
    {
        void DisplayMessage(MessageHelper.MessageType type, string message);
    }
}
