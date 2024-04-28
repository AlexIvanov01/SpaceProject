using SpaceProject.src;

try
{
    var list = new List<CityWeather>();
    list = DataReader.LoadDataFromFolder("/home/alex/c#/SpaceProject/input_example/");
    string reason;
    var bestList = new List<CityWeather>();
    foreach(var city in list)
    {
        city.PrintData();
        var bestDay = city.FindBestDayForLaunch(out reason);
        if (bestDay != null) bestList.Add(city);
    }    
    var bestCity = CityWeather.FindBestLocation(bestList, out reason);
    if(bestCity != null)
    System.Console.WriteLine($"\nBest location for launch is {bestCity.Name}.\nReason: {reason}\n");
    else System.Console.WriteLine("No suitable location for the given period.");
    CityWeather.ExportBestDateToCSV(bestList, out string filePath);
    if(bestCity != null)
    CityWeather.SendDataToMail(bestList, bestCity, filePath, "alex_ivanov075@outlook.com", "Ai0889876!", "alexi01@abv.bg");
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
}
