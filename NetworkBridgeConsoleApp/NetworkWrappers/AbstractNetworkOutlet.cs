using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBridgeConsoleApp.NetworkWrappers
{
    public class AbstractNetworkOutlet
    {
        public readonly DataManagement.IDataStore<string> DataManager = null;

        internal AbstractNetworkOutlet(DataManagement.IDataStore<string> dataManager)
        {
            this.DataManager = dataManager;
        }

        internal AbstractNetworkOutlet() { }
    }
}
