using Spaceproject;
using Spaceproject.src;

namespace SpaceProject
{
    public static class WeatherDataReader
    {
        public static List<CityWeather> LoadData(string folderPath)
        {
            var list = new List<CityWeather>();
            
        }
        public static WeatherData[] ReadCSV(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            string type = "";
            WeatherData[] weatherData = new WeatherData[lines[0].Length];
            CloudCover cloud;
            int day;
            double temperature;
            double wind;
            double humidity;
            double percipitation;
            int lighting;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                for (int j = 0; j < values.Length; j++)
                {
                    if (j == 0) type = values[j].ToLower();
                    switch (type)
                    {
                        case "day":
                            if (!int.TryParse(values[j], out day))
                                throw new FormatException(
                                    "Cannot convert day to" +
                                    $"int type at row[{i + 1}], column[{j + 1}].");
                            weatherData[j].Day = day;
                            break;
                        case "temperature":
                            if (!double.TryParse(values[j], out temperature))
                                throw new FormatException(
                                    "Cannot convert temperature to" +
                                    $"double type at row[{i + 1}], column[{j + 1}].");
                            weatherData[j].Temperature = temperature;
                            break;
                        case "wind":
                            if (!double.TryParse(values[j], out wind))
                                throw new FormatException(
                                    "Cannot convert wind to " +
                                     $"double type at row[{i + 1}], column[{j + 1}].");
                            weatherData[j].Wind = wind;
                            break;
                        case "humidity":
                            if (!double.TryParse(values[j], out humidity))
                                throw new FormatException(
                                    "Cannot convert humidity to " +
                                     $"double type at row[{i + 1}], column[{j + 1}].");
                            weatherData[j].Humidity = humidity;
                            break;
                        case "precipitation":
                            if (!double.TryParse(values[j], out percipitation))
                                throw new FormatException(
                                    "Cannot convert percipitation to " +
                                     $"double type at row[{i + 1}], column[{j + 1}].");
                            weatherData[j].Precipitation = percipitation;
                            break;
                        case "lighting":
                            if (!int.TryParse(values[j], out lighting))
                                throw new FormatException(
                                    "Cannot convert lighting to " +
                                     $"int type at row[{i + 1}], column[{j + 1}].");
                            if (lighting == 0) weatherData[j].Lighting = false;
                            else weatherData[j].Lighting = true;
                            break;
                        case "clouds":
                            if (!Enum.TryParse(values[j], out cloud))
                                throw new FormatException(
                                    "Cannot convert to " +
                                    $"boolean type at row[{i + 1}], column[{j + 1}].");
                            weatherData[j].Clouds = cloud;
                            break;
                    }
                }
            }
            return weatherData;
        }
    }
}
