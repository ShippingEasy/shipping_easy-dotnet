using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShippingEasy
{
    public static class Serialization
    {
        static Serialization()
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new UnderscoreMappingResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateParseHandling = DateParseHandling.DateTimeOffset,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
            };
        }

        public static JsonSerializerSettings Settings { get; set; }

        class UnderscoreMappingResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return Regex.Replace(propertyName, "([a-z])([A-Z])", "$1_$2").ToLower();
            }
        }
    }
}