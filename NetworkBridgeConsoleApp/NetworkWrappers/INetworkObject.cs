using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBridgeConsoleApp.NetworkWrappers
{
    public interface INetworkObject
    {
        void Start();

        void Stop();
    }
}
