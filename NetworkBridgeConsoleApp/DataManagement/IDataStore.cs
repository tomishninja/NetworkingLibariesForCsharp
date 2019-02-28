using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBridgeConsoleApp.DataManagement
{
    interface IDataStore<T>
    {
        bool TryGetNext(out T output);

        void Add(T dataEntry);
    }
}
