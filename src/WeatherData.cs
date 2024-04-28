
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
        private static double _minTemperatureCriteria = 1;
        private static double _maxTemperatureCriteria = 32;
        private static double _windCriteria = 11;
        private static double _precipitationCriteria = 0;
        private static double _humidityCriteria = 55;
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
        public static double MinTemperatureCriteria
        {
            get { return _minTemperatureCriteria; }
            set 
            { 
                if(value < -20 || value > 60)
                    throw new ArgumentException("Invalid argument for " +
                                                $"minimal temperature criteria: {value}");
                if(value > _maxTemperatureCriteria)
                    throw new ArgumentException("Invalid argument for " +
                                                $"minimal temperature criteria: {value}" +
                                                "Minimal temperature cannot me higher than " +
                                                "the current maximal temperature " +
                                                $"({_maxTemperatureCriteria})");
                _minTemperatureCriteria = value; 
            }
        }
        public static double MaxTemperatureCriteria
        {
            get { return _maxTemperatureCriteria; }
            set 
            { 
                if(value < -20 || value > 60)
                    throw new ArgumentException("Invalid argument for " +
                                                $"maximal temperature criteria: {value}");
                if(value < _minTemperatureCriteria)
                    throw new ArgumentException("Invalid argument for " +
                                                $"maximal temperature criteria: {value}" +
                                                "Maximal temperature cannot me lower than " +
                                                "the current minimal temperature " +
                                                $"({_minTemperatureCriteria})");
                _maxTemperatureCriteria = value; 
            }
        }
        
        public static double WindCriteria
        {
            get { return _windCriteria; }
            set 
            { 
                if(value < 0 || value > 40)
                    throw new ArgumentException("Invalid argument for " +
                                                $"wind criteria speed in m/s: {value}");
                _windCriteria = value; 
            }
        }
        public static double HumidityCriteria
        {
            get { return _humidityCriteria; }
            set 
            {
                if(value < 0 || value > 100)
                    throw new ArgumentException("Invalid argument for " + 
                                                $"humidity criteria in %: {value}");
                _humidityCriteria = value; 
            }
        }
        public static double PrecipitationCriteria
        {
            get { return _precipitationCriteria; }
            set 
            { 
                if(value < 0 || value > 100)
                    throw new ArgumentException("Invalid argument for " + 
                                                $"precipitation in %: {value}");
                _precipitationCriteria = value;
            }
        }
        
        
        
        

        public override string ToString()
        {
            return $"Day {Day}: Temperature - {Temperature} degrees, Wind - {Wind} m/s, " +
            $"Humidity - {Humidity}%, Precipitation - {Precipitation}%, " + 
            $"Lighting - {Lighting}, Clouds - {Clouds}";
        }
    }
}