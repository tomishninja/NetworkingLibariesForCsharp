using DataLibrary;
using NetworkingLibaryStandard;
using System;

namespace MovementTestingAppMidSep
{
    class Program : IDisplayMessage
    {
        static void Main(string[] args)
        {
            bool useUDP = true;

            Marker[] markers =
            {
                new Marker("Crown", 1, -0.981f, -0.005f, 1.569f, 0, 0, 0, 0),
                new Marker("Neck", 2, -0.785f, 0.013f, 1.653f, 255, 0, 0, 0.5f),
                new Marker("Sholder_Left", 3, -0.749f, 0.155f, 1.703f, 0, 255, 0, 0.5f),
                new Marker("Shoulder_Right", 4, -0.749f, -0.183f, 1.708f, 127, 127, 127, 0),
                new Marker("Groin", 5, -0.08f, -0.029f, 1.85f, 127, 127, 127, 255),
                new Marker("Hip_Left", 6, -0.316f, 0.119f, 1.822f, 255, 255, 255, 0.5f),
                new Marker("Hip_Right", 7, -0.325f, -0.16f, 1.817f, 0, 0, 255, 0f),
                new Marker("Knee_Right", 8, 0.259f, -0.17f, 1.814f, 255, 0, 255, 0.5f),
                new Marker("Knee_Left", 9, 0.268f, 0.089f, 1.817f, 255, 255, 255, 0.5f),
                new Marker("Ankel_Left", 10, 0.643f, 0.074f, 2.018f, 0, 255, 255, 0.5f),
                new Marker("Ankel_Right", 11, 0.632f, -0.165f, 2.028f, 255, 127, 0, 0.5f)
            };

            int MarkerToRemove = 0;
            int MarkerToChangeColorOf = 3;
            Vector4 colorToChangeTo = markers[2].color;

            AutomatedJSONTestWrapperVOne JSONTestData = new AutomatedJSONTestWrapperVOne(0.0f, 0.4f);
            
            JSONTestData.AppendMarkers(markers);
            System.Console.WriteLine(JSONTestData.ToJSON());

            string output = JSONTestData.ToJSON();

            TCPClient tCPClient = new TCPClient(22110, NetworkingLibaryStandard.NetworkingLibaryStandard.LocalHostString);
            UDPClient uDPClient = new UDPClient(22114);
            
            if (useUDP)
            {
                uDPClient.Start();
                uDPClient.Send(output);
            }
            else
            {
                tCPClient.Start();
                tCPClient.Send(output);
                tCPClient.Close();
            }

            JSONTestData.RemoveMarkers(markers[MarkerToRemove]);

            bool GoIn = true;
            bool GoUp = true;
            while (true)
            {
                if (GoIn)
                {
                    JSONTestData.TableHorizontal += 0.01f;
                    if (JSONTestData.TableHorizontal >= 2.40f)
                    {
                        GoIn = false;
                        JSONTestData.AppendMarkers(markers[MarkerToRemove++]);
                        Vector4 temp = JSONTestData.Markers[MarkerToChangeColorOf].color;
                        JSONTestData.Markers[MarkerToChangeColorOf].color = colorToChangeTo;

                        if (MarkerToRemove >= markers.Length)
                        {
                            MarkerToRemove = 0;
                        }
                        if (MarkerToChangeColorOf >= markers.Length)
                        {
                            MarkerToChangeColorOf = 0;
                        }
                        JSONTestData.RemoveMarkers(markers[MarkerToRemove]);
                        colorToChangeTo = temp;
                    }
                }
                else
                {
                    JSONTestData.TableHorizontal -= 0.01f;
                    if (JSONTestData.TableHorizontal <= 0.0f)
                    {
                        GoIn = true;

                        Vector4 temp = JSONTestData.Markers[MarkerToChangeColorOf].color;
                        JSONTestData.AppendMarkers(markers[MarkerToRemove++]);
                        JSONTestData.Markers[MarkerToChangeColorOf].color = colorToChangeTo;

                        if (MarkerToRemove >= markers.Length)
                        {
                            MarkerToRemove = 0;
                        }
                        JSONTestData.RemoveMarkers(markers[MarkerToRemove]);
                        colorToChangeTo = temp;
                    }
                }

                if (GoUp)
                {
                    JSONTestData.TableVertical += 0.01f;
                    if (JSONTestData.TableVertical >= 1.20f)
                    {
                        GoUp = false;
                        JSONTestData.AppendMarkers(markers[MarkerToRemove]);

                        MarkerToRemove++;
                        if (MarkerToRemove >= markers.Length)
                        {
                            MarkerToRemove = 0;
                        }
                        JSONTestData.RemoveMarkers(markers[MarkerToRemove]);
                    }
                }
                else
                {
                    JSONTestData.TableVertical -= 0.01f;
                    if (JSONTestData.TableVertical <= 0.40)
                    {
                        GoUp = true;
                        JSONTestData.AppendMarkers(markers[MarkerToRemove]);

                        MarkerToRemove++;
                        if (MarkerToRemove >= markers.Length)
                        {
                            MarkerToRemove = 0;
                        }
                        JSONTestData.RemoveMarkers(markers[MarkerToRemove]);
                    }
                }


                if (useUDP)
                {
                    uDPClient.Send(JSONTestData.ToJSON());
                }
                else
                {
                    tCPClient.Start();

                    tCPClient.Send(JSONTestData.ToJSON());

                    tCPClient.Close();
                }
                
            }
        }

        public void DisplayMessage(MessageHelper.MessageType type, string message)
        {
            Console.WriteLine(message);
        }
    }
}
