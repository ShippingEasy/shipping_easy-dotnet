using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace ShippingEasy
{
    /// <summary>
    /// Low level access to the ShippingEasy API endpoints
    /// </summary>
    /// <remarks>
    /// This class contains the methods to send and receive JSON strings
    /// to the ShippingEasy API; ensuring the URL and authentication
    /// signatures are constructed properly.
    /// In most cases, you do not need to use this class directly, but it
    /// can be useful if the higher-level <see cref="ShippingEasy.Client"/>
    /// does not provide the functionality you need. For example, you want
    /// more control over the serialization/deserialization of the JSON, or
    /// if you need to make API requests that are not yet exposed
    /// via <see cref="ShippingEasy.Client"/>.
    /// </remarks>
    public class Connection
    {
        public const string DefaultApiUrl = "https://app.shippingeasy.com";
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly Uri _baseUri;

        /// <summary>
        /// Creates a new instance of a Connection
        /// </summary>
        /// <param name="apiKey">The API Key that identifies your ShippingEasy account.
        /// <remarks>Available on the API Credentials section of the Settings page.
        /// Do not confuse with the Store API Key on the Store settings page which
        /// identifies a specific store integration.</remarks>
        /// </param>
        /// <param name="apiSecret">The API Secret that authenticates your ShippingEasy acccount.
        /// <remarks>Available on the API Credentials section of the Settings page.</remarks></param>
        /// <param name="baseUrl">The server URL hosting the ShippingEasy API
        /// <remarks>This is provided for development/testing purposes. In normal production
        /// scenarios you should omit this value (or pass null)
        /// </remarks>
        /// </param>
        public Connection(string apiKey, string apiSecret, string baseUrl = null)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _baseUri = new Uri(baseUrl ?? DefaultApiUrl, UriKind.Absolute);
        }

        public HttpResponse GetAllOrdersJson(IDictionary<string, string> options = null)
        {
            return MakeRequest("GET", "/api/orders", null, options);
        }


        public HttpResponse GetStoreOrdersJson(string storeApiKey, IDictionary<string, string> options = null)
        {
            return MakeRequest("GET", String.Format("/api/stores/{0}/orders", storeApiKey), query: options);
        }

        public HttpResponse CreateOrderFromJson(string storeApiKey, string jsonBody)
        {
            return MakeRequest("POST", String.Format("/api/stores/{0}/orders", storeApiKey), jsonBody);
        }
        
        /// <summary>
        /// A hook to override the method that takes a fully-constructed WebRequest and returns a response string
        /// </summary>
        /// <remarks>
        /// Override if you need to handle responses/errors differently, or modify the request before executing.
        /// </remarks>
        public Func<WebRequest, HttpResponse> RequestRunner { get; set; }
        public HttpResponse MakeRequest(string httpMethod, string path, string body = null, IDictionary<string, string> query = null)
        {
            var request = BuildRequest(path, query, httpMethod, body);
            return (RequestRunner ?? HandleResponse)(request);
        }

        private static HttpResponse HandleResponse(WebRequest request)
        {
            try
            {
                using (var response = request.GetResponse())
                {
                    var responseStream = response.GetResponseStream();
                    var statusCode = (int) ((HttpWebResponse) response).StatusCode;
                    return new HttpResponse{ Body = ReadBody(responseStream), Status = statusCode};
                }
            }
            catch (WebException webException)
            {
                if (webException.Response == null) throw;
                var failureResponse = ((HttpWebResponse)webException.Response);
                var errorBody = ReadBody(failureResponse.GetResponseStream());
                var statusCode = (int) failureResponse.StatusCode;
                return new HttpResponse {Body = errorBody, Status = statusCode};
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
            (SetRequestBody ?? WriteToRequestStream)(request, body);
            return request;
        }

        /// <summary>
        /// Override writing the request body to the HttpWebRequest
        /// <remarks>
        /// Calling WebRequest.GetRequestStream() opens a connection to the URL,
        /// even before you execute the request (GetResponse). This hook
        /// allows us to stub out the functionality so we do not connect
        /// to any remote URLs from unit tests. 
        /// </remarks>
        /// </summary>
        public Action<WebRequest, string> SetRequestBody { get; set; }
        private static void WriteToRequestStream(WebRequest request, string body)
        {
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(body);
            }
        }

        public string BuildSignature(string method, string path, IDictionary<string, string> parameters, string body)
        {
            return new Signature(_apiSecret, method, path, parameters, body).ToString();
        }

    }

    public class HttpResponse
    {
        public string Body { get; set; }
        public int Status { get; set; }
    }
}
