using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using zipCodeWeather.DLL.Models;
using System.Web.Http.Cors;
using System.IO;
using Newtonsoft.Json.Linq;

namespace zipCodeWeather.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class WeatherController : ApiController
    {
        // GET: api/Weather/GetWeather
        [ResponseType(typeof(WeatherResult))]
        public HttpResponseMessage GetWeather(string zipCode)
        {
            var weather = GetWeatherData(zipCode);
            var res = Request.CreateResponse(HttpStatusCode.Created, weather);
            res.Content = new StringContent(new JavaScriptSerializer().Serialize(weather), Encoding.UTF8, "application/json");
            return res;
        }

        private WeatherResult GetWeatherData(string zipCode)
        {
            long code = 0;
            if(!long.TryParse(zipCode, out code))
            {
                return new WeatherResult() { Status = "Error", ErrorMessage = "Zip code must contain only digits without spaces, e.g. 75034" };
            }
            try
            {
                var googleApiKey = "AIzaSyCWVwKTwK7KX9WNBx65CxUlm4v-Axf5_G0";
                var geocodeReq = WebRequest.Create(@"https://maps.googleapis.com/maps/api/geocode/json?key=" + googleApiKey + "&components=postal_code:" + zipCode);
                geocodeReq.ContentType = "application/json; charset=utf-8";
                var geocodeResponse = (HttpWebResponse)geocodeReq.GetResponse();

                if (geocodeResponse.StatusDescription == "OK")
                {
                    var weatherGeo = GetWeatherLatitudeAndLongitudeFromGeocodeResponse(geocodeResponse);

                    try
                    {
                        var openweatherApiKey = "0053ea67e03320c6f7a322ed2b84ba0a";
                        var openweatherReq = WebRequest.Create("https://api.openweathermap.org/data/2.5/onecall?lat=" + weatherGeo.Latitude + "&lon=" + weatherGeo.Longitude + "&units=metric" + "&exclude=minutely,hourly,daily" + "&appid=" + openweatherApiKey);
                        var openweatherResponse = (HttpWebResponse)openweatherReq.GetResponse();
                        if (openweatherResponse.StatusDescription == "OK")
                        {
                            var openweatherData = GetWeatherDataFromOpenweatherResponse(openweatherResponse);

                            try
                            {
                                var timestamp = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;//January 1, 1970 
                                var googleTimezoneReq = WebRequest.Create("https://maps.googleapis.com/maps/api/timezone/json?location=" + weatherGeo.Latitude+ ","+weatherGeo.Longitude + "&timestamp=" + timestamp + "&key=" + googleApiKey);
                                var googleTimezoneResponse = (HttpWebResponse)googleTimezoneReq.GetResponse();

                                if (googleTimezoneResponse.StatusDescription == "OK")
                                {
                                    var googleTimezoneResponseData = GetGoogleTimezoneResponseData(googleTimezoneResponse);

                                    return new WeatherResult() { City = openweatherData.TimeZone, Temperature = openweatherData.Temperature, TimeZone = googleTimezoneResponseData.TimeZone, Status = "OK" };
                                }
                            }
                            catch (Exception e) { return new WeatherResult() { Status = "Error", ErrorMessage = e.Message }; }
                        }
                    }
                    catch (Exception e) { return new WeatherResult() { Status = "Error", ErrorMessage = e.Message }; }
                }
            }
            catch (Exception e) { return new WeatherResult() { Status = "Error", ErrorMessage = e.Message }; }
            
            return new WeatherResult() { Status = "Error", ErrorMessage = "No result" };
        }
        private WeatherGeo GetWeatherLatitudeAndLongitudeFromGeocodeResponse(HttpWebResponse geocodeResponse)
        {
            Stream dataStream = geocodeResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromGeocode = reader.ReadToEnd();
            dynamic data = JObject.Parse(responseFromGeocode);
            try
            {
                var results = data["results"][0]["geometry"];
                var location = results["location"];
                var lng = location["lng"];
                var lat = location["lat"];
                return new WeatherGeo() { Latitude = lat, Longitude = lng };
            }
            catch { throw new Exception("No found results with the provided zip code"); }
        }
        private OpenWeatherResult GetWeatherDataFromOpenweatherResponse(HttpWebResponse openweatherResponse)
        {
            Stream dataStream = openweatherResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromGeocode = reader.ReadToEnd();
            dynamic data = JObject.Parse(responseFromGeocode);
            var timeZone = data["timezone"];
            var currentTemp = data["current"]["temp"];
            return new OpenWeatherResult() { Temperature = currentTemp, TimeZone = timeZone };
        }
        private GoogleTimezoneResult GetGoogleTimezoneResponseData(HttpWebResponse googleTimezoneResponse)
        {
            Stream dataStream = googleTimezoneResponse.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromGeocode = reader.ReadToEnd();
            dynamic data = JObject.Parse(responseFromGeocode);
            var timeZoneId = data["timeZoneId"];
            var timeZoneName = data["timeZoneName"];
            return new GoogleTimezoneResult() { TimeZone = timeZoneName, TimeZoneId = timeZoneId };
        }
    }
}