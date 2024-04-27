
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
                return goodDays.First();
            }

            var OrderedByWindList =
            goodDays.OrderBy(day => day.Wind).ToArray();

            if (OrderedByWindList[0].Wind != OrderedByWindList[1].Wind)
            {
                leadCondition = "Day with the lowest wind speed. Humidity not accounted.";
                return OrderedByWindList[0];
            }

            var FirstByHumidityDay = OrderedByWindList.Where(
                day => day.Wind == OrderedByWindList.First().Wind).OrderBy(
                    day => day.Humidity).First();

            leadCondition = "Day with the lowest wind speed and humidity.";
            return FirstByHumidityDay;
        }
    }
}