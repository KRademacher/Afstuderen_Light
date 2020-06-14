using System;
using Google.Protobuf.WellKnownTypes;

namespace Server
{
    public class WeatherData
    {
        public Timestamp Timestamp { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string WindDirection { get; set; }
        public float WindSpeed { get; set; }
        public int AtmosphericPressure { get; set; }

        private readonly Random random = new Random();

        public WeatherData()
        {
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow);
            Temperature = (float)random.Next(200, 281) / 10;
            Humidity = (float)random.Next(700, 1001) / 10;
            WindDirection = PickWindDirection(random.Next(1, 17));
            WindSpeed = (float)random.Next(100, 201) / 10;
            AtmosphericPressure = random.Next(800, 1001);
        }

        //Don't look at this, it's ugly.
        private string PickWindDirection(int i)
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