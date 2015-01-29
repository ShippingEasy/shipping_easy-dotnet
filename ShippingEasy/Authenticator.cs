using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ShippingEasy
{
    /// <summary>
    /// Used to verify signed requests from the ShippingEasy API
    /// </summary>
    /// <remarks>
    /// ShippingEasy may send POST requests to your server for
    /// various events (for example: shipment notifications when
    /// an order is shipped). The requests from ShippingEasy will
    /// always be signed with your API Secret. The signature is
    /// included in the request as the api_signature key in the
    /// query string. You can use this class to check the validity
    /// of the signature to ensure it came from ShippingEasy.
    /// </remarks>
    public class Authenticator
    {
        private readonly string _apiSecret;

        /// <summary>
        /// Create an instance of the Authenticator
        /// </summary>
        /// <param name="apiSecret">The API Secret that authenticates your ShippingEasy acccount.
        /// <remarks>Available on the API Credentials section of the Settings page.
        /// Not to be confused with your Store's API Key on the Store settings page.
        /// </remarks>
        /// </param>


        public Authenticator(string apiSecret)
        {
            _apiSecret = apiSecret;
        }

        /// <summary>
        /// Check the signature on a specific request
        /// </summary>
        /// <param name="httpMethod">The HTTP method used for the request.
        /// <remarks>Can be retrieved from System.Web.HttpContext.Current.Request.HttpMethod</remarks>
        /// </param>
        /// <param name="path">The path without querystring for the request
        /// <remarks>Can be retrieved from Request.Path</remarks></param>
        /// <param name="queryString">The querystring for the request
        /// <remarks>Can be retrieved from Request.QueryString.ToString()
        ///  or Request.ServerVariables["QUERY_STRING"]</remarks>
        /// </param>
        /// <param name="requestBody">The body of the request</param>
        /// <returns>True if the signature in the querystring matches the
        ///  signature computed with the provided apiKey
        /// </returns>
        public bool Verify(string httpMethod, string path, string queryString, string requestBody)
        {
            var parameters = ParseQueryString(queryString);
            
            if (!parameters.ContainsKey(Signature.ParameterKey))
            {
                throw new ArgumentException(String.Format("Must contain an {0} parameter", Signature.ParameterKey), "queryString");
            }
            var remoteSignature = parameters[Signature.ParameterKey];
            var localSignature = new Signature(_apiSecret, httpMethod, path, parameters, requestBody);
            return (remoteSignature == localSignature.ToString());
        }

        private static Dictionary<string, string> ParseQueryString(string queryString)
        {
            return Regex.Matches(queryString, "([^?=&]+)(=([^&]*))?")
                .Cast<Match>()
                .ToDictionary(x => x.Groups[1].Value, x => x.Groups[3].Value);
        }
    }
}