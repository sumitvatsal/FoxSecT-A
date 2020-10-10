using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace FoxSec.Web.ViewModels
{
    public class TimeZoneModel
    {
       
        private long GetUnixTimeStampFromDateTime(DateTime dt)
        {
            DateTime epochDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = dt - epochDate;
            return (int)ts.TotalSeconds;
        }

        private DateTime GetDateTimeFromUnixTimeStamp(double unixTimeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimeStamp);
            return dt;
        }

        private GeoLocation GetCoordinatesByLocationName(string address, string ApiKey)
        {
            string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key={1}", Uri.EscapeDataString(address), ApiKey);

            XDocument xdoc = GetXmlResponse(requestUri);

            XElement status = xdoc.Element("GeocodeResponse").Element("status");
            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("geometry").Element("location");
            XElement lat = locationElement.Element("lat");
            XElement lng = locationElement.Element("lng");

            return new GeoLocation()
            {
                Latitude = Convert.ToDouble(lat.Value),
                Longitude = Convert.ToDouble(lng.Value)
            };
        }

        private GoogleTimeZoneResult GetConvertedDateTimeBasedOnAddress(GeoLocation location, long timestamp, string ApiKey)
        {
            //string requestUri = string.Format("https://maps.googleapis.com/maps/api/timezone/xml?location={0},{1}&timestamp={2}&key={3}", location.Latitude, location.Longitude, timestamp, ApiKey);

            //XDocument xdoc = GetXmlResponse(requestUri);

            //XElement result = xdoc.Element("TimeZoneResponse");
            //XElement rawOffset = result.Element("raw_offset");
            //XElement dstOfset = result.Element("dst_offset");
            //XElement timeZoneId = result.Element("time_zone_id");
            //XElement timeZoneName = result.Element("time_zone_name");
            JsonRead jr = new JsonRead();

            var timeZoneRespontimeZoneRequest = "https://maps.googleapis.com/maps/api/timezone/json?location=" + location.Latitude + "," + location.Longitude + "&timestamp=" + timestamp + "&sensor=false";
            var timeZoneResponseString = new System.Net.WebClient().DownloadString(timeZoneRespontimeZoneRequest);
            jr = new JavaScriptSerializer().Deserialize<JsonRead>(timeZoneResponseString);

            return new GoogleTimeZoneResult()
            {
                DateTime = GetDateTimeFromUnixTimeStamp(Convert.ToDouble(timestamp) + jr.rawOffset + jr.dstOffset),
                TimeZoneId = jr.timeZoneId,
                TimeZoneName = jr.timeZoneName
            };
        }

        public XDocument GetXmlResponse(string url)
        {
            Uri ServivrUri = new Uri(url);
            WebClient proxy = new WebClient();
            byte[] abc = proxy.DownloadData(ServivrUri);

            MemoryStream stream = new MemoryStream(abc);
            var xDocument = XDocument.Load(stream);
            return xDocument;
        }

        public GoogleTimeZoneResult GetConvertedDateTimeBasedOnAddress(string address, DateTime dateTime, string ApiKey)
        {
            long timestamp = GetUnixTimeStampFromDateTime(TimeZoneInfo.ConvertTimeToUtc(dateTime));

            //if (previousAddress != address)
            //{
            //    this.location = GetCoordinatesByLocationName(address, ApiKey);

            //    previousAddress = address;

            //    if (this.location == null)
            //    {
            //        return null;
            //    }
            //}
            GeoLocation location = new GeoLocation();
            location = GetCoordinatesByLocationName(address, ApiKey);
            return GetConvertedDateTimeBasedOnAddress(location, timestamp, ApiKey);
        }

    }

    public class JsonRead
    {
        public int dstOffset { get; set; }
        public int rawOffset { get; set; }
        public string status { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
    }
    class GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class GoogleTimeZone
    {
        private string apiKey;
        //private GeoLocation location;
        private string previousAddress = string.Empty;

        public GoogleTimeZone(string apiKey)
        {
            this.apiKey = apiKey;
        }
    }

    public class GoogleTimeZoneResult
    {
        public DateTime DateTime { get; set; }
        public string TimeZoneId { get; set; }
        public string TimeZoneName { get; set; }
    }
}