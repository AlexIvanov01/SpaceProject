
using System.Globalization;

namespace SpaceProject.src
{
    public static class DataReader
    {
        // Method that reads the csv file and saves it as string,
        // then tries to parse each value from the csv
        private static WeatherData[] ReadCSV(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            // Checks for valid csv formatting
            CheckCSV(lines, filePath);

            WeatherData[] weatherData =
            new WeatherData[lines[0].Split(',').Length - 1]; // == columns of csv without header column

            for (int i = 0; i < weatherData.Length; i++)
            {
                weatherData[i] = new WeatherData();
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(','); //  takes a row and separates all values

                for (int j = 1; j < values.Length; j++)
                {
                    int tempInt;
                    double tempDouble;
                    bool isParseFailure;
                    switch (values[0].ToLower().Replace("\"", "")) // Tries to parse value based on
                    {                                              // based on header column
                        case "day":
                            isParseFailure = !int.TryParse(values[j], out tempInt);
                            if (isParseFailure)
                            {
                                throw new FormatException(
                                    "Cannot convert day to" +
                                    "int type at " +
                                    $"row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Day = tempInt;
                            break;

                        case "temperature":
                            isParseFailure =
                            !double.TryParse(values[j], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out tempDouble); 

                            if (isParseFailure)
                            {
                                throw new FormatException(
                                    "Cannot convert temperature to " +
                                    "double type at " +
                                    $"row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Temperature = tempDouble;
                            break;

                        case "wind":
                            isParseFailure =
                            !double.TryParse(values[j], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out tempDouble);

                            if (isParseFailure)
                            {
                                throw new FormatException(
                                    "Cannot convert wind to " +
                                    "double type at " +
                                    $"row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Wind = tempDouble;
                            break;

                        case "humidity":
                            isParseFailure =
                            !double.TryParse(values[j], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out tempDouble);

                            if (isParseFailure)
                            {
                                throw new FormatException(
                                    "Cannot convert humidity to " +
                                    "double type at " +
                                    $"row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Humidity = tempDouble;
                            break;

                        case "precipitation":
                            isParseFailure =
                            !double.TryParse(values[j], NumberStyles.Float,
                            CultureInfo.InvariantCulture, out tempDouble);

                            if (isParseFailure)
                            {
                                throw new FormatException(
                                    "Cannot convert percipitation to " +
                                    "double type at " +
                                    $"row[{i + 1}], column[{j + 1}].");
                            }
                            weatherData[j - 1].Precipitation = tempDouble;
                            break;

                        case "lighting":
                            if (values[j].ToLower()
                                         .Replace("\"", "")
                                         .Equals("no"))

                                weatherData[j - 1].Lighting = false;

                            else if (values[j].ToLower()
                                              .Replace("\"", "")
                                              .Equals("yes"))

                                weatherData[j - 1].Lighting = true;
                            else
                            {
                                throw new FormatException(
                                "Cannot convert lighting to boolean type. " +
                                "Read falue is " +
                                $"{values[j].ToLower().Replace("\"", "")}" +
                                $"at row[{i + 1}], column[{j + 1}");
                            }
                            break;

                        case "clouds":
                            CloudCover tempCloud;
                            isParseFailure = !Enum.TryParse(values[j]
                                                  .Replace("\"", ""), out tempCloud);

                            if (isParseFailure)
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
                                "Read row type is " +
                                $"{values[0].ToLower().Replace("\"", "")}");
                    }
                }
            }
            return weatherData;
        }

        // Checks if csv has exactly 7 rows, checks if it has at most 15 columns with data,
        // checks if each header row is unique and checks if all 'day' values are unique.
        private static void CheckCSV(string[] lines, string filePath)
        {
            if (lines.Length != 7)
            {
                throw new FormatException(
                    $"Invalid file foramt for file {filePath}. " +
                    "Required number of valid rows is 7. " +
                    $"Read rows is {lines.Length}");
            }

            if (lines[0].Split(',').Length > 16)
            {
                throw new FormatException(
                    $"Invalid file foramt for file {filePath}. " +
                    "Maximum number of columns is 15. " +
                    $"Read number of columns is {lines[0].Split(',').Length}");
            }

            var firstValues = lines.Select(line => line.Split(',')[0]);
            bool isEachRowUnique = firstValues.Distinct().Count() == lines.Length;

            if (!isEachRowUnique)
            {
                throw new FormatException(
                    $"Invalid file foramt for file {filePath}. " +
                    "There are duplicate rows present in the file.");
            }

            // Find the row that starts with "day"
            string? dayRow = Array.Find(lines, line => line.ToLower()
                                                            .Replace("\"", "")
                                                            .StartsWith("day"));

            // Check if the day row exists and if all values in it are unique
            if (dayRow != null)
            {
                string[] values = dayRow.Split(',');
                bool isDayRowUnique = values.Distinct().Count() == values.Length;
                if (!isDayRowUnique)
                {
                    throw new FormatException(
                    $"Invalid file foramt for file {filePath}. " +
                    "There are duplicate days in the day row");
                }
            }
        }

        // Method that goes to a folder and reads from every city weather csv file
        // by using the ReadCSV method, which cheks for data validation while reading
        public static List<CityWeather> LoadDataFromFolder(string folderPath)
        {
            var list = new List<CityWeather>();

            string[] fileEntries =
            Directory.GetFiles(folderPath, "*WeatherData.csv");

            foreach (string filePath in fileEntries)
            {
                WeatherData[] cityWeatherData = ReadCSV(filePath);

                string cityNameString =
                Path.GetFileNameWithoutExtension(filePath)
                    .Replace("WeatherData", "");

                if (!Enum.TryParse(cityNameString, out CityName cityNameEnum))
                {
                    throw new FormatException(
                        "Cannot convert to " +
                        $"enum the value {cityNameString}.");
                }

                CityWeather currentCity = new(cityNameEnum, cityWeatherData);

                list.Add(currentCity);
            }
            return list;
        }
    }
}

