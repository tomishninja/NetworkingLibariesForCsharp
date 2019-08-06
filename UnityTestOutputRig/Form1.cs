using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NetworkingLibaryStandard;

namespace UnityTestOutputRig
{
    public partial class Form1 : Form, IDisplayMessage
    {
        /// <summary>
        /// A setting for the maxiumum amount of items in the console
        /// </summary>
        public const int maxAmountOfConsoleItems = 10;

        /// <summary>
        /// Works in parrell to the flow layout manager allowing me to remove unwanted lables
        /// </summary>
        private Queue<Label> itemsInConsole = new Queue<Label>();

        /// <summary>
        /// 
        /// </summary>
        private Queue<string> itemsToAddTo = new Queue<string>();

        /// <summary>
        /// The lock for this class when its dealing with the streaming data
        /// </summary>
        private Object SyncroLock = new object();

        /// <summary>
        /// This object is just responcible for holding and transforming the data
        /// being sent via the stream.
        /// </summary>
        private UnityStreamingDataExample streamingData = new UnityStreamingDataExample();

        /// <summary>
        /// 
        /// </summary>
        private UDPClient uDPClient;

        /// <summary>
        /// 
        /// </summary>
        private enum Textbox
        {
            X,
            Y,
            Z
        }

        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            // just write a message to the console saying that this part of the class has been reached
            WriteToOuput("Initialized");

            // set up and start the uDPClient
            uDPClient = new UDPClient(this);
            uDPClient.Start();

            // set up a timer event to regulary send messages out via UDP
            Timer timer = new Timer();
            timer.Interval = 1000; // 1 sec
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            // send data that needs to be sent
            SendStreamingData();

            // update the console
            while (0 != itemsToAddTo.Count)
            {
                WriteToOuput(itemsToAddTo.Dequeue());
            }
        }

        /// <summary>
        /// Adds a lable to the console area of the main screen
        /// </summary>
        /// <param name="text"></param>
        private void WriteToOuput(string text)
        {
            // write the output to the GUI
            if(itemsInConsole.Count > maxAmountOfConsoleItems)
            {
                ConsoleBufferflowLayoutPanel.Controls.Remove(itemsInConsole.Dequeue());
            }

            // create the new lable then set its text to something appropriate
            Label newItem = new Label
            {
                Text = text
            };

            // add the new message to the output 
            ConsoleBufferflowLayoutPanel.Controls.Add(newItem);
            itemsInConsole.Enqueue(newItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            lock (SyncroLock)
            {
                this.streamingData.X = XtextBox.Text;
                this.streamingData.Y = YtextBox.Text;
                this.streamingData.Z = ZtextBox.Text;
                this.streamingData.HasChanged = true;
            }
        }

        /// <summary>
        /// Send data to another device via UDP
        /// </summary>
        public void SendStreamingData()
        {
            // update the content of the streaming data to be sent
            lock (SyncroLock)
            {
                uDPClient.Send(this.streamingData.ToString());
            }
        }

        /// <summary>
        /// This method is designed ot handle messages from other threads
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            itemsToAddTo.Enqueue(message);
        }
    }
}
