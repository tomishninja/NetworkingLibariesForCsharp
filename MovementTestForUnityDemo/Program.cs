using NetworkingLibaryStandard;
using System;
using System.Threading.Tasks;
using DataLibrary;

namespace MovementTestForUnityDemo
{
    class Program : IDisplayMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public const string JSONPreamble = "{\"Items\":[";

        /// <summary>
        /// 
        /// </summary>
        public const string JSONPostamble = "]}";

        static void Main(string[] args)
        {
            Program instance = new Program(true);

            /// The object that will send out the data
            UDPClient uDPClient = new UDPClient(instance);
            uDPClient.Start(22115);

            int millisecondsBetweenLoops = 2;

            instance.sendMessages(uDPClient, millisecondsBetweenLoops);

            Console.ReadKey();
        }

        private readonly bool useJson;

        public Program(bool useJson)
        {
            this.useJson = useJson;
        }

        public async void sendMessages(UDPClient uDPClient, int millisecondsBetweenLoops)
        {
            if (useJson)
            {
                DataLibrary.JSONObjectWrapper streamingData = new DataLibrary.JSONObjectWrapper();

                streamingData.Set(DataLibrary.JSONObjectWrapper.DataValues.PosX, 0.50f);
                streamingData.Set(DataLibrary.JSONObjectWrapper.DataValues.PosY, 0.50f);
                streamingData.Set(DataLibrary.JSONObjectWrapper.DataValues.PosZ, 0.50f);
                streamingData.Set(DataLibrary.JSONObjectWrapper.DataValues.HasChanged, 1);

                // send inital values
                uDPClient.Send(streamingData.GetJSON(true));
                Console.WriteLine(streamingData.GetJSON(true));

                while (true)
                {
                    // rise the obect up
                    for (float i = streamingData.Get(JSONObjectWrapper.DataValues.PosY); i < 1; i += 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosY, i);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    // lower the object all of the way
                    for (float i = streamingData.Get(JSONObjectWrapper.DataValues.PosY); i > 0; i -= 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosY, i);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    // rise the obect up
                    for (float i = streamingData.Get(JSONObjectWrapper.DataValues.PosY); i <= 0.5; i += 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosY, i);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }


                    for (float index = streamingData.Get(JSONObjectWrapper.DataValues.PosZ); index <= 1; index += 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosZ, index);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }
                    
                    for (float index = streamingData.Get(JSONObjectWrapper.DataValues.PosZ); index > 0; index -= 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosZ, index);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (float index = streamingData.Get(JSONObjectWrapper.DataValues.PosZ); index <= 0.5; index += 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosZ, index);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    float indexZ = streamingData.Get(JSONObjectWrapper.DataValues.PosZ);
                    float indexY = streamingData.Get(JSONObjectWrapper.DataValues.PosY);
                    for (; indexZ < 1 && indexY < 1; indexZ += 0.01f, indexY += 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosY, indexY);
                        streamingData.Set(JSONObjectWrapper.DataValues.PosZ, indexZ);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; indexZ > 0 && indexY > 0; indexZ -= 0.01f, indexY -= 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosY, indexY);
                        streamingData.Set(JSONObjectWrapper.DataValues.PosZ, indexZ);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }
                    
                    for (; indexZ < 0.5 && indexY < 0.5; indexZ += 0.01f, indexY += 0.01f)
                    {
                        streamingData.Set(JSONObjectWrapper.DataValues.PosY, indexY);
                        streamingData.Set(JSONObjectWrapper.DataValues.PosZ, indexZ);
                        uDPClient.Send(streamingData.GetJSON(true));
                        Console.WriteLine(streamingData.GetJSON(true));
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    // wait 3 seconds between interations
                    await Task.Delay(3000);
                }
            }
            else
            {
                UnityStreamingDataExample streamingData = new UnityStreamingDataExample();

                // set intial streaming values
                streamingData.X = 50;
                streamingData.Y = 50;
                streamingData.Z = 50;
                streamingData.HasChanged = true;

                // send inital values
                uDPClient.Send(streamingData.ToString());
                Console.WriteLine(streamingData.ToString());

                while (true)
                {
                    // rise the obect up
                    for (; streamingData.Y < 100; streamingData.Y++)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    // lower the object all of the way
                    for (; streamingData.Y > 0; streamingData.Y--)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    // rise the obect up
                    for (; streamingData.Y <= 50; streamingData.Y++)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; streamingData.Z < 99; streamingData.Z++)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; streamingData.Z > 0; streamingData.Z--)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; streamingData.Z <= 50; streamingData.Z++)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; streamingData.Z < 100 && streamingData.Y < 100; streamingData.Z++, streamingData.Y++)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; streamingData.Z > 0 && streamingData.Y > 0; streamingData.Z--, streamingData.Y--)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }

                    for (; streamingData.Z <= 50 && streamingData.Y <= 50; streamingData.Z++, streamingData.Y++)
                    {
                        streamingData.HasChanged = true;
                        uDPClient.Send(streamingData.ToString());
                        await Task.Delay(millisecondsBetweenLoops);
                    }
                }
            }
        }

        public void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            Console.WriteLine(message);
        }
    }
}
