using Spaceproject.src;

namespace Spaceproject
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
        public CityName Name{ get; set; }
        public List<WeatherData> WeatherDatas{ get; set; }
        public CityWeather(CityName cityName,List<WeatherData> weatherDatas)
        {
            this.Name = cityName;   
            this.WeatherDatas = weatherDatas;
        }
    }
}