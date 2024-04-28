using System.Text;
using System.Net;
using System.Net.Mail;
using System.Security;
using System.Text.RegularExpressions;

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
                if (day.Precipitation > WeatherData.PrecipitationCriteria ||
                    day.Lighting == true ||
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
            cities.RemoveAll(city => city.BestDayForLaunch == null);

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
                                "has the lowest wind speed on day " +
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

        public static void ExportBestDateToCSV(List<CityWeather> cities, out string filePath)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            filePath = $"{currentDirectory}/LaunchAnalysisReport.csv";

            // Create or overwrite the CSV file
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                // Write header row
                writer.WriteLine("\"Spaceport\", \"Best Day for launch\"");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                // Write data rows
                for (int i = 0; i < cities.Count; i++)
                {
                    if (cities[i].BestDayForLaunch == null)
                        writer.WriteLine($"\"{cities[i].Name}\", {cities[i].BestDayForLaunch.Day}");
                    else
                        writer.WriteLine($"\"{cities[i].Name}\", \"No suitable day for launch\"");
                }
            }
            Console.WriteLine($"CSV file '{filePath}' has been created successfully.");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public static void SendDataToMail(List<CityWeather> cities,
                                          CityWeather bestLocation, string filePath)
        {
            if (bestLocation.BestDayForLaunch == null)
                throw new ArgumentException("[bestLocation.BestDayForLaunch] is null.");

            System.Console.WriteLine("\nPlease provide full outlook sender email, password and " +
                             "receiver email to send results to mail.\n");

            System.Console.WriteLine("Enter sender email: ");
            string senderEmail = System.Console.ReadLine() ??
                throw new ArgumentException("Error. No sender email provided.");

            string pattern = @"@outlook\.com$";
            Regex regex = new Regex(pattern);
            bool isMatch = regex.IsMatch(senderEmail);
            if (!isMatch) throw new ArgumentException("Please provide Outlook email.");

            // Instantiate the secure string.
            SecureString securePwd = new SecureString();
            ConsoleKeyInfo key;

            Console.Write("Enter password: ");
            do
            {
                key = Console.ReadKey(true);

                // Append the character to the password.
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

            System.Console.WriteLine("Enter receiver: ");
            string receiverEmail = System.Console.ReadLine() ??
                throw new ArgumentException("Error. No receiver email provided.");

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