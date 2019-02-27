using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LSLFramework
{
    public abstract class AbstractLSLObject
    {
        /// <summary>
        /// The default amount of times between loops for the devices
        /// </summary>
        public const int DefaultDelay = 1000;

        /// <summary>
        /// 
        /// </summary>
        internal Thread LSLthread = null;

        /// <summary>
        /// 
        /// </summary>
        internal readonly int delayInMiliseconds = 0;

        /// <summary>
        /// 
        /// </summary>
        internal bool keepThreadRunning = false;

        /// <summary>
        /// 
        /// </summary>
        internal IDisplayMessage messageSystem = null;
        
        /// <summary>
        /// 
        /// </summary>
        public const int defaultArrayLength = 0;

        /// <summary>
        /// 
        /// </summary>
        internal int arrayLength = defaultArrayLength;

        public AbstractLSLObject()
        {
            delayInMiliseconds = DefaultDelay;
        }

        public AbstractLSLObject(IDisplayMessage messageSystem)
        {
            delayInMiliseconds = DefaultDelay;
            this.messageSystem = messageSystem;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            // tell the main loop not to stop
            keepThreadRunning = true;

            // run the method that will keep the main loop runing
            LSLthread = new Thread(RunLSL);
            LSLthread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            keepThreadRunning = false;
        }

        /// <summary>
        /// 
        /// </summary>
        internal abstract void RunLSL();
    }
}
