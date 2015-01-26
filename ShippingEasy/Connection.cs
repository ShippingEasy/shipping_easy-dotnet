using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace ShippingEasy
{
    public class Connection
    {
        public const string DefaultApiUrl = "https://app.shippingeasy.com";
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly Uri _baseUri;


        public Connection(string apiKey, string apiSecret, string baseUrl = null)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _baseUri = new Uri(baseUrl ?? DefaultApiUrl, UriKind.Absolute);
        }

        public string GetAllOrdersJson(IDictionary<string, string> options = null)
        {
            return MakeRequest("GET", "/api/orders", null, options);
        }


        public string GetStoreOrdersJson(string storeApiKey, IDictionary<string, string> options = null)
        {
            return MakeRequest("GET", String.Format("/api/stores/{0}/orders", storeApiKey), null, options);
        }

        public string CreateOrderFromJson(string storeApiKey, string jsonBody)
        {
            return MakeRequest("POST", String.Format("/api/stores/{0}/orders", storeApiKey), jsonBody);
        }

        public string MakeRequest(string httpMethod, string path, string body = null, IDictionary<string, string> query = null)
        {
            var request = BuildRequest(path, query, httpMethod, body);
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

        private WebRequest BuildRequest(string path, IDictionary<string, string> query, string httpMethod, string body)
        {
            var parameters = new Dictionary<string, string>(query ?? new Dictionary<string, string>());
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64((DateTime.UtcNow - epoch).TotalSeconds);
            parameters.Add("api_key", _apiKey);
            parameters.Add("api_timestamp", timestamp.ToString());
            var signature = BuildSignature(httpMethod, path, parameters, body);
            parameters.Add("api_signature", signature);

            var queryString = String.Join("&", parameters.Select(param => String.Format("{0}={1}", param.Key, param.Value)));
            var pathAndQuery = path + "?" + queryString;
            var requestUrl = new Uri(_baseUri, pathAndQuery);

            Debug.WriteLine("Calling: {0}", requestUrl);
            var request = ((HttpWebRequest) WebRequest.Create(requestUrl));
            request.Method = httpMethod;
            request.Accept = "application/json";
            request.UserAgent = "ShippingEasy .NET Client v1.0.0";

            if (body == null) return request;
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(body);
            }
            return request;
        }


        public string BuildSignature(string method, string path, IDictionary<string, string> parameters, string body)
        {
            return new Signature(_apiSecret, method, path, parameters, body).ToString();
        }

    }

    [Serializable]
    public class ApiException : Exception
    {
        public ApiException(HttpStatusCode statusCode, string errorBody)
            : base(String.Format("{0}: {1}", statusCode, errorBody))
        {
        }
    }
}
