using Map_Sample.Helpers;
using Map_Sample.WebApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Map_Sample.API
{
    public static class CovidDashboardAPI
    {
        public static async Task<dynamic> GetTrackingData(string fromDate, string toDate)
        {           
            using (var api = new WebAPI())
            {
                var WebAPIUrl = $"https://api.covid19tracking.narrativa.com/api?date_from={fromDate}&date_to={toDate}"; 
                var response = await api.Get<dynamic>(WebAPIUrl);
                if(response != null)
                    return response.Result;
                return null;
            }
        }

        public static async Task<dynamic> GetCountryData(string countryCode)
        {
            using (var api = new WebAPI())
            {
                var WebAPIUrl = $"https://newsapi.org/v2/top-headlines?country={countryCode}&category=health&apiKey={Constants.NewsAPIKey}";
                var response = await api.Get<dynamic>(WebAPIUrl);
                if(response != null)
                    return response.Result;
                return null;
            }
        }


        public static async Task<dynamic> GetCountryCode(string name)
        {            
            using (var api = new WebAPI())
            {
                var WebAPIUrl = $"https://restcountries.eu/rest/v2/name/{name}?fullText=true";
                var response = await api.Get<dynamic>(WebAPIUrl);
                if(response != null)
                    return response.Result.ToObject<List<JObject>>()[0];
                return null;
            }
        }
    }
}
