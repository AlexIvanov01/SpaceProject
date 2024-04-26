
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
        public CityName Name{ get; set; }
        public WeatherData[] WeatherDatas{ get; set; }
        public CityWeather(CityName cityName,WeatherData[] weatherDatas)
        {
            this.Name = cityName;   
            this.WeatherDatas = weatherDatas;
        }
    }
}