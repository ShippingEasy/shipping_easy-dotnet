using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShippingEasy
{
    public class ResponseParser
    {
        public TResponse Parse<TResponse>(string response) where TResponse : new()
        {
            var instance = new TResponse();
            var json = JObject.Parse(response);
            foreach (var prop in typeof (TResponse).GetProperties())
            {
                if (prop.Name == "RawJson")
                {
                    prop.SetValue(instance, response, null);
                }
                else
                {
                    var jsonSubset = json.SelectToken(prop.Name.ToLower());
                    if (jsonSubset == null) continue;
                    Debug.WriteLine("Populating `{0}` as `{1}` from `{2}`", prop.Name, prop.PropertyType.Name, jsonSubset);
                    var value = JsonConvert.DeserializeObject(jsonSubset.ToString(), prop.PropertyType);
                    prop.SetValue(instance, value, null);
                }
            }
            return instance;
        }

        public static Order ParseOrder(string json)
        {
            return JsonConvert.DeserializeObject<Order>(json);
        }
    }
}