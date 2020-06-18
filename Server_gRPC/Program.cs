using System;
using System.Diagnostics;
using System.Timers;
using Grpc.Core;
using Server;
using Weatherstation;

namespace Server_gRPC
{
    class Program
    {
        const int port = 50051;
        private static Timer timer;
        private static Stopwatch stopwatch;

        static void Main(string[] args)
        {
            //Create the channel and client to send the data from.
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
            //Create a timer with a one second interval and a stopwatch for monitoring.
            timer = new Timer(1000);
            stopwatch = new Stopwatch();

            //Hook up the Elapsed event for the timer. 
            timer.Elapsed += (sender, e) => OnTimerElapsed(sender, e, client);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e, WeatherDataSender.WeatherDataSenderClient client)
        {
            //Create new random data.
            WeatherData weatherData = new WeatherData();
            DataRequest request = new DataRequest()
            {
                Time = weatherData.Timestamp,
                Temperature = weatherData.Temperature,
                Humidity = weatherData.Humidity,
                WindDirection = weatherData.WindDirection,
                Windspeed = weatherData.WindSpeed,
                AtmPressure = weatherData.AtmosphericPressure
            };

            //Start the stopwatch and send the data.
            stopwatch.Start();
            DataReply response = client.SendData(request);

            //Stop the stopwatch when a response is received. Log the response and reset the timer.
            stopwatch.Stop();
            Console.WriteLine("Response: " + response.Message);
            Console.WriteLine($"Response time: {stopwatch.ElapsedMilliseconds} ms");
            stopwatch.Reset();
        }
    }
}