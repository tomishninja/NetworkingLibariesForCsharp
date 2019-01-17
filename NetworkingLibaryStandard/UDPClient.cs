using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetworkingLibaryStandard
{
    public class UDPClient
    {
        private readonly int portNumber = 0;
        private readonly string hostAddress = null;
        private UdpClient Client = null;
        private bool Connected = false;

        public UDPClient()
        {
            this.portNumber = Class1.DefaultPortNumber;
            this.hostAddress = Class1.LocalHostString;
        }

        public UDPClient(int portNumber)
        {
            this.portNumber = portNumber;
            this.hostAddress = Class1.LocalHostString;
        }

        public UDPClient(string hostAddress)
        {
            this.portNumber = Class1.DefaultPortNumber;
            this.hostAddress = hostAddress;
        }

        public void Start()
        {
            // if the main object hasn't been made yet make it
            if (this.Client == null)
            {
                Client = new UdpClient(this.portNumber+1);
            }

            // if the program hasn't already started start it
            if (!Connected)
            {
                Client.Connect(this.hostAddress, this.portNumber);
                this.Connected = true;
            }
        }

        public void Send(string message)
        {
            // transfer the string message into a byte message
            Byte[] packageContent = Encoding.ASCII.GetBytes(message);

            //
            this.Client.Send(packageContent, packageContent.Length);
        }
        
        public void Disconnect()
        {
            this.Client.Close();
        }
    }
}
