using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace ShippingEasy
{
    public class Client
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly Uri _baseUri;


        public Client(string apiKey, string apiSecret, string baseUrl)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _baseUri = new Uri(baseUrl, UriKind.Absolute);
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
            return MakeRequest("GET", "/api/orders");
        }

        public string MakeRequest(string httpMethod, string path, IDictionary<string, string> query = null)
        {
            var request = BuildRequest(path, query, httpMethod);
            return HandleResponse(request);
        }

        private static string HandleResponse(WebRequest request)
        {
            try
            {
                using (var response = request.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    return ReadBody(responseStream);
                }
            }
            catch (WebException webException)
            {
                if (webException.Response == null) throw;
                var failureResponse = ((HttpWebResponse)webException.Response);
                var statusCode = failureResponse.StatusCode;
                var errorBody = ReadBody(failureResponse.GetResponseStream());
                throw new ApiException(statusCode, errorBody);
            }
        }

        private static string ReadBody(Stream responseStream)
        {
            if (responseStream == null) return string.Empty;
            var streamReader = new StreamReader(responseStream);
            return streamReader.ReadToEnd();
        }

        private WebRequest BuildRequest(string path, IDictionary<string, string> query, string httpMethod)
        {
            var parameters = new Dictionary<string, string>(query ?? new Dictionary<string, string>());
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);
            parameters.Add("api_key", _apiKey);
            parameters.Add("api_timestamp", timestamp.ToString());
            var signature = BuildSignature(httpMethod, path, parameters);
            parameters.Add("api_signature", signature);

            var queryString = String.Join("&", parameters.Select(param => String.Format("{0}={1}", param.Key, param.Value)));
            var pathAndQuery = path + "?" + queryString;
            var requestUrl = new Uri(_baseUri, pathAndQuery);

            Debug.WriteLine("Calling: {0}", requestUrl);
            var request = ((HttpWebRequest) WebRequest.Create(requestUrl));
            request.Method = httpMethod;
            request.Accept = "application/json";
            request.UserAgent = "ShippingEasy .NET Client v1.0.0";
            return request;
        }


        //  Valid Responses:
        // all orders: {"orders": [], "meta": {}}
        // one order: {"order": {}}
        // create: {"order": {}}
        // create (fail): {"errors":[]}
        // failures: {"errors": [{"message": "foo", "status": 403}]}



        public string BuildSignature(string method, string path, IDictionary<string, string> parameters)
        {
            return new Signature(_apiSecret, method, path, parameters).ToString();
        }

    }

    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string errorBody)
            : base(String.Format("{0}: {1}", statusCode, errorBody))
        {
        }
    }
}
