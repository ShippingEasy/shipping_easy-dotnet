using Newtonsoft.Json;

namespace ShippingEasy
{
    public class ApiResponse
    {
        public HttpResponse HttpResponse { get; protected set; }
        public bool Success
        {
            get { return Errors == null; }
        }

        [JsonProperty]
        public object Errors { get; protected set; }
    }
}