using System.Diagnostics;
using Newtonsoft.Json;
using ShippingEasy.Responses;

namespace ShippingEasy
{
    public class ResponseHandler
    {
        public TResponse Build<TResponse>(HttpResponse response) where TResponse : ApiResponse, new()
        {
            var responseType = typeof(TResponse);
            Debug.WriteLine("Deserializing into `{0}`: `{1}`", responseType.Name, response);
            TResponse instance;
            try
            {
                instance = JsonConvert.DeserializeObject<TResponse>(response.Body, Serialization.Settings);
            }
            catch (JsonException exception)
            {
                instance = new TResponse{ParseException = exception};
            }
            instance.HttpResponse = response;
            return instance;
        }
    }
}