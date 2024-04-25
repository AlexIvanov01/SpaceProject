namespace Spaceproject.src
{
    public enum CloudCover
    {   
        Cumulus,
        Stratus,
        Stratocumulus,
        Cumulonimbus,
        Nimbus,
        Altocumulus,
        Altostratus,
        Cirrus,
        Cirrostratus,
        Cirrocumulus
    }
    public class WeatherData
    {
        public int Day { get; set; }
        public double Temperature { get; set; }
        public double Wind { get; set; }
        public double Humidity { get; set; }
        public double Precipitation { get; set; }
        public bool Lighting { get; set; }
        public CloudCover Clouds { get; set; } 
    }
}