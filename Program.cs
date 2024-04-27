using SpaceProject.src;

try
{
    var list = new List<CityWeather>();
    list = DataReader.LoadDataFromFolder("/home/alex/c#/SpaceProject/input_example/");
    foreach(var city in list)
    {
        city.PrintData();
        string reason;
        var bestDay = city.FindBestDayForLaunch(out reason);
        if (bestDay != null) System.Console.WriteLine("\nBest day for launch:\n" +
                                  $"{bestDay}\n\nReason:\n{reason}");
        else System.Console.WriteLine("\nNo best day for launch for this city\n");
    }    
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
}
