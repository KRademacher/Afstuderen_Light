using System;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Server;
using Weatherstation;

namespace Server_gRPC
{
    class Program
    {
        const int port = 50051;
        static readonly Random random = new Random();
        static TimeSpan startTimeSpan = TimeSpan.Zero;
        static TimeSpan secondTimeSpan = TimeSpan.FromSeconds(1);
        private static System.Timers.Timer timer;

        static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:" + port, ChannelCredentials.Insecure);

            var client = new WeatherDataSender.WeatherDataSenderClient(channel);

            SetTimer(client);

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.ReadLine();
            timer.Stop();
            timer.Dispose();

            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer(WeatherDataSender.WeatherDataSenderClient client)
        {
            // Create a timer with a one second interval.
            timer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += (sender, e) => TimerElapsed(sender, e, client);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e, WeatherDataSender.WeatherDataSenderClient client)
        {
            WeatherData weatherData = GenerateWeatherData(random);

            DataReply response = client.SendData(new DataRequest
            {
                Time = weatherData.Timestamp,
                Temperature = weatherData.Temperature,
                Humidity = weatherData.Humidity,
                WindDirection = weatherData.WindDirection,
                Windspeed = weatherData.WindSpeed,
                AtmPressure = weatherData.AtmosphericPressure
            });

            Console.WriteLine("Response: " + response.Message);
        }

        static private WeatherData GenerateWeatherData(Random random)
        {
            WeatherData weatherData = new WeatherData()
            {
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                Temperature = (random.Next(250, 261) / 10),
                Humidity = (random.Next(700, 1001) / 10),
                WindDirection = PickWindDirection(random.Next(1, 17)),
                WindSpeed = (random.Next(100, 201) / 10),
                AtmosphericPressure = random.Next(800, 1001)
            };

            return weatherData;
        }

        //Don't look at this, it's ugly.
        static private string PickWindDirection(int i)
        {
            switch (i)
            {
                case 1:
                    return "N";
                case 2:
                    return "NNE";
                case 3:
                    return "NE";
                case 4:
                    return "ENE";
                case 5:
                    return "E";
                case 6:
                    return "ESE";
                case 7:
                    return "SE";
                case 8:
                    return "SSE";
                case 9:
                    return "S";
                case 10:
                    return "SSW";
                case 11:
                    return "SW";
                case 12:
                    return "WSW";
                case 13:
                    return "W";
                case 14:
                    return "WNW";
                case 15:
                    return "NW";
                case 16:
                    return "NNW";
                default:
                    return "";
            }
        }
    }
}