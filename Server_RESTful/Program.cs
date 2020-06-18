using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Timers;
using Server;

namespace Server_RESTful
{
    class Program
    {
        private static Timer timer;
        private static Stopwatch stopwatch;

        static void Main(string[] args)
        {
            //Create a new HTTPClient to POST from.
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:3000/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SetTimer(client);

            Console.WriteLine("\nPress the Enter key to exit the application...\n");

            Console.ReadLine();
            timer.Stop();
            timer.Dispose();

            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer(HttpClient client)
        {
            //Create a timer with a one second interval and a stopwatch for monitoring.
            timer = new Timer(1000);
            stopwatch = new Stopwatch();

            //Hook up the Elapsed event for the timer. 
            timer.Elapsed += async (sender, e) => await OnTimerElapsed(sender, e, client);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static async Task OnTimerElapsed(object sender, ElapsedEventArgs e, HttpClient client)
        {
            //Create new random data.
            WeatherData weatherData = new WeatherData();

            //Start the stopwatch and send a JSON version of the data. Wait for the response.
            stopwatch.Start();
            var responseMessage = await client.PostAsJsonAsync("weather", weatherData);

            if (responseMessage.IsSuccessStatusCode)
            {
                //Stop stopwatch and process reponse.
                stopwatch.Stop();
                string response = await responseMessage.Content.ReadAsStringAsync();
                Console.WriteLine($"Response: {response}");
                Console.WriteLine($"Response time: {stopwatch.ElapsedMilliseconds} ms");
            }
            else
            {
                //Stop stopwatch and log the timestamp of the failed package.
                stopwatch.Stop();
                Console.WriteLine("Error sending data package: " + weatherData.Timestamp);
            }
            stopwatch.Reset();
        }
    }
}