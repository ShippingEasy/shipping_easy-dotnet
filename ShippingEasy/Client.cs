using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace ShippingEasy
{
    public class Client
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _baseUrl;
        private Uri _baseUri;


        public Client(string apiKey, string apiSecret, string baseUrl)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _baseUrl = baseUrl;
        }

        public Client(string apiKey, string apiSecret) : this(apiKey,apiSecret, "http://172.16.65.1:5000")
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public Client()
            : this("f9a7c8ebdfd34beaf260d9b0296c7059", "850fd4e023478758360b0d1d1817448f0a57b3176be25ffe8a7cf2236eca9ec3")
        {
        }

        public string GetOrders()
        {
            return MakeRequest("/api/orders");
        }

        public string MakeRequest(string path, IDictionary<String, String> query = null)
        {
            var parameters = new Dictionary<string, string>(query ?? new Dictionary<string, string>());
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);
            parameters.Add("api_key", _apiKey);
            parameters.Add("api_timestamp", timestamp.ToString());
            var signature = BuildSignature("GET", "/api/orders", parameters);
            parameters.Add("api_signature", signature);

            var queryString = parameters.OrderBy(param => param.Key)
                .Aggregate("", (all, param) => String.Format("{0}&{1}={2}", all, param.Key, param.Value));
            var pathAndQuery = parameters.Count > 0 ? path + "?" + queryString.Substring(1) : path;
            var requestUrl = new Uri(BaseUri, pathAndQuery);

            var request = ((HttpWebRequest) WebRequest.Create(requestUrl));
            request.Method = "GET";
            request.Accept = "application/json";
            request.UserAgent = "ShippingEasy .NET Client v1.0.0";
            try
            {
                using (var response = request.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    if (responseStream == null) return string.Empty;
                    var streamReader = new StreamReader(responseStream);
                    return streamReader.ReadToEnd();
                }
            }
            catch (WebException webException)
            {
                return webException.Message;
            }
        }

        public string BuildSignature(string method, string path, IDictionary<string, string> parameters)
        {
            return "foo";
        }

        public Uri BaseUri { get { return _baseUri ?? new Uri(_baseUrl, UriKind.Absolute); } }

        public static string MakeSignature(string apiSecret, string httpMethod, string path, IDictionary<string, string> parameters, string body)
        {
            return body;
        }
    }

}
