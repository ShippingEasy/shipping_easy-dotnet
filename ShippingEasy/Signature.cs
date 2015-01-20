using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ShippingEasy
{
    public class Signature
    {
        private readonly string _apiSecret;
        private readonly string _httpMethod;
        private readonly string _path;
        private readonly IDictionary<string, string> _parameters;
        private readonly string _body;

        public Signature(string apiSecret, string httpMethod, string path, IDictionary<string, string> parameters, string body = null)
        {
            _apiSecret = apiSecret;
            _httpMethod = httpMethod;
            _path = path;
            _parameters = parameters;
            _body = body;
        }

        public string ToPlainTextString()
        {
            var orderedParams = _parameters.OrderBy(param => param.Key)
                .Where(param => !String.IsNullOrWhiteSpace(param.Value))
                .Select(param => String.Format("{0}={1}", param.Key, param.Value));
            var combinedParameters = String.Join("&", orderedParams);

            return String.Join("&",
                new []{
                _httpMethod.ToUpper(),
                _path,
                combinedParameters,
                _body
                }.Where(component => !String.IsNullOrWhiteSpace(component))
            );
        }

        public override string ToString()
        {
            return ComputeHmacSha256Hash(_apiSecret, ToPlainTextString());
        }


        private static string ComputeHmacSha256Hash(string key, string data)
        {
            var encoding = new UTF8Encoding();
            var keyByte = encoding.GetBytes(key);
            var messageBytes = encoding.GetBytes(data);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                var hashmessage = hmacsha256.ComputeHash(messageBytes);
                return ToHexString(hashmessage);
            }
           
        }

        private static string ToHexString(byte[] bytes)
        {
            return bytes.Aggregate(new StringBuilder(),(builder, part) =>
                builder.AppendFormat("{0:x2}", part)).ToString();
        }

    }
}