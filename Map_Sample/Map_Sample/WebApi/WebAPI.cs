using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Map_Sample.WebApi
{
    public class WebAPI : IDisposable
    {
        private HttpClient Client { get; set; }

        #region IDisposable implementation

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
            }
        }

        #endregion

        public virtual async Task<APIResult<T>> Get<T>(string WebAPIUrl)
        {
            var uri = new Uri(WebAPIUrl);
            Client = new HttpClient();
            Client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            Client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            
            try
            {
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = uri,
                };
               
                request.Headers.Add("Timeout", "20000");

                var response = await Client.SendAsync(request, cancellationToken: CancellationToken.None);
                if (response.IsSuccessStatusCode)
                {
                   var result = await response.Content.ReadAsStringAsync();
                    return await APIResult.WithJsonAsync<T>(result);
                }
            }
            catch (Exception) { }
            return null;
        }
    }    
    public sealed class APIResult
    {
        public static APIResult<X> Create<X>(HttpStatusCode statusCode)
        {
            return new APIResult<X>(statusCode);
        }

        public static APIResult<X> WithResult<X>(X result)
        {
            return new APIResult<X>(result);
        }

        public static Task<APIResult<X>> WithJsonAsync<X>(string json)
        {
            return WithJsonInternalAsync<X>(json);
        }

        private static async Task<APIResult<X>> WithJsonInternalAsync<X>(string json, bool tryWrapJson = true)
        {
            try
            {
                if (typeof(X) == typeof(JToken))
                {
                    var jObj = await Task.Run(() => (object)JToken.Parse(json));
                    var jResult = WithResult((X)jObj);
                    jResult.OriginalJson = json;
                    return jResult;
                }
                if (typeof(X) == typeof(JObject))
                {
                    var jObj = await Task.Run(() => (object)JObject.Parse(json));
                    var jResult = WithResult((X)jObj);
                    jResult.OriginalJson = json;
                    return jResult;
                }

                if (typeof(X) == typeof(JArray))
                {
                    var jArray = await Task.Run(() => (object)JArray.Parse(json));
                    var jaResult = WithResult((X)jArray);
                    jaResult.OriginalJson = json;
                    return jaResult;
                }

                var obj = await Task.Run(() => JsonConvert.DeserializeObject<X>(json));
                var result = APIResult.WithResult(obj);
                result.OriginalJson = json;
                return result;
            }
            catch (JsonReaderException)
            {
                if (tryWrapJson) return await WithJsonInternalAsync<X>("\"" + json + "\"", false);

                throw;
            }
        }
        public static APIResult<X> WithError<X>(string error, Dictionary<string, string> details, HttpStatusCode statusCode)
        {
            return new APIResult<X>(error, details, statusCode);
        }
        public static APIResult<X> WithError<X>(string error, Dictionary<string, string> details)
        {
            return new APIResult<X>(error, details);
        }

        public static APIResult<X> WithException<X>(Exception exception)
        {
            return new APIResult<X>(exception);
        }
        public static APIResult<X> Canceled<X>()
        {
            return new APIResult<X>(true);
        }
    }

    public class APIResult<T>
    {
        internal APIResult(HttpStatusCode statusCode)
        {
            Result = default(T);
            StatusCode = statusCode;
        }

        internal APIResult(T result)
        {
            Result = result;
            StatusCode = HttpStatusCode.OK;
        }
        internal APIResult(bool isCanceled) : this(HttpStatusCode.BadRequest)
        {
            IsCanceled = isCanceled;
        }
        internal APIResult(string error, HttpStatusCode statusCode) : this(statusCode)
        {
            Error = error;
        }

        internal APIResult(string error, Dictionary<string, string> details, HttpStatusCode statusCode) : this(error, statusCode)
        {
            ErrorDetails = details;
        }
        internal APIResult(string error, Dictionary<string, string> details) : this(error, HttpStatusCode.BadRequest)
        {
            ErrorDetails = details;
        }
        internal APIResult(Exception exception) : this(HttpStatusCode.BadRequest)
        {
            if (exception == null)
                return;
            Error = exception.Message;
            Exception = exception;
        }

        public readonly T Result;
        public readonly string Error;
        public bool IsForbidden => StatusCode == HttpStatusCode.Forbidden;

        public readonly HttpStatusCode StatusCode;
        public readonly bool IsCanceled;
        public readonly Dictionary<string, string> ErrorDetails;
        public readonly Exception Exception;

        public bool Failed => !IsCanceled &&
            (Exception != null ||
            !string.IsNullOrWhiteSpace(Error) ||
            (int)StatusCode < 200 || (int)StatusCode > 299);

        public bool Succeeded => Result != null;

        public string OriginalJson { get; set; }
    }


}
