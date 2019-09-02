namespace DataTestingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DataLibrary.CoodenateTransferData data = new DataLibrary.CoodenateTransferData(1,1);

            data.SetBedVector(1, 2, 3);
            data.SetMarkerData(0, "test", 1, 1, 1);
            data.SetNeedleInfo(0, "TEST", 2, 2, 2, 3, 3, 3);

            System.Console.WriteLine(data.ToUnityJSON());

            System.Console.ReadKey();
        }
    }
}
