using System;
using Newtonsoft.Json;

namespace ShippingEasy.Responses
{
    public class ApiResponse
    {
        /// <summary>
        /// The unparsed response from the remote server
        /// </summary>
        public HttpResponse HttpResponse { get; set; }
        
        /// <summary>
        /// Indiciates if request was successful
        /// </summary>
        public bool Success
        {
            get { return ParseException == null && ErrorObject == null; }
        }

        /// <summary>
        /// Exception thrown while trying to parse response
        /// </summary>
        public Exception ParseException { get; set; }

        /// <summary>
        /// The untyped error object. Could be a JSON array, object, or string.
        /// </summary>
        [JsonProperty("errors")]
        public object ErrorObject { get; set; }
        
        /// <summary>
        /// The errors as a string. May contain unparsed JSON.
        /// </summary>
        [JsonIgnore]
        public string Errors { get { return ErrorObject.ToString(); } }
    }
}