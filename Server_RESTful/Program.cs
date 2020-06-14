using System;
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

        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:3000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                await SetTimer(client);

            }
        }

        private static async Task SetTimer(HttpClient client)
        {
            timer = new Timer(1000);
            timer.Elapsed += async (sender, e) => await OnTimerElapsed(sender, e, client);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static async Task OnTimerElapsed(object sender, ElapsedEventArgs e, HttpClient client)
        {
            WeatherData weatherData = new WeatherData();

            HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api", weatherData);

            if (responseMessage.IsSuccessStatusCode)
            {
                Uri apiUri = responseMessage.Headers.Location;

                //Process reponse.
                Console.WriteLine(responseMessage.Content);
            }
        }
    }
}