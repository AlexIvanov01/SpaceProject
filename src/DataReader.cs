using System.Globalization;
using Spaceproject;
using Spaceproject.src;

namespace SpaceProject
{
    public static class DataReader
    {
        public static List<CityWeather>? LoadDataFromFolder(string folderPath)
        {
            try
            {
                var list = new List<CityWeather>();
                string[] fileEntries = Directory.GetFiles(folderPath, "*WeatherData.csv");

                foreach (string filePath in fileEntries)
                {
                    WeatherData[] cityWeatherData = ReadCSV(filePath);
                    string cityNameString = Path.GetFileNameWithoutExtension(filePath).Replace("WeatherData", "");

                    if (!Enum.TryParse(cityNameString, out CityName cityNameEnum))
                    {
                        throw new FormatException(
                            "Cannot convert to " +
                            $"enum the value {cityNameString}.");
                    }
                    CityWeather currentCity = new CityWeather(cityNameEnum, cityWeatherData);
                    list.Add(currentCity);
                }
                return list;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message + "" + ex.StackTrace);
                return null;
            }
        }

        public static WeatherData[] ReadCSV(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            CheckCSV(lines, filePath);

            WeatherData[] weatherData = new WeatherData[lines[0].Split(',').Length - 1];
            for (int i = 0; i < weatherData.Length; i++)
            {
                weatherData[i] = new WeatherData();
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');

                for (int j = 1; j < values.Length; j++)
                {
                    int tempInt;
                    double tempDouble;
                    switch (values[0].ToLower().Replace("\"", ""))
                    {
                        case "day":
                            if (!int.TryParse(values[j], out tempInt))
                            {
                                throw new FormatException(
                                    "Cannot convert day to" +
                                    $"int type at row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Day = tempInt;
                            break;

                        case "temperature":
                            if (!double.TryParse(values[j], NumberStyles.Float,
                                CultureInfo.InvariantCulture, out tempDouble))
                            {
                                throw new FormatException(
                                    "Cannot convert temperature to " +
                                    $"double type at row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Temperature = tempDouble;
                            break;

                        case "wind":
                            if (!double.TryParse(values[j], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out tempDouble))
                            {
                                throw new FormatException(
                                    "Cannot convert wind to " +
                                    $"double type at row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Wind = tempDouble;
                            break;

                        case "humidity":
                            if (!double.TryParse(values[j], NumberStyles.Float,
                             CultureInfo.InvariantCulture, out tempDouble))
                            {
                                throw new FormatException(
                                    "Cannot convert humidity to " +
                                    $"double type at row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Humidity = tempDouble;
                            break;

                        case "precipitation":
                            if (!double.TryParse(values[j], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out tempDouble))
                            {
                                throw new FormatException(
                                    "Cannot convert percipitation to " +
                                    $"double type at row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Precipitation = tempDouble;
                            break;

                        case "lighting":
                            if (values[j].ToLower().Replace("\"", "").Equals("no"))
                                weatherData[j - 1].Lighting = false;
                            else if (values[j].ToLower().Replace("\"", "").Equals("yes"))
                                weatherData[j - 1].Lighting = true;
                            else
                            {
                                throw new FormatException(
                                "Cannot convert lighting to boolean type. " +
                                $"Read falue is {values[j].ToLower().Replace("\"", "")}" +
                                $"at row[{i + 1}], column[{j + 1}");
                            }
                            break;

                        case "clouds":
                            CloudCover tempCloud;
                            if (!Enum.TryParse(values[j].Replace("\"", ""), out tempCloud))
                            {
                                throw new FormatException(
                                    "Cannot convert to " +
                                    $"enum type at row[{i + 1}], column[{j + 1}]." +
                                    $"Read value was {values[j].Replace("\"", "")}");
                            }
                            weatherData[j - 1].Clouds = tempCloud;
                            break;

                        default:
                            throw new FormatException(
                                $"Invalid file foramt for {filePath} " +
                                $"An invalid row is present at row {i + 1} " +
                                $"Read row type is {values[0].ToLower().Replace("\"", "")}");
                    }
                }
            }
            return weatherData;
        }

        private static void CheckCSV(string[] lines, string filePath)
        {
            if (lines.Length != 7)
            {
                throw new FormatException(
                    $"Invalid file foramt for file {filePath}." +
                    "Required number of valid rows is 7. " +
                    $"Read rows is {lines.Length}");
            }

            if (lines[0].Split(',').Length > 16)
            {
                throw new FormatException(
                    $"Invalid file foramt for file {filePath}." +
                    "Maximum number of columns is 15. " +
                    $"Read number of columns is {lines[0].Split(',').Length}");
            }
        }
    }
}