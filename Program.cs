using System.Collections;
using System.Security;
using SpaceProject.src;

System.Console.WriteLine("\n\nTo use the app, please provide a path to a " +
    "folder, containing all *WeatherData.csv files, where * \nis the name of " +
    "a valid city location for launch. Please make sure the files are in " +
    "the correct format. \nThe app will read the files and then " +
    "output the most \nsuitable day for launch for each location given the criteria.\nYou may choose to use custom or " +
    "default criteria.\nIf there are suitable days for launch, data can be send to an email adress.\n\n");


while (true)
{
    try
    {
        System.Console.WriteLine("\n\nFull path to Folder containing all valid csv files: ");
        string filePath = string.Empty;
        filePath = System.Console.ReadLine() ??
         throw new ArgumentException("Error. No folder path provided.");

        System.Console.WriteLine("\nUse default criteria for filering best day and location for launch? (y/n): ");

        string choice = System.Console.ReadLine() ?? "y";
        switch (choice.ToLower())
        {
            case "n":
                System.Console.WriteLine("Please provide valid criteria for the following:\n");

                System.Console.WriteLine("Maximal temperature: ");
                WeatherData.MaxTemperatureCriteria = Convert.ToDouble(System.Console.ReadLine() ??
                throw new ArgumentException("Error. No cirteria provided."));

                System.Console.WriteLine("Minimal temperature: ");
                WeatherData.MinTemperatureCriteria = Convert.ToDouble(System.Console.ReadLine() ??
                throw new ArgumentException("Error. No cirteria provided."));

                System.Console.WriteLine("Maximal wind speed in m/s: ");
                WeatherData.WindCriteria = Convert.ToDouble(System.Console.ReadLine() ??
                throw new ArgumentException("Error. No cirteria provided."));

                System.Console.WriteLine("Maximal humidity in %:");
                WeatherData.HumidityCriteria = Convert.ToDouble(System.Console.ReadLine() ??
                throw new ArgumentException("Error. No cirteria provided."));

                System.Console.WriteLine("Maximal precipitation in %: ");
                WeatherData.PrecipitationCriteria = Convert.ToDouble(System.Console.ReadLine() ??
                throw new ArgumentException("Error. No cirteria provided."));

                break;
            case "y":
                break;
            default:
                throw new ArgumentException("Invalid choice.\n");
        }

        var cityList = new List<CityWeather>();
        cityList = DataReader.LoadDataFromFolder(filePath);
        string reason;

        foreach (var city in cityList)
        {
            city.PrintData();
            var bestDay = city.FindBestDayForLaunch(out reason);
            if (bestDay != null)
            {
                System.Console.WriteLine($"\nBest day for launch for {city.Name} " +
                $"is day {bestDay.Day}.\nReason: {reason}\n");
            }
            else
                System.Console.WriteLine($"\nCity {city.Name} has no " +
                "suitable day for launch in the given period.\n");
        }

        var bestCityLocation = CityWeather.FindBestLocation(cityList, out reason);

        if (bestCityLocation != null)
            System.Console.WriteLine($"\nBest location for launch is " +
            $"{bestCityLocation.Name}.\nReason: {reason}\n");
        else System.Console.WriteLine("No suitable location for the given period.\n");

        CityWeather.ExportBestDateToCSV(cityList, out string exportFilePath);

        if (bestCityLocation != null)
        {
            System.Console.WriteLine("Do you want to send data to mail with outlook account? (y/n): ");
            string mailChoice = System.Console.ReadLine() ?? "y";
            switch (mailChoice.ToLower())
            {
                case "y":
                    CityWeather.SendDataToMail(cityList, bestCityLocation, exportFilePath);
                    break;
                default:
                    break;
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
    }
}
