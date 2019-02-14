using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace NetworkingLibaryStandard
{
    public class UDPListener
    {
        IPEndPoint EndPoint = null;
        readonly System.Net.Sockets.UdpClient EndpointClient = null;

        bool IsConnected = false;

        IDisplayMessage messageSystem = null;

        private static object ResponceLock = new object();

        public UDPListener()
        {
            EndPoint = new IPEndPoint(IPAddress.Any, 0);

            this.EndpointClient = new System.Net.Sockets.UdpClient(NetworkingLibary.DefaultPortNumber);
        }

        public UDPListener(IDisplayMessage messageSystem)
        {
            this.messageSystem = messageSystem;

            this.EndpointClient = new System.Net.Sockets.UdpClient(NetworkingLibary.DefaultPortNumber);
        }

        public void Start()
        {
            // start a listening thread for the object
            IsConnected = true;
            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        private void Listen()
        {
            try
            {
                while (this.IsConnected)
                {
                    Byte[] receiveBytes = this.EndpointClient.Receive(ref this.EndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    // if there is a message outlet send the data there
                    if (messageSystem != null)
                    {
                        messageSystem.DisplayMessage(MessageHelper.MessageType.Data, returnData);
                    }

                    // an overiable method 
                    //Responce(returnData, ref this.EndPoint);
                }
            }
            catch (Exception ex)
            {
                if (messageSystem != null)
                {
                    this.messageSystem.DisplayMessage(MessageHelper.MessageType.Exception, "Exception: " + ex.Message);
                }
            }
        }

        void Responce(string dataRecived, ref IPEndPoint endpoint)
        {
            lock (ResponceLock)
            {
                byte[] data = Encoding.Unicode.GetBytes(dataRecived);
                this.EndpointClient.SendAsync(data, data.Length);
            }
            
        }

        public void Stop()
        {
            this.IsConnected = false;

            if(EndpointClient != null)
            {
                this.EndpointClient.Close();
            }
            
        }
    }
}
