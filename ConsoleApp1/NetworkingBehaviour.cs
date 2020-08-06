using System.Collections.Generic;
using NetworkingLibaryStandard;
using System.Text;
using System;
using ConsoleApp1;

public class NetworkingBehaviourHandler : IContinuousMessageHandeler
{
    public const string StartMessage = "Starting";

    private string noData = "<nodata></nodata>";

    private string outputString = StartMessage;

    private Queue<string> contentToSendBack = new Queue<string>();

    private object sem = new object();

    private StringBuilder sb = new StringBuilder();

    private bool shouldUpdate = false;

    private void Start()
    {
        noData = "<nodata><\nodata>";

    }

    private void Update()
    {
        /*
        if (shouldUpdate)
        {
            lock (sem)
            {
                // clear the string buffer from its last use
                sb.Clear();

                // if there is any thing to add left add it to the output string for the next message
                while (contentToSendBack.Count == 0)
                {
                    sb.Append(contentToSendBack.Dequeue());
                }

                // add this content to the string buffer
                outputString = sb.ToString();
            }
        }
        */
    }

    public void startMethods()
    {
        if (!shouldUpdate)
        {
            // make should update equal true
            shouldUpdate = true;

            // make update equal false
            //AvatarGameObject.DisplayContentFromNetwork();
            //MedicalGameObject.DisplayContentFromNetwork();
        }
    }

    public string RespondTo(string messageRecived)
    {
        if (noData.Equals(messageRecived))
        {
            Console.WriteLine("No Data In package");
        }
        else
        {
            foreach (string tagName in Tags.tageNames)
            {
                // Make a completed start tag of each of the tags
                string completedStart = Tags.MakeStartTag(tagName);

                // this just holds a string for later on
                string plyString;

                int start = messageRecived.IndexOf(completedStart, System.StringComparison.Ordinal);

                if (start > -1)
                {
                    int end = messageRecived.IndexOf(Tags.MakeEndTag(tagName), System.StringComparison.Ordinal);

                    int startPoint;

                    if (end > start)
                    {
                        switch (tagName)
                        {
                            case "nodata":
                                // the no data tag was reached this shoud be unreachable at this point
                                Console.WriteLine("no Data tag found should have been found earlier");

                                return noData;
                            case "jsondata":
                                // work out were the string starts after the XML tag
                                startPoint = start + completedStart.Length;
                                // get a substring starting from the new start point for 
                                // the length of the list and the length of the disired substring
                                string jsonString = messageRecived.Substring(startPoint, end - startPoint);

                                // output the json data as the form
                                try
                                {
                                    //AutomaticJSONFromMeVis jSONFromMeVis = JsonUtility.FromJson<AutomaticJSONFromMeVis>(jsonString);

                                    // send the data to the main thread
                                    //MainThread.JSONDataFromResponder = jSONFromMeVis;
                                    Console.WriteLine(jsonString);
                                }
                                catch (System.ArgumentException)
                                {
                                    Console.WriteLine("JSON String wasnt properly formated");
                                }

                                return noData;
                            case "medicalplyfile":
                                // Start to generate a mesh to the screen
                                startPoint = start + completedStart.Length;
                                plyString = messageRecived.Substring(startPoint, end - startPoint);
                                Console.WriteLine(plyString.Substring(0, 500));

                                // Start to generate the mesh that needs to built
                                //if (Manager != null)
                                //MedicalGameObject.GetMeshFilter().mesh = Manager.GenerateMesh(plyString);

                                return Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.RecivedData], tagName);
                            case "avatarplyfile":
                                // Start to generate a mesh to the screen
                                startPoint = start + completedStart.Length;
                                plyString = messageRecived.Substring(startPoint, end - startPoint);
                                Console.WriteLine(plyString.Substring(0, 500));
                                // Start to generate the mesh that needs to built
                                //if (Manager != null)
                                    //AvatarGameObject.GetMeshFilter().mesh = Manager.GenerateMesh(plyString);

                                return Tags.Encapsulate(Tags.tageNames[(int)Tags.indexs.RecivedData], tagName);
                            default:
                                // should be unreachable but if it is reached then find out why
                                //Console.WriteLine(tagName);
                                startPoint = start + completedStart.Length;
                                Console.WriteLine("Message for stuff: " + messageRecived.Substring(startPoint, end - startPoint));

                                return noData;
                        }
                    }
                }
            }
        }
        return noData;
    }

    private void SetMarkerInfo(string message)
    {
        //TODO format some marker detail to apply to this
        string markerDetail = "This fuction needs to be completed";

        contentToSendBack.Enqueue(Tags.Encapsulate("Markers", markerDetail));
    }
}
