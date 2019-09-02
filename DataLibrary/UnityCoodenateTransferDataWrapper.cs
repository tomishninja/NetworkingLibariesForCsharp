using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    class UnityCoodenateTransferDataWrapper
    {
        readonly CoodenateTransferData data;

        public new enum DataValues
        {

        }

        public UnityCoodenateTransferDataWrapper()
        { }

        /// <summary>
        /// Set a object within this objects data storage
        /// </summary>
        /// <param name="item">
        /// The data values that could be entered in to the system
        /// </param>
        /// <param name="value">
        /// The new value for the data value chosen
        /// </param>
        public void Set(DataValues item, float value)
        {
            this.data.values[(int)item] = value;
        }

        /// <summary>
        /// Set a object within this objects data storage
        /// </summary>
        /// <param name="index">
        /// The index of the item that needs to be set
        /// </param>
        /// <param name="value">
        /// The new value for the data value chosen
        /// </param>
        public void Set(int index, float value)
        {
            this.data.values[index] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public float Get(DataValues item)
        {
            return this.data.values[(int)item];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float Get(int index)
        {
            return this.data.values[index];
        }
    }
}
