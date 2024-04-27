
namespace SpaceProject.src
{
    public enum CityName
    {
        Kourou,
        Tanegashima,
        CapeCanaveral,
        Kodiak,
        Mahia,
    }
    public class CityWeather
    {
        public CityName Name { get; set; }
        public WeatherData[] WeatherDatas { get; set; }
        public WeatherData? BestDayForLaunch;
        public CityWeather(CityName cityName, WeatherData[] weatherDatas)
        {
            this.Name = cityName;
            this.WeatherDatas = weatherDatas;
        }

        public void PrintData()
        {

            System.Console.WriteLine($"\n{Name} weather data:\n");
            foreach (var day in WeatherDatas)
            {
                System.Console.WriteLine(day);
            }
        }

        public WeatherData? FindBestDayForLaunch(out string leadCondition)
        {
            List<WeatherData> goodDays = new List<WeatherData>();
            foreach (var day in WeatherDatas)
            {
                if (day.Precipitation != 0 ||
                    day.Lighting == true ||
                    day.Humidity >= 55 ||
                    day.Wind > 11 ||
                    day.Clouds == CloudCover.Cumulus ||
                    day.Clouds == CloudCover.Nimbus ||
                    day.Temperature < 1 ||
                    day.Temperature > 32) continue;

                goodDays.Add(day);
            }

            if (goodDays.Count == 0)
            {
                leadCondition = string.Empty;
                return null;
            }
            if (goodDays.Count == 1)
            {
                leadCondition = "Only day that fits conditions.";
                BestDayForLaunch = goodDays.First();
                return goodDays.First();
            }

            var OrderedByWindArray =
            goodDays.OrderBy(day => day.Wind).ToArray();

            if (OrderedByWindArray[0].Wind != OrderedByWindArray[1].Wind)
            {
                leadCondition = "Day with the lowest wind speed. " + 
                                "Humidity not accounted.";
                BestDayForLaunch = OrderedByWindArray[0];
                return OrderedByWindArray[0];
            }

            var FirstByHumidityDay = OrderedByWindArray.Where(
                day => day.Wind == OrderedByWindArray.First().Wind).OrderBy(
                    day => day.Humidity).First();

            leadCondition = "Day with the lowest wind speed and humidity.";
            BestDayForLaunch = FirstByHumidityDay;
            return FirstByHumidityDay;
        }

        public static CityWeather? FindBestLocation(List<CityWeather> cities, out string leadCondition)
        {
            foreach (CityWeather cityWeather in cities)
            {
                if (cityWeather.BestDayForLaunch == null)
                    cities.Remove(cityWeather);
            }
            if (cities.Count == 0)
            {
                leadCondition = string.Empty;
                return null;
            }
            #pragma warning disable CS8602 // Dereference of a possibly null reference.

            var OrderedByWindArray =
                cities.OrderBy(city => city.BestDayForLaunch.Wind).ToArray();

            if (OrderedByWindArray.Length == 1)
            {
                leadCondition = $"Location {OrderedByWindArray[0].Name} " +
                                "has thee lowest wind speed on day " + 
                                $"{OrderedByWindArray[0].BestDayForLaunch.Day}. " +
                                "Humidity not accounted.";
                return OrderedByWindArray[0];
            }

            if (OrderedByWindArray[0].BestDayForLaunch.Wind != 
                OrderedByWindArray[1].BestDayForLaunch.Wind)
            {
                leadCondition = $"Location {OrderedByWindArray[0].Name} " +
                                "has the lowest wind speed on day " + 
                                $"{OrderedByWindArray[0].BestDayForLaunch.Day}. " +
                                "Humidity not accounted.";
                return OrderedByWindArray[0];
            }

            var OrderedByHumidityArray = OrderedByWindArray.Where(
                                         city => city.BestDayForLaunch.Wind == 
                                         OrderedByWindArray.First().BestDayForLaunch.Wind)
                                         .OrderBy(city => 
                                         city.BestDayForLaunch.Humidity).ToArray();

            if (OrderedByHumidityArray[0].BestDayForLaunch.Humidity != 
                OrderedByHumidityArray[1].BestDayForLaunch.Humidity)
            {
                leadCondition = $"Location {OrderedByHumidityArray[0].Name} has " +
                                "the lowest wind speed and humidity on day " +
                                $"{OrderedByHumidityArray[0].BestDayForLaunch.Day}.";
                return OrderedByHumidityArray[0];
            }

            var FirstByClosestToEquator = OrderedByHumidityArray.Where(
                                city => city.BestDayForLaunch.Humidity ==
                                OrderedByHumidityArray.First().BestDayForLaunch.Humidity)
                                .OrderBy(city => city.Name).First();

            leadCondition = $"Location {FirstByClosestToEquator.Name} has " +
                            "the lowest wind speed and humidity on day " +
                            $"{OrderedByHumidityArray[0].BestDayForLaunch.Day} " +
                            "and is closest to the Equator.";
            return FirstByClosestToEquator;

            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}