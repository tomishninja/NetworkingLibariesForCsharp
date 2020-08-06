using DataLibrary;
using NetworkingLibaryStandard;

namespace DataTestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CoodenateTransferData data = new CoodenateTransferData(1,1);

            data.SetBedVector(1, 2, 3);
            data.SetMarkerData(0, "test", 1, 1, 1);
            data.SetNeedleInfo(0, "TEST", 2, 2, 2, 3, 3, 3);

            System.Console.WriteLine(data.ToUnityJSON());

            System.Console.ReadKey();

            AutomatedJSONTestWrapperVOne JSONTestData = new AutomatedJSONTestWrapperVOne(1, 2);

            Marker[] markers =
            {
                new Marker("A Marker", 12, 1, 2, 3, 4, 5, 6, 7),
                new Marker("An Marker", 10, 1, 2, 3, 4, 5, 6, 7),
                new Marker("Another Marker", 9, 1, 2, 3, 4, 5, 6, 7),
                new Marker("Ein Marker", 11, 1, 2, 3, 4, 5, 6, 7)
            };

            JSONTestData.AppendMarkers(new Marker("Eine Marker", 11, 1, 2, 3, 4, 5, 6, 7));
            JSONTestData.AppendMarkers(markers);


            System.Console.WriteLine(JSONTestData.ToJSON());

            string output = JSONTestData.ToJSON();

            UDPClient uDPClient = new UDPClient(22114);
            uDPClient.Start();
            uDPClient.Send(output);

            System.Console.ReadKey();

            uDPClient.Close();
        }
    }
}
