using Spaceproject;
using Spaceproject.src;
using SpaceProject;

try
{
    var list = new List<CityWeather>();
    list = DataReader.LoadDataFromFolder("/home/alex/c#/SpaceProject/input_example/");
  
    foreach(var city in list)
    {
        System.Console.WriteLine(city.Name);
    }
}
catch(Exception ex)
{
    System.Console.WriteLine(ex.Message + "" + ex.StackTrace);
}
