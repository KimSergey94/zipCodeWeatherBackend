using System;
using System.Collections.Generic;
using System.Text;

namespace zipCodeWeather.DLL.Models
{
    public class WeatherResult
    {
        public string City { get; set; }
        public double Temperature { get; set; }
        public string TimeZone { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
