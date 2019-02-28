using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBridgeConsoleApp.DataManagement
{
    class SyncronisedQueueDataStore<T> : IDataStore<T>
    {
        Queue DataStore = null;

        /// <summary>
        /// This represents the a value that is not acceptable.
        /// </summary>
        public readonly T InvalidMarker;

        public SyncronisedQueueDataStore(T invalidMarker)
        {
            DataStore = Queue.Synchronized(new Queue());

            InvalidMarker = invalidMarker; 
        }

        public void Add(T dataEntry)
        {
            DataStore.Enqueue(dataEntry);
        }

        public bool TryGetNext(out T output)
        {
            // the the data store has no more members left inform the entity
            // getting data that they need to find more
            if (DataStore.Count == 0)
            {
                output = InvalidMarker;
                return false;
            }
            else
            {
                // get the next item off the queue
                object value = DataStore.Dequeue();

                // make sure the object isn't returning null if it is return false instead
                if (value != null)
                {
                    try
                    {
                        // if it works return as expected
                        output = (T)value;
                        return true;
                    }
                    catch(Exception ex)
                    {
                        // if there are any errors 
                        // return fasle and move on
                        output = InvalidMarker;
                        return false;
                    }
                }
                else
                {
                    // entry isn't valid move forward
                    output = InvalidMarker;
                    return false;
                }
            }
        }
    }
}
