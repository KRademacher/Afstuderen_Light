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
    }
}