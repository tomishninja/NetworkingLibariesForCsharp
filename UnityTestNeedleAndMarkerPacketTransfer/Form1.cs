using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataLibrary;
using NetworkingLibaryStandard;

namespace UnityTestNeedleAndMarkerPacketTransfer
{
    public partial class Form1 : Form, IDisplayMessage
    {
        /// <summary>
        /// A setting for the maxiumum amount of items in the console
        /// </summary>
        public const int maxAmountOfConsoleItems = 12;

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
        private CoodenateTransferData wrapper;

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
            wrapper = new CoodenateTransferData(11, 1);
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
                wrapper.SetBedVector(float.Parse(XtextBox.Text), float.Parse(YtextBox.Text), float.Parse(ZtextBox.Text));

                wrapper.SetMarkerData(0, textBoxItemOneName.Text, float.Parse(textBoxItemOneX.Text), float.Parse(textBoxItemOneY.Text), float.Parse(textBoxItemOneZ.Text));
                wrapper.SetMarkerData(1, textBoxItemTwoName.Text, float.Parse(textBoxItemTwoX.Text), float.Parse(textBoxItemTwoY.Text), float.Parse(textBoxItemTwoZ.Text));
                wrapper.SetMarkerData(2, textBoxItemThreeName.Text,  float.Parse(textBoxItemThreeX.Text), float.Parse(textBoxItemThreeY.Text), float.Parse(textBoxItemThreeZ.Text));
                wrapper.SetMarkerData(3, textBoxItemFourName.Text, float.Parse(textBoxItemFourX.Text), float.Parse(textBoxItemFourY.Text), float.Parse(textBoxItemFourZ.Text));
                wrapper.SetMarkerData(4, textBoxItemFiveName.Text, float.Parse(textBoxItemFiveX.Text), float.Parse(textBoxItemFiveY.Text), float.Parse(textBoxItemFiveZ.Text));
                wrapper.SetMarkerData(5, textBoxItemSixName.Text, float.Parse(textBoxItemSixX.Text), float.Parse(textBoxItemSixY.Text), float.Parse(textBoxItemSixZ.Text));
                wrapper.SetMarkerData(6, textBoxItemSevenName.Text, float.Parse(textBoxItemSevenX.Text), float.Parse(textBoxItemSevenY.Text), float.Parse(textBoxItemSevenZ.Text));
                wrapper.SetMarkerData(7, textBoxItemEightName.Text, float.Parse(textBoxItemEightX.Text), float.Parse(textBoxItemEightY.Text), float.Parse(textBoxItemEightZ.Text));
                wrapper.SetMarkerData(8, textBoxItemNineName.Text, float.Parse(textBoxItemNineX.Text), float.Parse(textBoxItemNineY.Text), float.Parse(textBoxItemNineZ.Text));
                wrapper.SetMarkerData(9, textBoxItemTenName.Text, float.Parse(textBoxItemTenX.Text), float.Parse(textBoxItemTenY.Text), float.Parse(textBoxItemTenZ.Text));
                wrapper.SetMarkerData(10, textBoxItemElevenName.Text, float.Parse(textBoxItemElevenX.Text), float.Parse(textBoxItemElevenY.Text), float.Parse(textBoxItemElevenZ.Text));
                
                wrapper.SetNeedleInfo(0, textBoxNeedleOneName.Text, float.Parse(textBoxNeedleOneAX.Text), float.Parse(textBoxNeedleOneAY.Text), float.Parse(textBoxNeedleOneAZ.Text),
                    float.Parse(textBoxNeedleOneXB.Text), float.Parse(textBoxNeedleOneBY.Text), float.Parse(textBoxNeedleOneBZ.Text));

                string json = wrapper.ToUnityJSON();
                
                uDPClient.Send(json);
                this.WriteToOuput(json);
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
