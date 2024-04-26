using SpaceProject.src;

try
{
    var list = new List<CityWeather>();
    list = DataReader.LoadDataFromFolder("/home/alex/c#/SpaceProject/input_example/");

    foreach (var city in list)
    {
        Console.WriteLine(city.Name);
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
}
