using ConsoleApp1;
using DataLibrary;
using NetworkingLibaryStandard;
using System;
using System.IO;
using System.Threading;

namespace MovementTCPDemoMidOct
{
    class Program
    {
        static void Main(string[] args)
        {
            // set up all the tools for sending the data packets
            HugeDataStucureNetworkingPackets largeDataVersion = new ConsoleApp1.HugeDataStucureNetworkingPackets();
            TCPClient client = new TCPClient(responder: largeDataVersion);
            client.Start(true, "Starting");

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

                // TODO add the data to the compiler
                largeDataVersion.AddData(Tags.tageNames[2], JSONTestData.ToString());
            }
        }

        static void NeverEndingTcpDemo()
        {
            string plyFileContent = File.ReadAllText("ExampleOutputOfOtherSystem.txt");
            string JSONFileContent = "World";//File.ReadAllText("C:/Users/z004344b/GitRepos/NetworkingLibariesForCsharp/ConsoleApp1/TestData/DemoPrototypeOutput.json");

            TCPEchoResponderForHugeAmountOfData respondingLogic = new TCPEchoResponderForHugeAmountOfData(plyFileContent);
            TCPEchoResponder echoResponder = new TCPEchoResponder();
            HugeDataStucureNetworkingPackets largeDataVersion = new ConsoleApp1.HugeDataStucureNetworkingPackets();

            largeDataVersion.AddData("PlyFile", plyFileContent);
            largeDataVersion.AddData(Tags.tageNames[2], JSONFileContent);

            //Program program = new Program();

            //TCPListener listener = new TCPListener(responder: largeDataVersion, output: program);
            TCPClient client = new TCPClient(responder: largeDataVersion);

            //listener.Start();

            client.Start(true, JSONFileContent);

            Thread.Sleep(400);
            largeDataVersion.AddData("PlyFile", JSONFileContent);

            Console.ReadKey();

            //Console.WriteLine(listener.data);

            Console.ReadKey();

            client.Close();
            //listener.Close();
        }
    }
}
