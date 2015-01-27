using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ShippingEasy
{
    public static class Serialization
    {
        public static JsonSerializerSettings Settings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new UnderscoreMappingResolver(),
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                    DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                    TraceWriter = new DiagnosticsTraceWriter()
                };
            }
        }

        class UnderscoreMappingResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return Regex.Replace(propertyName, "([a-z])([A-Z])", "$1_$2").ToLower();
            }
        }
    }
}