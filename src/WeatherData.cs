
namespace SpaceProject.src
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
        Cirrocumulus,
        Default
    }
    public class WeatherData
    {
        private int _day = 0;
        private double _temperature = 0;
        private double _wind = 0;
        private double _humidity = 0;
        private double _precipitation = 0;
        public bool Lighting { get; set; } = false;
        public CloudCover Clouds { get; set; } = CloudCover.Default;
        public int Day
        {
            get { return _day; }
            set 
            {
                if(value < 1 || value > 31)
                    throw new ArgumentException("Invalid argument for " + 
                                                $"day: {value}");
                 _day = value;
            }
        }
        public double Temperature
        {
            get { return _temperature; }
            set 
            { 
                if(value < -20 || value > 60)
                    throw new ArgumentException("Invalid argument for " +
                                                $"temperature: {value}");
                _temperature = value; 
            }
        }
        public double Wind
        {
            get { return _wind; }
            set 
            { 
                if(value < 0 || value > 40)
                    throw new ArgumentException("Invalid argument for " +
                                                $"wind speed in m/s: {value}");
                _wind = value; 
            }
        }
        public double Humidity
        {
            get { return _humidity; }
            set 
            {
                if(value < 0 || value > 100)
                    throw new ArgumentException("Invalid argument for " + 
                                                $"humidity in %: {value}");
                _humidity = value; 
            }
        }
        public double Precipitation
        {
            get { return _precipitation; }
            set 
            { 
                if(value < 0 || value > 100)
                    throw new ArgumentException("Invalid argument for " + 
                                                $"precipitation in %: {value}");
                _precipitation = value;
            }
        }
    }
}