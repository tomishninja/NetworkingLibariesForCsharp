using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NetworkingLibaryStandard;

namespace ConsoleApp1
{
    class UnityWorkFlowExample : ITCPResponder
    {
        public enum state
        {
            NotStarted = 0,
            Started = 1,
            SentAllGraphicalData = 2,
            SendingMarkerData = 3,
            Waiting = 4
        }

        private readonly KeyValuePair<string, string>[] TypesOfTags;

        private readonly string NoDataTag = "";

        private state currentState = state.NotStarted;

        private readonly string avatarPly = "";
        private bool avatarHasBeenSent = false;

        private readonly string medicalInfoPly = "";
        private bool medicalInfoHasBeenSent = false;

        private string positioningAndMarkerData = "";

        private Queue<KeyValuePair<Tags.indexs, string>> MessagesRecived;

        public UnityWorkFlowExample(string avatarPly, string medicalInfoPly, string positioningAndMarkerData)
        {
            // set up the content to be sent
            this.avatarPly = Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.AvatarPly], avatarPly);
            this.medicalInfoPly = Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.MedicalPly], medicalInfoPly);
            this.positioningAndMarkerData = positioningAndMarkerData;

            // set up some perment veribles
            NoDataTag = Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.NoData], "");

            // build all of the recoring encapulation tags for the values
            TypesOfTags = new KeyValuePair<string, string>[Tags.tageNames.Length];
            for (int index = 0; index < TypesOfTags.Length; index++)
            {
                TypesOfTags[index] = new KeyValuePair<string, string>(
                    Tags.MakeStartTag(Tags.tageNames[index]),
                    Tags.MakeEndTag(Tags.tageNames[index]));
            }

            // set up the message recived queue
            MessagesRecived = new Queue<KeyValuePair<Tags.indexs, string>>();
        }

        public void Respond(NetworkStream stream, string data)
        {
            // veribles
            byte[] msg; // the byte array that will be sent to unity
            int start;
            int end;
            // clear out the old messages
            MessagesRecived.Clear();

            // make sure the no data page is as expected

            // return a responce
            switch (currentState)
            {
                case state.NotStarted:
                    // Regardless of what the message is disregard it and start sending the new one
                    currentState = state.Started;
                    Console.WriteLine("Started");

                    // convert the string into a byte array
                    msg = System.Text.Encoding.ASCII.GetBytes(medicalInfoPly);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);

                    // mark medical info as sent
                    medicalInfoHasBeenSent = true;

                    break;
                case state.Started:
                    // The only message that is important at this stage is that the data has been 
                    // transfered and the other PC is ready to accept the other data.
                    // once this pc has recived the first set of data that should have been noted
                    // in the Not started section

                    // if no data is recived then don't send anything back
                    if (NoDataTag.Equals(data))
                    {
                        // if no data is requested then return no data back
                        msg = System.Text.Encoding.ASCII.GetBytes(NoDataTag);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);

                        break;
                    }

                    // keep looping though all the messages recived

                    // get the start tag
                    start = data.IndexOf(TypesOfTags[(int)Tags.indexs.RecivedData].Key, System.StringComparison.Ordinal);
                    if (start > -1)
                    {
                        // get the end tag
                        end = data.IndexOf(TypesOfTags[(int)Tags.indexs.RecivedData].Value, System.StringComparison.Ordinal);

                        if (end > start)
                        {
                            // Collect this data to use later in this method when we know what stage were in

                            // work out were the content starts
                            int startPoint = start + TypesOfTags[(int)Tags.indexs.RecivedData].Key.Length;

                            string message = data.Substring(startPoint, end - startPoint);

                            // process this data for the next stream
                            if (message.Equals(Tags.tageNames[(int)Tags.indexs.AvatarPly]))
                            {
                                // If the avatar was previouly sent send over the avatar information

                                // convert the sting into a byte array
                                msg = System.Text.Encoding.ASCII.GetBytes(medicalInfoPly);

                                // Send back a response.
                                stream.Write(msg, 0, msg.Length);

                                // mark medical info as sent
                                medicalInfoHasBeenSent = true;
                            }
                            else if (message.Equals(Tags.tageNames[(int)Tags.indexs.MedicalPly]))
                            {
                                // if the medical data was already sent send over the avatar data

                                // convert the sting into a byte array
                                msg = System.Text.Encoding.ASCII.GetBytes(avatarPly);

                                // Send back a response.
                                stream.Write(msg, 0, msg.Length);

                                // mark avartar as sent
                                avatarHasBeenSent = true;
                            }

                            // once all of the data that needs to be sent has been sent change this systems state over to waiting
                            if (avatarHasBeenSent && medicalInfoHasBeenSent)
                            {
                                currentState = state.SentAllGraphicalData;
                                Console.WriteLine("All graphical data sent");
                            }


                        }
                    }
                    break;
                case state.SentAllGraphicalData:
                    // in this state all the system wants to do is wait until the other system asks for marker data to come though

                    // if no data is recived then don't send anything back
                    if (NoDataTag.Equals(data))
                    {
                        // if no data is requested then return no data back
                        msg = System.Text.Encoding.ASCII.GetBytes(NoDataTag);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);

                        break;
                    }

                    // if the system recives anything other than than a start tag then start.
                    start = data.IndexOf(TypesOfTags[(int)Tags.indexs.ChangedState].Key, System.StringComparison.Ordinal);
                    if (start > -1)
                    {
                        // get the end tag
                        end = data.IndexOf(TypesOfTags[(int)Tags.indexs.ChangedState].Value, System.StringComparison.Ordinal);

                        if (end > start)
                        {
                            // change the current state 
                            currentState = state.SendingMarkerData;
                            Console.WriteLine("Sending MarkerData");

                            // send some marker data
                            msg = System.Text.Encoding.ASCII.GetBytes(Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.JSONData], positioningAndMarkerData));

                            // send off the first message
                            stream.Write(msg, 0, msg.Length);
                        }
                    }
                    else
                    {
                        // The assumption is made that the other system isn't asking for anything

                        // if no data is requested then return no data back
                        msg = System.Text.Encoding.ASCII.GetBytes(NoDataTag);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                    }
                    break;
                case state.SendingMarkerData:
                    // TODO eventually you going to want to recive some 
                    // data here so there is some two way communication

                    // TODO work eventually this would need to be updated
                    msg = System.Text.Encoding.ASCII.GetBytes(Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.JSONData], positioningAndMarkerData));

                    // send off the first message
                    stream.Write(msg, 0, msg.Length);

                    break;
                case state.Waiting:
                    // TODO work out exactly what this should do.
                    break;
            }
        }
    }
}
