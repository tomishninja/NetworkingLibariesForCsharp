using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataLibrary;
using NetworkingLibaryStandard;

namespace WindowsFormsApp1
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
        /// The object that will send out the data
        /// </summary>
        private UDPClient uDPClient;

        /// <summary>
        /// The JSON wrapper object
        /// </summary>
        private PrototypeMarkers wrapper;

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
            uDPClient.Start(22115);

            // set up a timer event to regulary send messages out via UDP
            Timer timer = new Timer
            {
                Interval = 1000 // 1 sec
            };
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            // create a JSON wrapper object that will hold all of the data from the object
            wrapper = new PrototypeMarkers();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
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
            if (itemsInConsole.Count > maxAmountOfConsoleItems)
            {
                ConsoleBufferflowLayoutPanel.Controls.Remove(itemsInConsole.Dequeue());
            }

            // create the new lable then set its text to something appropriate
            Label newItem = new Label
            {
                Text = text
            };
            newItem.Width = 100;

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
                wrapper.Set(PrototypeMarkers.DataValues.HasChanged, 1);
                wrapper.Set(PrototypeMarkers.DataValues.MainPosX, float.Parse(XtextBox.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MainPosY, float.Parse(YtextBox.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MainPosZ, float.Parse(ZtextBox.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerOneX, float.Parse(textBoxItemOneX.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerOneY, float.Parse(textBoxItemOneY.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerOneZ, float.Parse(textBoxItemOneZ.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerTwoX, float.Parse(textBoxItemTwoX.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerTwoY, float.Parse(textBoxItemTwoY.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerTwoZ, float.Parse(textBoxItemTwoZ.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerThreeX, float.Parse(textBoxItemThreeX.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerThreeY, float.Parse(textBoxItemThreeY.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerThreeZ, float.Parse(textBoxItemThreeZ.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerFourX, float.Parse(textBoxItemFourX.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerFourY, float.Parse(textBoxItemFourY.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerFourZ, float.Parse(textBoxItemFourZ.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerFiveX, float.Parse(textBoxItemFiveX.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerFiveY, float.Parse(textBoxItemFiveY.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerFiveZ, float.Parse(textBoxItemFiveZ.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerSixX, float.Parse(textBoxItemSixX.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerSixY, float.Parse(textBoxItemSixY.Text));
                wrapper.Set(PrototypeMarkers.DataValues.MarkerSixZ, float.Parse(textBoxItemSixZ.Text));
                //TODO fix this section up so it dosn't wrap the code here but in the other object
                string json = wrapper.GetJSON(true);

                //string json = this.streamingData.ToJSON();
                uDPClient.Send(json);
                this.WriteToOuput(json);
                wrapper.Set(JSONObjectWrapper.DataValues.HasChanged, 0);
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
