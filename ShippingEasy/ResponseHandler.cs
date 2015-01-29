using System;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;

namespace ShippingEasy
{
    public class ResponseHandler
    {
        public TResponse Build<TResponse>(HttpResponse response) where TResponse : new()
        {
            var responseType = typeof(TResponse);
            Debug.WriteLine("Deserializing into `{0}`: `{1}`", responseType.Name, response);
            var instance = JsonConvert.DeserializeObject<TResponse>(response.Body, Serialization.Settings);
            PopulateSpecialProperties(response, responseType, instance);
            return instance;
        }

        private static void PopulateSpecialProperties(HttpResponse response, Type responseType, object instance)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
            var propertyInfo = responseType.GetProperty("HttpResponse", flags);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(instance, response, null);
            }
        }
    }
}