using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkingLibaryStandard
{
    interface IResponder
    {
        void Respond(string messageRecived, System.Net.Sockets.UdpClient client);
    }
}
