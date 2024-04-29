using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text.RegularExpressions;
using ConsoleTables;

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
            var table = new ConsoleTable("Day", "Temperature [°C]", "Wind [m/s]",
                         "Humidity [%]", "Precipitation [%]", "Lighting", "Clouds");

            foreach (var day in WeatherDatas)
            {
                table.AddRow(day.Day, day.Temperature, day.Wind, day.Humidity,
                 day.Precipitation, day.Lighting, day.Clouds);
            }

            System.Console.WriteLine($"\n{Name} weather data:\n");
            table.Write();
        }

        // Method that takes the list of WeatherData and checks by the set criteria
        // if the day is suitable for launch, if more than 1 is suitable,
        // orders the suitable days by wind speed, and if 2 or more have 
        // the same wind speed to the lowest value, orders the WeatherData by humidity
        // and retunrs the first element.
        public WeatherData? FindBestDayForLaunch(out string leadCondition)
        {
            List<WeatherData> goodDays = new List<WeatherData>();
            foreach (var day in WeatherDatas)
            {
                if (day.Precipitation > WeatherData.PrecipitationCriteria ||
                    day.Lighting ||
                    day.Humidity >= WeatherData.HumidityCriteria ||
                    day.Wind > WeatherData.WindCriteria ||
                    day.Clouds == CloudCover.Cumulus ||
                    day.Clouds == CloudCover.Nimbus ||
                    day.Temperature < WeatherData.MinTemperatureCriteria ||
                    day.Temperature > WeatherData.MaxTemperatureCriteria) continue;

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
                BestDayForLaunch = goodDays[0];
                return goodDays[0];
            }
             // Orders all days that pass the set criteria and converts to array.
            var OrderedByWindArray =
            goodDays.OrderBy(day => day.Wind).ToArray();

            // Checks if the first value of the ordered array by wind is close enough to the
            // second value by using epsilon to negate unexpected rounding done by the type 
            // double. Returns the first element of the ordered array if the values of the wind
            //  speed are enough different.
            if (Math.Abs(OrderedByWindArray[0].Wind - OrderedByWindArray[1].Wind) >= double.Epsilon)
            {
                leadCondition = "Day with the lowest wind speed. " +
                                "Humidity not accounted.";
                BestDayForLaunch = OrderedByWindArray[0];
                return OrderedByWindArray[0];
            }

            // Takes every day that has the same wind speed as the first day,
            // and orders them by humidity. Returns the first day from the new array.
            // Uses epsilon to negate irrelevant difference made by rounding 
            // errors from calculations with double type.
            var FirstByHumidityDay = OrderedByWindArray.Where(day =>
                Math.Abs(day.Wind - OrderedByWindArray[0].Wind) <= double.Epsilon)
                .OrderBy(day => day.Humidity).First();

            leadCondition = "Day with the lowest wind speed and humidity.";
            BestDayForLaunch = FirstByHumidityDay;
            return FirstByHumidityDay;
        }

        // Method that compares the most suitable days for launch in each city.
        // First tries to compare by wind speed, if matching by lowest speeds percist,
        // orders them by humidity, if they are matching again, the closest city
        // to the equator is returned.
        public static CityWeather? FindBestLocation(List<CityWeather> citiesList, out string leadCondition)
        {
            // Removes all cities that do not have a suitable day for launch
            // To not change the original list, elements are coppied to a new list.
            List<CityWeather> cities = new List<CityWeather>(citiesList);
            cities.RemoveAll(city => city.BestDayForLaunch == null);

            var table = new ConsoleTable("City", "Day", "Temperature [°C]", "Wind [m/s]",
                         "Humidity [%]", "Precipitation [%]", "Lighting", "Clouds");

            // Null check provided. Disable false warning.
            #pragma warning disable CS8602 // Dereference of a possibly null reference. 

            foreach (var city in cities)
            {
                table.AddRow(city.Name, city.BestDayForLaunch.Day, city.BestDayForLaunch.Temperature,
                 city.BestDayForLaunch.Wind, city.BestDayForLaunch.Humidity,
                 city.BestDayForLaunch.Precipitation, city.BestDayForLaunch.Lighting,
                 city.BestDayForLaunch.Clouds);
            }

            System.Console.WriteLine("\n Most suitable day for each city: \n");
            table.Write();

            // Check for null reference, no suitable day for launch in any city 
            // for the given period
            if (cities.Count == 0)
            {
                leadCondition = string.Empty;
                return null;
            }


            // Check if only 1 city is suitable for launch for the given period.
            if (cities.Count == 1)
            {
                leadCondition = $"Location {cities[0].Name} " +
                                "is the only suitable city for launch on day " +
                                $"{cities[0].BestDayForLaunch.Day}. ";
                return cities[0];
            }

            var OrderedByWindArray =
                cities.OrderBy(city => city.BestDayForLaunch.Wind).ToArray();

            // Checks if the wind speed of the first city in the orderer array
            // is different enough to the wind speed of the second by using
            // epsilon to negate irrelevant difference made by rounding 
            // errors from calculations with double type.
            if (Math.Abs(OrderedByWindArray[0].BestDayForLaunch.Wind -
                OrderedByWindArray[1].BestDayForLaunch.Wind) >= double.Epsilon)
            {
                leadCondition = $"Location {OrderedByWindArray[0].Name} " +
                                "has the lowest wind speed on day " +
                                $"{OrderedByWindArray[0].BestDayForLaunch.Day}. " +
                                "Humidity not accounted.";
                return OrderedByWindArray[0];
            }

            // Takes the ordered by wind array and finds the cities with the lowest
            // wind speed that are matching, orders those cities by humidity.
            // Uses epsilon to negate irrelevant difference made by rounding 
            // errors from calculations with double type.
            var OrderedByHumidityArray = OrderedByWindArray.Where(
                city => Math.Abs(city.BestDayForLaunch.Wind - 
                OrderedByWindArray[0].BestDayForLaunch.Wind) <= double.Epsilon)
                .OrderBy(city => city.BestDayForLaunch.Humidity).ToArray();
            
            // Checks if the fisrt 2 values of the ordered by humidity array
            // are different enough by using epsilon.
            if (Math.Abs(OrderedByHumidityArray[0].BestDayForLaunch.Humidity -
                OrderedByHumidityArray[1].BestDayForLaunch.Humidity) >= double.Epsilon)
            {
                leadCondition = $"Location {OrderedByHumidityArray[0].Name} has " +
                                "the lowest wind speed and humidity on day " +
                                $"{OrderedByHumidityArray[0].BestDayForLaunch.Day}.";
                return OrderedByHumidityArray[0];
            }

            // Checks in which cities the humidity is the same, orders them
            // by closest to the equator and returns the first element.
            var FirstByClosestToEquator = OrderedByHumidityArray.Where(
                city => Math.Abs(city.BestDayForLaunch.Humidity -
                OrderedByHumidityArray[0].BestDayForLaunch.Humidity) <= double.Epsilon)
                .OrderBy(city => city.Name).First();

            leadCondition = $"Location {FirstByClosestToEquator.Name} has " +
                            "the lowest wind speed and humidity on day " +
                            $"{OrderedByHumidityArray[0].BestDayForLaunch.Day} " +
                            "and is closest to the Equator.";
            return FirstByClosestToEquator;
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        // Method takes each city's most suiatble day for
        // launch and exports the day to a csv file.  
        public static void ExportBestDateToCSV(List<CityWeather> cities, out string filePath)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            filePath = $"{currentDirectory}/LaunchAnalysisReport.csv";

            // Create or overwrite the CSV file
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write header row
                writer.WriteLine("\"Spaceport\", \"Best Day for launch\"");

            // Disable false warning.
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
                // Write data rows
                for (int i = 0; i < cities.Count; i++)
                {
                    // Check for null reference
                    if (cities[i].BestDayForLaunch != null)
                        writer.WriteLine($"\"{cities[i].Name}\", {cities[i].BestDayForLaunch.Day}");
                    else
                        writer.WriteLine($"\"{cities[i].Name}\", \"No suitable day for launch\""); 
                }
            }
            Console.WriteLine($"CSV file '{filePath}' has been created successfully.");
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        // Method takes a city's most suitable day for launch and puts it in a
        // body of a mail to be send with outlook smtp service.
        // Method takes a file path to a file and attaches it to the mail.
        public static void SendDataToMail(CityWeather bestLocation, string filePath)
        {
            if (bestLocation.BestDayForLaunch == null)
                throw new ArgumentException("To send data to mail, provide a city " + 
                                            "that has a suitable date for launch");

            System.Console.WriteLine("\nPlease provide full outlook sender email, password and " +
                             "receiver email to send results to mail.\n");

            System.Console.WriteLine("Enter sender email: ");
            string senderEmail = System.Console.ReadLine() ??
                throw new ArgumentException("Error. No sender email provided.");

            string pattern = @"@outlook\.com$";
            Regex regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
            bool isMatch = regex.IsMatch(senderEmail);
            if (!isMatch) throw new ArgumentException("Please provide Outlook email.");

            // Instantiate the secure string for password.
            SecureString securePwd = new SecureString();
            ConsoleKeyInfo key;

            Console.Write("Enter password: \n");
            do
            {
                key = Console.ReadKey(true);

                securePwd.AppendChar(key.KeyChar);
                Console.Write("*");

                // Exit if Enter key is pressed.
            } while (key.Key != ConsoleKey.Enter);

            securePwd.MakeReadOnly();

            // Create SMTP client
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(senderEmail, securePwd);
            securePwd.Dispose();

            System.Console.WriteLine();
            System.Console.WriteLine("Enter receiver: ");
            string receiverEmail = System.Console.ReadLine() ??
                throw new ArgumentException("Error. No receiver email provided.");

            System.Console.WriteLine("\nSending mail...");

            // Create a new MailMessage
            MailMessage mail = new MailMessage(senderEmail, receiverEmail);
            mail.Subject = "Space Shuttle Launch Analysis Report";
            mail.Body = $"Best location for launch is {bestLocation.Name} " +
                        $"on day {bestLocation.BestDayForLaunch.Day}.";
            mail.IsBodyHtml = false;

            // Attach the CSV file
            Attachment attachment = new Attachment(filePath);
            mail.Attachments.Add(attachment);

            // Send the email
            client.Send(mail);

            Console.WriteLine("\nEmail sent successfully.");
        }
    }
}